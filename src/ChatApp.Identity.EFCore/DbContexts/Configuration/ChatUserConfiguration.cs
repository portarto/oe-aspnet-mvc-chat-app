using ChatApp.Identity.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Identity.EFCore.DbContexts.Configuration
{
    internal static partial class ChatAppDbContextCnfig
    {
        internal sealed class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
        {
            public void Configure(EntityTypeBuilder<ChatUser> builder)
            {
                builder
                    .Property(cu => cu.FirstName)
                    .HasMaxLength(50)
                    .IsRequired(true)
                ;
                
                builder
                    .Property(cu => cu.LastName)
                    .HasMaxLength(50)
                    .IsRequired(true)
                ;
                
                builder
                    .Property(m => m.RegisteredAt)
                    .HasDefaultValueSql("getdate()")
                ;
            }
        }
    }
}
