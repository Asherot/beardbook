using System.Linq;
using BeardBook.DAL;
using BeardBook.Entities;

namespace BeardBook.Commands
{
    public class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly BeardBookDbContext _context;

        public UpdateUserInfoCommandHandler(BeardBookDbContext context)
        {
            _context = context;
        }

        public void Handle(UpdateUserInfoCommand command)
        {
            var user = _context.Users.First(u => u.Id == command.UserId);
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Nickname = command.Nickname;

            if (command.UploadedAvatar == null || command.UploadedAvatar.ContentLength <= 0)
                _context.SaveChanges();
            else
            {
                var avatar = new File
                {
                    Name = System.IO.Path.GetFileName(command.UploadedAvatar.FileName),
                    FileType = FileType.Avatar,
                    ContentType = command.UploadedAvatar.ContentType
                };
                using (var reader = new System.IO.BinaryReader(command.UploadedAvatar.InputStream))
                {
                    avatar.Data = reader.ReadBytes(command.UploadedAvatar.ContentLength);
                }

                user.Avatar = avatar;
                _context.SaveChanges();

                var filestreamService = new FilestreamService(_context);
                filestreamService.SaveFileData(avatar);
            }
        }
    }
}
