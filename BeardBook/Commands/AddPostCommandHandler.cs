using System.Collections.Generic;
using System.IO;
using BeardBook.Controllers;
using BeardBook.DAL;
using BeardBook.Entities;
using File = BeardBook.Entities.File;

namespace BeardBook.Commands
{
    public class AddPostCommandHandler : ICommandHandler<AddPostCommand>
    {
        private readonly BeardBookDbContext _context;

        public AddPostCommandHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public void Handle(AddPostCommand command)
        {
            var post = new Post
            {
                UserId = command.UserId,
                Text = command.Text,
                MediaFiles = new List<File>()
            };

            foreach (var postedFile in command.UploadedFiles)
            {
                var file = new File
                {
                    Name = Path.GetFileName(postedFile.FileName),
                    ContentType = postedFile.ContentType,
                    FileType = FilesController.GetFileTypeFrom(postedFile.ContentType)
                };
                using (var reader = new BinaryReader(postedFile.InputStream))
                {
                    file.Data = reader.ReadBytes(postedFile.ContentLength);
                }
                post.MediaFiles.Add(file);
            }

            _context.Posts.Add(post);
            _context.SaveChanges();

            var filestreamService = new FilestreamService(_context);
            foreach (var file in post.MediaFiles)
            {
                filestreamService.SaveFileData(file);
            }
        }
    }
}
