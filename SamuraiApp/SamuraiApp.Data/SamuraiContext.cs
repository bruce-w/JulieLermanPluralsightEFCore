using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbQuery<SamuraiStat> SamuraiStats { get; set; }

        //Logging
        public static readonly LoggerFactory MyConsoleLoggerFactory = new LoggerFactory(new[] {
            new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name
                                                        && level == LogLevel.Information, true)});

        public SamuraiContext()
        { }

        // ----- WebAP as Main Project (Dependency Injection) -----
        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
        { }


        // ----- SomeUI as Main Project -----
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(MyConsoleLoggerFactory)
                    .EnableSensitiveDataLogging(true)
                    .UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = SamuraiAppData; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new { s.SamuraiId, s.BattleId });

            modelBuilder.Entity<Battle>().Property(b => b.StartDate).HasColumnType("Date");
            modelBuilder.Entity<Battle>().Property(b => b.EndDate).HasColumnType("Date");

            //View
            modelBuilder.Query<SamuraiStat>()
                        .ToView("SamuraiBattleStats");
            //Shadow properties

            ///Mapping unconventionally named foreign key property
            /// Special syntax (parameterless WithOne, HFK<SecretIdentity>
            /// are because I have no Samurai navigation property
            //modelBuilder.Entity<Samurai>()
            //    .HasOne(i => i.SecretIdentity)
            //    .WithOne()
            //    .HasForeignKey<SecretIdentity>(i => i.SamuraiFK);

            //-------------- Shadow properties --------------

            ///Mapping nullable foreign key SamuraiId --- Shadow Property            
            //modelBuilder.Entity<Samurai>()
            //    .HasOne(s => s.SecretIdentity)
            //    .WithOne(i => i.Samurai).HasForeignKey<SecretIdentity>("SamuraiId");

            //modelBuilder.Entity<Samurai>().Property<DateTime>("Created");
            //modelBuilder.Entity<Samurai>().Property<DateTime>("LastModified");

            //adiciona as 2 shadow properties a todas as Entytis
            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => !e.IsOwned() && !e.IsQueryType))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("Created");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
            }

            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).ToTable("BetterNames");
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.GivenName).HasColumnName("GivenName");
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.SurName).HasColumnName("SurName");

            //modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName, b =>
            //{
            //    b.Property(p => p.GivenName).HasColumnName("GivenName");
            //    b.Property(p => p.SurName).HasColumnName("SurName");
            //});
        }
       
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var timestamp = DateTime.Now;

            foreach (var entry in ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) && !e.Metadata.IsOwned()))
            {
                entry.Property("LastModified").CurrentValue = timestamp;

                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = timestamp;
                }

                //Garantir que o workaround de PersonFullName funciona
                if (entry.Entity is Samurai)
                {
                    if (entry.Reference("BetterName").CurrentValue == null)
                    {
                        entry.Reference("BetterName").CurrentValue = PersonFullName.Empty();
                    }
                }
            }

            return base.SaveChanges();
        }

        // ----- Scalar Functions -----

        [DbFunction]
        public string EarliestBattleFoughtBySamurai(int samuraiId)
        {
            throw new Exception();
        }

        [DbFunction]
        public int DaysInBattle(DateTime start, DateTime end)
        {
            return (int)end.Subtract(start).TotalDays + 1;
        }
    }
}
