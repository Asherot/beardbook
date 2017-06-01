using BeardBook.DAL;
using BeardBook.Entities;
using BeardBook.Identity;
using Microsoft.AspNet.Identity;

namespace BeardBook.Commands
{
    public class UpdateUserInfoCommandHandler : ICommandHandler<UpdateUserInfoCommand>
    {
        private readonly BeardBookDbContext _context;
        private readonly AppUserManager _userManager;

        public UpdateUserInfoCommandHandler(BeardBookDbContext context, AppUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Handle(UpdateUserInfoCommand command)
        {
            var user = _userManager.FindById(command.UserId);
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Nickname = command.Nickname;
            _userManager.Update(user);

            if (command.UploadedAvatar == null || command.UploadedAvatar.ContentLength <= 0)
                return;

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
            _userManager.Update(user);

            var filestreamService = new FilestreamService(_context);
            filestreamService.SaveFileData(avatar);
        }
    }
}
