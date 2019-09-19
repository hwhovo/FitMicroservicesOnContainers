using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.Core.Entites;
using ChatRoom.Core.Enums;
using ChatRoom.Core.Models;
using ChatRoom.Core.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoom.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IUserChatRoomOperations _chatRoomOperations;

        public ChatRoomsController(IUserChatRoomOperations chatRoomOperations)
        {
            _chatRoomOperations = chatRoomOperations;
        }

        [HttpGet("{id}")]
        public async Task<Response> GetById(int id)
        {
            var chatRoom = await _chatRoomOperations.GetByIdAsync(id);

            return new Response
            {
                Result = new ChatRoomViewModel
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                },
                Status = ResponseStatus.Ok
            };
        }

        [HttpGet("{id}/Messages")]
        public async Task<Response> GetMessages(int id)
        {
            var messages = await _chatRoomOperations.GetMessages(id);

            return new Response
            {
                Result = messages,
                Status = ResponseStatus.Ok
            };
        }

        [HttpGet]
        public async Task<Response> GetAll()
        {
            var chatRoomList = await _chatRoomOperations.GetAllAsync();

            return new Response
            {
                Result = chatRoomList.Select(chatRoom => new ChatRoomViewModel
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                }),
                Status = ResponseStatus.Ok
            };
        }

        [HttpPost]
        public async Task<Response> AddChatRoom([FromBody]string Name)
        {
            var chatRoom = await _chatRoomOperations.CreateAsync(new UserChatRoom { Name = Name });

            return new Response
            {
                Result = new ChatRoomViewModel
                {
                    Id = chatRoom.Id,
                    Name = chatRoom.Name,
                },
                Status = ResponseStatus.Ok
            };
        }


    }
}