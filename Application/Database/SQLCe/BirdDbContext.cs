namespace BlazorPoultryDashboard.Application.Database.SQLCe
{
    using BlazorPoultryDashboard.Models;
    using Microsoft.EntityFrameworkCore;

    public class BirdDbContext : DbContext
    {
        public DbSet<Bird> Birds { get; set; }
        public DbSet<Report> Reports { get; set; }

        public BirdDbContext(DbContextOptions<BirdDbContext> options) : base(options)
        {
        }
    }
}
