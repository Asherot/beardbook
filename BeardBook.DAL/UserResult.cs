using BeardBook.Entities;

namespace BeardBook.DAL
{
    public class UserResult
    {
        public UserResult(User user, bool isFriend)
        {
            User = user;
            IsFriend = isFriend;
        }

        public User User { get; private set; }
        public bool IsFriend { get; private set; }
    }
}