using System;
using System.Collections.Generic;

namespace BeardBook.Models
{
    public class PostViewModel
    {
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string UserDisplayName { get; set; }
        public int AvatarId { get; set; }
        public ICollection<int> PhotosIds { get; set; }
        public ICollection<string> VideosSrcs { get; set; }
    }
}