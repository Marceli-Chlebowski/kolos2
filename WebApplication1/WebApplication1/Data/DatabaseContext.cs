using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Item> Items { get; set; }
    public DbSet<Title> Titles { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<CharacterTitle> CharacterTitles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>().HasData(new List<Item>
        {
            new Item
            {
                Id = 1,
                Name = "Sword",
                Weight = 15
            },
            new Item
            {
                Id = 2,
                Name = "Dagger",
                Weight = 5
            },
            new Item
            {
                Id = 3,
                Name = "Shield",
                Weight = 3
            }
        });

        modelBuilder.Entity<Title>().HasData(new List<Title>
        {
            new Title
            {
                Id = 1,
                Name = "La'Zel"
            },
            new Title
            {
                Id = 2,
                Name = "Astorion"
            }
        });

        modelBuilder.Entity<Character>().HasData(new List<Character>
        {
            new Character
            {
                Id = 1,
                FirstName = "Astorion",
                LastName = "Dasd",
                CurrentWeight = 23,
                MaxWeight = 120
            },
            new Character
            {
                Id = 2,
                FirstName = "La",
                LastName = "'zel",
                CurrentWeight = 8,
                MaxWeight = 60
            }
        });

        modelBuilder.Entity<CharacterTitle>().HasData(new List<CharacterTitle>
        {
            new CharacterTitle
            {
                CharacterId = 1,
                TitleId = 1,
                AcquiredAt = DateTime.Parse("01-01-2002")
            },
            new CharacterTitle
            {
                CharacterId = 2,
                TitleId = 1,
                AcquiredAt = DateTime.Parse("01-01-2004")
            },
            new CharacterTitle
            {
                CharacterId = 1,
                TitleId = 2,
                AcquiredAt = DateTime.Parse("01-01-2018")
            },
            new CharacterTitle
            {
                CharacterId = 2,
                TitleId = 2,
                AcquiredAt = DateTime.Parse("01-01-2020")
            }
        });

        modelBuilder.Entity<Backpack>().HasData(new List<Backpack>
        {
            new Backpack
            {
                CharacterId = 1,
                ItemId = 1,
                Amount = 1
            },
            new Backpack
            {
                CharacterId = 1,
                ItemId = 2,
                Amount = 1
            },
            new Backpack
            {
                CharacterId = 1,
                ItemId = 3,
                Amount = 1
            },
            new Backpack
            {
                CharacterId = 2,
                ItemId = 2,
                Amount = 1
            },
            new Backpack
            {
                CharacterId = 2,
                ItemId = 3,
                Amount = 1
            }
        });
    }
}