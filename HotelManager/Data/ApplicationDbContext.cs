namespace HotelManager.Data
{
    using HotelManager.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            if (!Database.IsInMemory())
            {
                Database.Migrate();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
