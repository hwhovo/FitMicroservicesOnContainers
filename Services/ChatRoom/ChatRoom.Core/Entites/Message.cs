using ChatRoom.Core.Abstractions;
using System;

namespace ChatRoom.Core.Entites
{
    public class Message : EntityBase
    {
        public Message()
        {
            SentTime = DateTime.Now;
        }

        public long Id { get; set; }
        public string MessageText { get; set; }
        public long SenderId { get; set; }
        public long ChatRoomId { get; set; }
        public DateTime SentTime { get; set; }

        public User Sender { get; set; }
        public UserChatRoom ChatRoom { get; set; }
    }
}
