using System.Collections.Generic;
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

            _context.Conversations.Add(new Conversation
            {
                Users = new List<User>(new []{user, friend})
            });

            _context.SaveChanges();
        }
    }
}