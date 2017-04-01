using System.Linq;
using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Identity;
using Microsoft.AspNet.Identity;

namespace BeardBook.Commands
{
    public class SendMessageCommandHandler : ICommandHandler<SendMessageCommand>
    {
        private readonly BeardBookDbContext _context;
        private readonly AppUserManager _userManager;

        public SendMessageCommandHandler(BeardBookDbContext context, AppUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Handle(SendMessageCommand command)
        {
            var conversation = _context.Conversations
                .First(c => c.Id == command.ConversationId);

            var message = new Message
            {
                UserId = command.UserId,
                Text = command.Text
            };

            conversation.Messages.Add(message);
            conversation.LastUpdate = message.Created;

            var friendId = _context.Conversations
                .First(c => c.Id == command.ConversationId)
                .Users
                .First(u => u.Id != command.UserId)
                .Id;

            _userManager.SendEmail(friendId, "New Message!",
                "You\'ve just received a new message! To see it please click the following link:" +
                $"\n{command.Url}/{command.UserId}");

            _context.SaveChanges();
        }
    }
}