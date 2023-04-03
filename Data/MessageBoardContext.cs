using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Models;

namespace MessageBoard.Data
{
    public class MessageBoardContext : DbContext
    {
        public MessageBoardContext (DbContextOptions<MessageBoardContext> options)
            : base(options)
        {
        }

        public DbSet<MessageBoard.Models.Topic> Topic { get; set; } = default!;

        public DbSet<MessageBoard.Models.Comment> Comment { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>().HasMany(t => t.Comments).WithOne(c => c.Topic);
        }

        public DbSet<MessageBoard.Models.User> User { get; set; } = default!;

        public DbSet<MessageBoard.Models.Announcement> Announcement { get; set; } = default!;
    }
}
