﻿namespace BeardBook.Commands
{
    public class AddFriendCommand
    {
        public AddFriendCommand(int userId, int friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }

        public int UserId { get; private set; }
        public int FriendId { get; private set; }
    }
}