using System.Collections.Generic;

namespace BeardBook.Models.ProfileViewModels
{
    public class ProfileViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public int AvatarId { get; set; }
        public ICollection<UserViewModel> Users { get; set; }
    }
}