using ChatRoom.Core.Abstractions;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.Core.Entites;
using ChatRoom.Core.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoom.BLL.Operations
{
    public class UserChatRoomOperations : IUserChatRoomOperations
    {
        private readonly IRepositoryManager _repositoryManager;

        public UserChatRoomOperations(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<UserChatRoom> CreateAsync(UserChatRoom chatRoom)
        {
            var chatRoomEntity = _repositoryManager.UserChatRooms.Add(chatRoom);
            await _repositoryManager.CompleteAsync();

            return chatRoomEntity;
        }

        public MessageViewModel AddMessage(MessageViewModel message)
        {
            var messageEntity = _repositoryManager.Messages.Add(new Message
            {
                ChatRoomId = message.ChatRoomId.Value,
                MessageText = message.MessageText,
                SenderId = message.UserId.Value
            });
            _repositoryManager.Complete();

            var user = _repositoryManager.Users.GetSingle(x => x.Id == message.UserId);

            return new MessageViewModel
            {
                ChatRoomId = messageEntity.ChatRoomId,
                MessageText = messageEntity.MessageText,
                SentDate = messageEntity.SentTime,
                UserId = messageEntity.SenderId,
                UserName = user.FirstName
            };
        }

        public async Task<IEnumerable<UserChatRoom>> GetAllAsync()
        {
            return await _repositoryManager.UserChatRooms.FindAsync();
        }

        public async Task<UserChatRoom> GetByIdAsync(int id)
        {
            return await _repositoryManager.UserChatRooms.GetSingleAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<MessageViewModel>> GetMessages(int id)
        {
            return await _repositoryManager.Messages.GetByChatRoomId(id);
        }
    }
}
