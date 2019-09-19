using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Entites;

namespace ChatRoom.DAL.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(ChatRoomContext context) : base(context)
        {
        }
    }
}
