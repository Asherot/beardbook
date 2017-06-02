using System.Linq;
using BeardBook.DAL;

namespace BeardBook.Commands
{
    public class RemoveFriendCommandHandler : ICommandHandler<RemoveFriendCommand>
    {
        private readonly BeardBookDbContext _context;

        public RemoveFriendCommandHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public void Handle(RemoveFriendCommand command)
        {
            var user = _context.Users.First(u => u.Id == command.UserId);
            var friend = _context.Users.First(u => u.Id == command.FriendId);

            user.Friends.Remove(friend);
            friend.Friends.Remove(user);

            var conversation = _context.Conversations
                .FirstOrDefault(c => c.Users.Any(u => u.Id == command.UserId)
                                  && c.Users.Any(u => u.Id == command.FriendId)
                                  && c.Users.Count == 2);
            
            if (conversation != null) conversation.Active = false;

            _context.SaveChanges();
        }
    }
}