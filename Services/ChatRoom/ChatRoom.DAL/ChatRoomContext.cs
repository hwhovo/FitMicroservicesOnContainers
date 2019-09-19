using ChatRoom.Core.Entites;
using ChatRoom.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.DAL
{
    public class ChatRoomContext : DbContext
    {
        public ChatRoomContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserChatRoom> UserChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region -- model builder configurations --
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserChatRoomConfiguration());
            #endregion -- model builder configurations --
        }
    }
}
