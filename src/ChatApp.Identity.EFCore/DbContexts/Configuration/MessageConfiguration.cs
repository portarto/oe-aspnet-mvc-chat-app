using ChatApp.Identity.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Identity.EFCore.DbContexts.Configuration
{
    internal static partial class ChatAppDbContextCnfig
    {
        internal sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
        {
            public void Configure(EntityTypeBuilder<Message> builder)
            {
                builder.HasKey(m => m.Id);

                builder
                    .HasOne(m => m.ChatRoom)
                    .WithMany(cr => cr.Messages)
                    .HasForeignKey(m => m.SentToChatRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                ;

                builder
                    .HasOne(m => m.User)
                    .WithMany()
                    .HasForeignKey(m => m.SentByUserId)
                ;

                builder
                    .Property(m => m.SentAt)
                    .HasDefaultValueSql("getdate()")
                ;
            }
        }
    }
}
