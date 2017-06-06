using System.Collections.Generic;
using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class GetConversationsUsersQuery : IQuery<IEnumerable<User>>
    {
        public GetConversationsUsersQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}
