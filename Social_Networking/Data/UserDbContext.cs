using Microsoft.EntityFrameworkCore;
using Social_Networking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Social_Networking.Data
{
    internal class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Posts> Post { get; set; }

        public DbSet<Friends> Friends { get; set; }
        public DbSet<FollowUsers> Follows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SocialNetworkDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
    }
}
