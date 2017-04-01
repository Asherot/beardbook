using System.ComponentModel;
using System.Web;
using BeardBook.Attributes;

namespace BeardBook.Models.ProfileViewModels
{
    public class EditViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        [DisplayName("Avatar")]
        [AllowedContentTypes(new[] { "image/png", "image/jpeg" })]
        [MaxFileSize(1024*1024, ErrorMessage = "Maximum allowed file size is 1MB")]
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}