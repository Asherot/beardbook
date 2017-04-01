using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace BeardBook.Models.HomeViewModels
{
    public class HomeViewModel
    {
        [DisplayName("Add files")]
        public ICollection<HttpPostedFileBase> MediaFiles { get; set; }
        public string PostText { get; set; }
        public ICollection<PostViewModel> PostViewModels { get; set; }
    }
}