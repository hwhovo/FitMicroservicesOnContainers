using System;

namespace ChatRoom.Core.Abstractions
{
    public interface IDbTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
