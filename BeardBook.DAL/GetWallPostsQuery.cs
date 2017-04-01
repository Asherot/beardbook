using System.Collections.Generic;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetWallPostsQuery : IQuery<IEnumerable<Post>>
    {
        public GetWallPostsQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
