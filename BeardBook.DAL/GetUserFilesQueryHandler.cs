using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using BeardBook.Entities;
using File = BeardBook.Entities.File;

namespace BeardBook.DAL
{
    public class GetUserFilesQueryHandler : IQueryHandler<GetUserFilesQuery, IEnumerable<FileResult>>
    {
        private readonly BeardBookDbContext _context;

        public GetUserFilesQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<FileResult> Handle(GetUserFilesQuery query)
        {
            var files = _context.Posts
                .Where(p => p.User.Id == query.UserId && p.MediaFiles.Count > 0)
                .SelectMany(post => post.MediaFiles
                    .Where(f => f.FileType == query.FileType))
                    .OrderByDescending(f => f.Created)
                .ToList();

            var filestreamService = new FilestreamService(_context);
            files.ForEach(f => f.Data = filestreamService.GetFileData(f.Id));

            if (query.FileType != FileType.Photo)
                return files.Select(f => new FileResult(f, new File()));

            var fileResults = new List<FileResult>();
            foreach (var file in files)
            {
                var image = (Bitmap)new ImageConverter().ConvertFrom(file.Data);
                var size = GetThumbnailSize(image);
                var thumbnailImage = image?.GetThumbnailImage(
                    size.Width,
                    size.Height,
                    () => false,
                    IntPtr.Zero);

                var stream = new MemoryStream();
                thumbnailImage?.Save(stream, ImageFormat.Png);
                var bytes = stream.ToArray();
                var thumbnail = new File
                {
                    ContentType = "image/png",
                    Data = bytes
                };
                fileResults.Add(new FileResult(file, thumbnail));
            }

            return fileResults;
        }

        private static Size GetThumbnailSize(Image original)
        {
            const int maxPixels = 64;

            var originalWidth = original.Width;
            var originalHeight = original.Height;

            var factor = originalWidth > originalHeight
                ? (double) maxPixels / originalWidth
                : (double) maxPixels / originalHeight;

            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }
    }

    public class FileResult
    {
        public FileResult(File file, File thumbnail)
        {
            File = file;
            Thumbnail = thumbnail;
        }
        public File File { get; private set; }
        public File Thumbnail { get; private set; }
    }
}
