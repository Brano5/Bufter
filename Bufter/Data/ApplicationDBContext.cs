using Bufter.Model;
using Bufter.Models;
using Microsoft.EntityFrameworkCore;

namespace Bufter.Data
{
	public class ApplicationDBContext : DbContext
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{

		}

		public DbSet<Room> Rooms { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<BuyLog> BuyLog { get; set; }
    }
}
