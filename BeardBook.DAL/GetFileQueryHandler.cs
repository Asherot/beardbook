using System;
using System.Linq;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetFileQueryHandler : IQueryHandler<GetFileQuery, File>
    {
        private readonly BeardBookDbContext _context;

        public GetFileQueryHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public File Handle(GetFileQuery query)
        {
            var file = _context.Files
                .First(f => f.Id == query.FileId);

            if (file.FileType != FileType.Avatar)
            {
                var ownerId = file.Post.UserId;

                var friends = _context.Users
                    .First(u => u.Id == query.UserId)
                    .Friends;

                if (query.UserId != ownerId && friends.All(f => f.Id != ownerId))
                    throw new InvalidOperationException(); 
            }

            var filestreamService = new FilestreamService(_context);
            file.Data = filestreamService.GetFileData(query.FileId);

            return file;
        }
    }
}
