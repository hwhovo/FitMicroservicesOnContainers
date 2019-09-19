using ChatRoom.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoom.DAL.EntityConfigurations
{
    public class MessageConfiguration
        : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.MessageText).HasMaxLength(255);

            builder.HasOne(x => x.Sender)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ChatRoom)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatRoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
