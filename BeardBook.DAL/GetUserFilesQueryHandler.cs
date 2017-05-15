using System.Collections.Generic;
using System.Linq;
using BeardBook.Entities;

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

            if (query.FileType != FileType.Photo)
                return files.Select(f => new FileResult(f, null));

            var filestreamService = new FilestreamService(_context);

            return files
                .Select(file => new FileResult(
                    file,
                    FileService.CreateThumbnail(filestreamService.GetFileData(file.Id))))
                .ToList();
        }
    }
}
