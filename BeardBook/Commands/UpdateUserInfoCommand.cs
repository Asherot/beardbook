using System.Web;

namespace BeardBook.Commands
{
    public class UpdateUserInfoCommand : ICommand
    {
        public UpdateUserInfoCommand(int userId, string firstName, string lastName, string nickname, HttpPostedFileBase uploadedAvatar)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
            UploadedAvatar = uploadedAvatar;
        }

        public int UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Nickname { get; private set; }
        public HttpPostedFileBase UploadedAvatar { get; private set; }
    }
}
