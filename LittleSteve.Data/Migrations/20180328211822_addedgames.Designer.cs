﻿// <auto-generated />
using LittleSteve.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace LittleSteve.Data.Migrations
{
    [DbContext(typeof(SteveBotContext))]
    [Migration("20180328211822_addedgames")]
    partial class addedgames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("LittleSteve.Data.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("EndTime");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("StartTime");

                    b.Property<long>("TwitchStreamerId");

                    b.HasKey("Id");

                    b.HasIndex("TwitchStreamerId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.GuildOwner", b =>
                {
                    b.Property<long>("DiscordId");

                    b.Property<long>("GuildId");

                    b.Property<long>("TwitchStreamerId");

                    b.Property<long>("TwitterUserId");

                    b.Property<string>("YoutuberId");

                    b.HasKey("DiscordId", "GuildId");

                    b.HasIndex("TwitterUserId");

                    b.ToTable("GuildOwners");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.TwitchAlertSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("DiscordChannelId");

                    b.Property<long>("MessageId");

                    b.Property<bool>("ShouldPin");

                    b.Property<long>("TwitchStreamerId");

                    b.HasKey("Id");

                    b.HasIndex("TwitchStreamerId");

                    b.ToTable("TwitchAlertSubscriptions");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.TwitchStreamer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset>("SteamStartTime")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<DateTimeOffset>("StreamEndTime")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

                    b.HasKey("Id");

                    b.ToTable("TwitchStreamers");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.TwitterAlertSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("DiscordChannelId");

                    b.Property<long>("TwitterUserId");

                    b.HasKey("Id");

                    b.HasIndex("TwitterUserId");

                    b.ToTable("TwitterAlertSubscriptions");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.TwitterUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("LastTweetId");

                    b.Property<string>("Name");

                    b.Property<string>("ScreenName");

                    b.HasKey("Id");

                    b.HasIndex("LastTweetId");

                    b.ToTable("TwitterUsers");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.UserBlacklist", b =>
                {
                    b.Property<long>("GuildId");

                    b.Property<long>("UserId");

                    b.HasKey("GuildId", "UserId");

                    b.ToTable("UserBlacklists");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.YoutubeAlertSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("DiscordChannelId");

                    b.Property<long>("MessageId");

                    b.Property<string>("YoutuberId");

                    b.HasKey("Id");

                    b.HasIndex("YoutuberId");

                    b.ToTable("YoutubeAlertSubscriptions");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.Youtuber", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("LatestVideoDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Youtubers");
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.Game", b =>
                {
                    b.HasOne("LittleSteve.Data.Entities.TwitchStreamer", "TwitchStreamer")
                        .WithMany("Games")
                        .HasForeignKey("TwitchStreamerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.TwitchAlertSubscription", b =>
                {
                    b.HasOne("LittleSteve.Data.Entities.TwitchStreamer", "TwitchStreamer")
                        .WithMany("TwitchAlertSubscriptions")
                        .HasForeignKey("TwitchStreamerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.TwitterAlertSubscription", b =>
                {
                    b.HasOne("LittleSteve.Data.Entities.TwitterUser", "User")
                        .WithMany("TwitterAlertSubscriptions")
                        .HasForeignKey("TwitterUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LittleSteve.Data.Entities.YoutubeAlertSubscription", b =>
                {
                    b.HasOne("LittleSteve.Data.Entities.Youtuber", "Youtuber")
                        .WithMany("YoutubeAlertSubscriptions")
                        .HasForeignKey("YoutuberId");
                });
#pragma warning restore 612, 618
        }
    }
}
