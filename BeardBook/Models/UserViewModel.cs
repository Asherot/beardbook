namespace BeardBook.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Nickname { get; set; }
        public int AvatarId { get; set; }
        public bool IsFriend { get; set; }
    }
}