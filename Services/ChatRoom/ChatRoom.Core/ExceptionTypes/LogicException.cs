﻿using ChatRoom.Core.Enums;
using System;
using System.Runtime.Serialization;

namespace ChatRoom.Core.ExceptionTypes
{
    public class LogicException : Exception
    {
        public ExceptionMessage? MessageType
        {
            get
            {
                return _messageType;
            }
        }
        private readonly ExceptionMessage? _messageType;
        public LogicException()
        {

        }

        public LogicException(ExceptionMessage message)
        {
            _messageType = message;
        }

        public LogicException(ExceptionMessage _messageType, string message)
            : base($"{_messageType}: {message}")
        {

        }

        public LogicException(string message) : base(message)
        {
        }

        protected LogicException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
