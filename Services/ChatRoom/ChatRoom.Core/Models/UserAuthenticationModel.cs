﻿namespace ChatRoom.Core.Models
{
    public class UserAuthenticationModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
