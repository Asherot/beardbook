using System.Collections.Generic;
using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetUserFilesQueryHandler : IQueryHandler<GetUserFilesQuery, IEnumerable<File>>
    {
        private readonly BeardBookDbContext _context;

        public GetUserFilesQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<File> Handle(GetUserFilesQuery query)
        {
            var files = _context.Posts
                .Where(p => p.User.Id == query.UserId && p.MediaFiles.Count > 0)
                .SelectMany(post => post.MediaFiles
                    .Where(f => f.FileType == query.FileType))
                    .OrderByDescending(f => f.Created)
                .ToList();

            var filestreamService = new FilestreamService(_context);
            files.ForEach(f => f.Data = filestreamService.GetFileData(f.Id));

            return files;
        }
    }
}
