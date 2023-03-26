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
    }
}
