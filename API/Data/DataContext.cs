using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLike>().HasKey(k => new{k.SourceUserId, k.TargetUserId});

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
        }
    }
}