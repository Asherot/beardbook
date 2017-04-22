using System;

namespace BeardBook.Models.ProfileViewModels
{
    public class UploadedMediaViewModel
    {
        public int FileId { get; set; }
        public DateTime Created { get; set; }
        public string ThumbnailSrc { get; set; }
    }
}