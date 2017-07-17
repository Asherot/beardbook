using System.Collections.Generic;
using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetWallPostsQueryHandler : IQueryHandler<GetWallPostsQuery, IEnumerable<PostResult>>
    {
        private readonly BeardBookDbContext _context;

        public GetWallPostsQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<PostResult> Handle(GetWallPostsQuery query)
        {
            var friendsIds = _context.Users
                .First(u => u.Id == query.UserId)
                .Friends
                .Select(f => f.Id);

            var posts = _context.Posts
                .Where(p => p.UserId == query.UserId || friendsIds.Any(f => f == p.UserId))
                .OrderByDescending(p => p.Created)
                .ToList();

            var filestreamService = new FilestreamService(_context);

            return posts
                .Select(post => new
                    {
                        post,
                        fileResults = post.MediaFiles
                                          .Where(f => f.FileType == FileType.Photo)
                                          .Select(file => new FileResult(
                                              file, FileService.CreateThumbnail(filestreamService.GetFileData(file.Id))))
                                          .ToList()
                    })
                .Select(t => new PostResult(t.post, t.fileResults))
                .ToList();
        }
    }
}