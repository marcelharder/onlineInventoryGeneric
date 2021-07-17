using api.DAL.models;
using Microsoft.EntityFrameworkCore;

namespace api.DAL
{

    public class dataContext : DbContext
    {
        public dataContext(DbContextOptions<dataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Class_ProductType> ProductTypes { get; set; }
        public DbSet<Class_Locations> Locations { get; set; }
        public DbSet<Class_Vendors> Vendors { get; set; }
        public DbSet<reorder_policy> ReorderPolicy { get; set; }
        public DbSet<rep> Reps { get; set; }
        public DbSet<Class_Product> Products { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Class_Transfer> Transfers { get; set; }
        public DbSet<Class_Product_Size> Valve_sizes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
           .HasOne(u => u.Recipient)
           .WithMany(m => m.MessagesReceived)
           .OnDelete(DeleteBehavior.Restrict);
        }
    }

}