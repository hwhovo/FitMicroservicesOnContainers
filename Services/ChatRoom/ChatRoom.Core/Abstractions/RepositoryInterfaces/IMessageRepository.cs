using ChatRoom.Core.Entites;
using ChatRoom.Core.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.Core.Abstractions.RepositoryInterfaces
{
    public interface IMessageRepository : IEntityBaseRepository<Message>
    {
        Task<IEnumerable<MessageViewModel>> GetByChatRoomId(int chatRoomId);
    }
}
