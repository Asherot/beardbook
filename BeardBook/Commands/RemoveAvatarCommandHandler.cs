using System.Linq;
using BeardBook.DAL;

namespace BeardBook.Commands
{
    public class RemoveAvatarCommandHandler :ICommandHandler<RemoveAvatarCommand>
    {
        private readonly BeardBookDbContext _context;

        public RemoveAvatarCommandHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public void Handle(RemoveAvatarCommand command)
        {
            var avatar = _context.Users
                .First(u => u.Id == command.UserId)
                .Avatar;

            _context.Files.Remove(avatar);
            _context.SaveChanges();
        }
    }
}