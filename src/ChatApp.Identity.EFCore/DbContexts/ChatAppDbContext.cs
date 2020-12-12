using ChatApp.Identity.EFCore.DbContexts.Configuration;
using ChatApp.Identity.EFCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Identity.EFCore.DbContexts
{
    internal class ChatAppDbContext : IdentityDbContext<ChatUser>
    {
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region "Seed Data"

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            #endregion

            builder.ApplyConfiguration(new ChatAppDbContextCnfig.ChatRoomConfiuration());
            builder.ApplyConfiguration(new ChatAppDbContextCnfig.ChatUserConfiguration());
            builder.ApplyConfiguration(new ChatAppDbContextCnfig.MessageConfiguration());
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatRoomParticipant> ChatRoomParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}