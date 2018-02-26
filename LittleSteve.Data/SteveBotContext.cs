using System;
using LittleSteve.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LittleSteve.Data
{
    public class SteveBotContext : DbContext
    {
        public SteveBotContext(DbContextOptions<SteveBotContext> options) : base(options)
        {
            DateCreated = DateTime.Now;
        }

        public DbSet<TwitterUser> TwitterUsers { get; set; }
        public DbSet<TwitterAlert> TwitterAlerts { get; set; }
        public DbSet<GuildOwner> GuildOwners { get; set; }
        public DateTime DateCreated { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TwitterUser>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.LastTweetId);
                t.HasMany(x => x.GuildOwners).WithOne(x => x.TwitterUser).HasForeignKey(x => x.TwitterUserId);
                t.HasMany(x => x.TwitterAlerts).WithOne(x => x.User).HasForeignKey(x => x.TwitterUserId);
            });

            modelBuilder.Entity<TwitterAlert>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.TwitterUserId);
                t.HasOne(x => x.User).WithMany(x => x.TwitterAlerts).HasForeignKey(x => x.TwitterUserId);
            });

            modelBuilder.Entity<GuildOwner>(g =>
            {
                g.HasKey(x => new {x.DiscordId, x.GuildId});
                g.HasIndex(x => x.TwitterUserId);
                g.HasOne(x => x.TwitterUser).WithMany(x => x.GuildOwners).HasForeignKey(x => x.TwitterUserId);
            });
        }

        
    }
}