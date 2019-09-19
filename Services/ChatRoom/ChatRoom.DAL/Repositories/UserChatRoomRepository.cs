using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Entites;

namespace ChatRoom.DAL.Repositories
{
    public class UserChatRoomRepository : EntityBaseRepository<UserChatRoom>, IUserChatRoomRepository
    {
        public UserChatRoomRepository(ChatRoomContext context) : base(context)
        {
        }
    }
}
