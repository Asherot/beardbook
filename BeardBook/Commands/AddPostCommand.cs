using System.Collections.Generic;
using System.Web;

namespace BeardBook.Commands
{
    public class AddPostCommand : ICommand
    {
        public AddPostCommand(int userId, string text, IEnumerable<HttpPostedFileBase> uploadedFiles)
        {
            UserId = userId;
            Text = text;
            UploadedFiles = uploadedFiles;
        }

        public int UserId { get; private set; }
        public string Text { get; private set; }
        public IEnumerable<HttpPostedFileBase> UploadedFiles { get; private set; }
    }
}
