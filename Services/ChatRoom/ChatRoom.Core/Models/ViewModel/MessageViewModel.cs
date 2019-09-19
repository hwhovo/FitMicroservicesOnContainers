using System;

namespace ChatRoom.Core.Models.ViewModel
{
    public class MessageViewModel
    {
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public long? ChatRoomId { get; set; }
        public string MessageText { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
