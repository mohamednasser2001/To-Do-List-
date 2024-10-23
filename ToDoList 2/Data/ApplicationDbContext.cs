using Microsoft.EntityFrameworkCore;
using ToDoList_2.Models;

namespace ToDoList_2.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Items> items { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", true, true)
           .Build()
           .GetConnectionString("DefualtConection");
           optionsBuilder.UseSqlServer(builder);
        }
    }
}
