using ChatRoom.Core.Abstractions;
using System.Collections.Generic;

namespace ChatRoom.Core.Entites
{
    public class UserChatRoom : EntityBase
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
