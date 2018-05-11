﻿using System;
using LittleSteve.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LittleSteve.Data
{
    public class JakeBotContext : DbContext
    {
        public JakeBotContext(DbContextOptions<JakeBotContext> options) : base(options)
        {
        }

        public DbSet<TwitterUser> TwitterUsers { get; set; }
        public DbSet<TwitterAlertSubscription> TwitterAlerts { get; set; }
        public DbSet<GuildOwner> GuildOwners { get; set; }
        public DbSet<TwitchStreamer> TwitchStreamers { get; set; }
        public DbSet<TwitchAlertSubscription> TwitchAlertSubscriptions { get; set; }
        public DbSet<Youtuber> Youtubers { get; set; }
        public DbSet<YoutubeAlertSubscription> YoutubeAlertSubscriptions { get; set; }
        public DbSet<UserBlacklist> UserBlacklists { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TwitterUser>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.LastTweetId);

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
                g.HasKey(x => new { x.DiscordId, x.GuildId });

                g.HasIndex(x => x.TwitterUserId);
            });

            modelBuilder.Entity<TwitchStreamer>(t =>
            {
                t.HasKey(x => x.Id);
                t.Property(x => x.SteamStartTime).HasDefaultValue(new DateTimeOffset());
                t.Property(x => x.StreamEndTime).HasDefaultValue(new DateTimeOffset());
                t.HasMany(x => x.Games).WithOne(x => x.TwitchStreamer).HasForeignKey(x => x.TwitchStreamerId);
                t.HasMany(x => x.TwitchAlertSubscriptions).WithOne(x => x.TwitchStreamer)
                    .HasForeignKey(x => x.TwitchStreamerId);
            });
            modelBuilder.Entity<TwitchAlertSubscription>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.TwitchStreamerId);
                t.HasOne(x => x.TwitchStreamer).WithMany(x => x.TwitchAlertSubscriptions)
                    .HasForeignKey(x => x.TwitchStreamerId);
            });
            modelBuilder.Entity<Youtuber>(t =>
            {
                t.HasKey(x => x.Id);
                t.Property(x => x.LatestVideoDate).HasDefaultValue(new DateTimeOffset());

                t.HasMany(x => x.YoutubeAlertSubscriptions).WithOne(x => x.Youtuber).HasForeignKey(x => x.YoutuberId);
            });

            modelBuilder.Entity<YoutubeAlertSubscription>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.YoutuberId);
                t.HasOne(x => x.Youtuber).WithMany(x => x.YoutubeAlertSubscriptions).HasForeignKey(x => x.YoutuberId);
            });

            modelBuilder.Entity<UserBlacklist>(b => { b.HasKey(x => new { x.GuildId, x.UserId }); });
            modelBuilder.Entity<Game>(g =>
            {
                g.HasKey(x => x.Id);
                g.HasOne(x => x.TwitchStreamer).WithMany(x => x.Games);
            });
        }
    }
}