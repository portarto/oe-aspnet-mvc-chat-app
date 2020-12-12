using ChatApp.Identity.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Identity.EFCore.DbContexts.Configuration
{
    internal static partial class ChatAppDbContextCnfig
    {
        internal sealed class ChatRoomConfiuration : IEntityTypeConfiguration<ChatRoom>
        {
            public void Configure(EntityTypeBuilder<ChatRoom> builder)
            {
                builder.HasKey(cr => cr.Id);

                builder
                    .HasMany(cr => cr.ChatRoomParticipants)
                    .WithOne(cr => cr.ChatRoom)
                    .HasPrincipalKey(cr => cr.Id)
                ;

                builder
                    .HasMany(cr => cr.Messages)
                    .WithOne(m => m.ChatRoom)
                    .HasPrincipalKey(cr => cr.Id)
                ;

                builder
                    .HasMany(cr => cr.ChatUsers)
                    .WithMany(cu => cu.ChatRooms)
                    .UsingEntity<ChatRoomParticipant>
                    (
                        c => c
                            .HasOne(crp => crp.User)
                            .WithMany(u => u.ChatRoomParticipants)
                            .HasForeignKey(crp => crp.UserId),
                        c => c
                            .HasOne(crp => crp.ChatRoom)
                            .WithMany(cr => cr.ChatRoomParticipants)
                            .HasForeignKey(crp => crp.ChatRoomId),
                        c => { c.HasKey(crp => new { crp.ChatRoomId, crp.UserId }); }
                    )
                ;
            }
        }
    }
}
