using Domain.business_entities.entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase;

public class SqLiteDbContext : DbContext
{
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<User> Users { get; set; }

    public SqLiteDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = DomenChat.db");
    }
}
