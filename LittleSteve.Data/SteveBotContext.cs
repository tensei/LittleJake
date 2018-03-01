using System;
using LittleSteve.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LittleSteve.Data
{
    public class SteveBotContext : DbContext
    {
        public SteveBotContext(DbContextOptions<SteveBotContext> options) : base(options)
        {
           
        }

        public DbSet<TwitterUser> TwitterUsers { get; set; }
        public DbSet<TwitterAlertSubscription> TwitterAlerts { get; set; }
        public DbSet<GuildOwner> GuildOwners { get; set; }
        public DbSet<TwitchStreamer> TwitchStreamers { get; set; }
        public DbSet<TwitchAlertSubscription> TwitchAlertSubscriptions { get; set; }
        public DbSet<Youtuber> Youtubers { get; set; }
        public DbSet<YoutubeAlertSubscription> YoutubeAlertSubscriptions { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TwitterUser>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.LastTweetId);
                t.HasMany(x => x.GuildOwners).WithOne(x => x.TwitterUser).HasForeignKey(x => x.TwitterUserId);
                t.HasMany(x => x.TwitterAlertSubscriptions).WithOne(x => x.User).HasForeignKey(x => x.TwitterUserId);
            });

            modelBuilder.Entity<TwitterAlertSubscription>(t =>
            {
                t.ToTable("TwitterAlertSubscriptions");
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.TwitterUserId);
                t.HasOne(x => x.User).WithMany(x => x.TwitterAlertSubscriptions).HasForeignKey(x => x.TwitterUserId);
            });

            modelBuilder.Entity<GuildOwner>(g =>
            {
                g.HasKey(x => new {x.DiscordId, x.GuildId});
                g.HasIndex(x => x.TwitterUserId);
                g.HasOne(x => x.TwitterUser).WithMany(x => x.GuildOwners).HasForeignKey(x => x.TwitterUserId);
            });

            modelBuilder.Entity<TwitchStreamer>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasMany(x => x.GuildOwners).WithOne(x => x.TwitchStreamer).HasForeignKey(x => x.TwitchStreamerId);
                t.HasMany(x => x.TwitchAlertSubscriptions).WithOne(x => x.TwitchStreamer).HasForeignKey(x => x.TwitchStreamerId);
            });
            modelBuilder.Entity<TwitchAlertSubscription>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.TwitchStreamerId);
                t.HasOne(x => x.TwitchStreamer).WithMany(x => x.TwitchAlertSubscriptions).HasForeignKey(x => x.TwitchStreamerId);
            });
            modelBuilder.Entity<Youtuber>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasMany(x => x.GuildOwners).WithOne(x => x.Youtuber).HasForeignKey(x => x.YoutuberId);
                t.HasMany(x => x.YoutubeAlertSubscriptions).WithOne(x => x.Youtuber).HasForeignKey(x => x.YoutuberId);
            });

            modelBuilder.Entity<YoutubeAlertSubscription>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.YoutuberId);
                t.HasOne(x => x.Youtuber).WithMany(x => x.YoutubeAlertSubscriptions).HasForeignKey(x => x.YoutuberId);
            });
        }

        
    }
}