using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
     IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
     IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
            .HasMany(u =>u.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId)
            .IsRequired();

            modelBuilder.Entity<AppRole>()
            .HasMany(u => u.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();

            modelBuilder.Entity<UserLike>().HasKey(k => new { k.SourceUserId, k.TargetUserId });

            modelBuilder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(w => w.LikedUsers)
            .HasForeignKey(x => x.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserLike>()
            .HasOne(s => s.TargetUser)
            .WithMany(w => w.LikedByUsers)
            .HasForeignKey(x => x.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
            .HasOne(a => a.Recipient)
            .WithMany(a => a.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
            .HasOne(a => a.Sender)
            .WithMany(a => a.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}