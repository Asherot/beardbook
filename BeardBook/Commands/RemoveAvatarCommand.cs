namespace BeardBook.Commands
{
    public class RemoveAvatarCommand
    {
        public RemoveAvatarCommand(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}