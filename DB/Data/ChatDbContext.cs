using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {
        }

        // establish the entities in DB.
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // defines the properties in table
            modelBuilder.Entity<Message>(tb =>
            {
                // define primary key
                tb.HasKey(col => col.MessageId);
                // auto increment id and generated when Add a register.
                tb.Property(col => col.MessageId).UseIdentityColumn().ValueGeneratedOnAdd();

                tb.Property(col => col.Text).HasMaxLength(200);
            });

            // defines the properties in table
            modelBuilder.Entity<User>(tb =>
            {
                // define primary key
                tb.HasKey(col => col.UserId);
                // define user name as index, and mark unique
                tb.HasIndex(col => col.UserName).IsUnique();
                // auto increment id and generated when Add a register.
                tb.Property(col => col.UserId).UseIdentityColumn().ValueGeneratedOnAdd();

                tb.Property(col => col.UserName).HasMaxLength(20);
                tb.Property(col => col.Password).HasMaxLength(100);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
