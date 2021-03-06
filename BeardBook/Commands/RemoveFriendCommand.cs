﻿namespace BeardBook.Commands
{
    public class RemoveFriendCommand : ICommand
    {
        public RemoveFriendCommand(int userId, int friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }

        public int UserId { get; private set; } 
        public int FriendId { get; private set; }
    }
}