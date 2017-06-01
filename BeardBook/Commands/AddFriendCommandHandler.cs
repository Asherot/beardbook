using System.Linq;
using BeardBook.DAL;
using BeardBook.Entities;

namespace BeardBook.Commands
{
    public class AddFriendCommandHandler : ICommandHandler<AddFriendCommand>
    {
        private readonly BeardBookDbContext _context;

        public AddFriendCommandHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public void Handle(AddFriendCommand command)
        {
            var user = _context.Users.First(u => u.Id == command.UserId);
            var friend = _context.Users.First(u => u.Id == command.FriendId);
            user.Friends.Add(friend);
            friend.Friends.Add(user);

            var conversation = _context.Conversations
                .FirstOrDefault(c => c.Users.Count == 2
                                && c.Users.Any(u => u.Id == command.UserId)
                                && c.Users.Any(u => u.Id == command.FriendId));

            if (conversation == null)
            {
                _context.Conversations.Add(new Conversation
                {
                    Users = new[] { user, friend }
                }); 
            }
            else
            {
                conversation.Active = true;
            }

            _context.SaveChanges();
        }
    }
}