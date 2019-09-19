using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.Core.Entites;
using ChatRoom.Core.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.DAL.Repositories
{
    public class MessageRepository : EntityBaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ChatRoomContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MessageViewModel>> GetByChatRoomId(int chatRoomId)
        {
            return await Context.Messages.Where(x => x.ChatRoomId == chatRoomId)
                                         .Select(x => new MessageViewModel
                                         {
                                             ChatRoomId = x.ChatRoomId,
                                             MessageText = x.MessageText,
                                             SentDate = x.SentTime,
                                             UserId = x.SenderId,
                                             UserName = x.Sender.FirstName
                                         }).ToListAsync();
        }
    }
}
