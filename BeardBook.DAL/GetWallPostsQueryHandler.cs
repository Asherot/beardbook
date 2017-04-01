using System.Collections.Generic;
using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetWallPostsQueryHandler : IQueryHandler<GetWallPostsQuery, IEnumerable<Post>>
    {
        private readonly BeardBookDbContext _context;

        public GetWallPostsQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Handle(GetWallPostsQuery query)
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
            foreach (var post in posts)
                foreach (var file in post.MediaFiles)
                    file.Data = filestreamService.GetFileData(file.Id);

            return posts;
        }
    }
}