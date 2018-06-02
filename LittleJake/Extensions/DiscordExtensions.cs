using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Discord;
using Discord.Commands;
using Humanizer;
using LittleJake.Preconditions;
using LittleJake.Utilities;

namespace LittleJake.Extensions
{
    //https://github.com/AntiTcb/DiscordBots/blob/vs17-convert/src/DiscordBCL/Extensions/DiscordExtensions.cs
    public static class DiscordExtensions
    {
        public static bool CanExecute(this CommandInfo cmd, ICommandContext ctx, IServiceProvider provider)
        {
            return cmd.Preconditions.Any(x =>
                x.GetType() != typeof(RequireOwnerOrAdminAttribute) && x.GetType() != typeof(RequireOwnerAttribute));
        }

        public static bool CanExecute(this ModuleInfo module, ICommandContext ctx, IServiceProvider provider)
        {
            return module.Commands.Any(c => c.CanExecute(ctx, provider));
        }

        public static Embed GetEmbed(this IEnumerable<ModuleInfo> mods, SocketCommandContext ctx,
            IServiceProvider provider)
        {
            var availModules = mods.Where(m => m.CanExecute(ctx, provider));

            var eb = new EmbedBuilder
            {
                Title = "Help",
                Description = "Commands available to you:",
                Footer = new EmbedFooterBuilder
                {
                    Text = $"This message will delete itself in 60 seconds."
                },
                Author = new EmbedAuthorBuilder
                {
                    Name = ctx.Guild.CurrentUser.Nickname ?? ctx.Guild.CurrentUser.Username,
                    IconUrl = ctx.Client.CurrentUser.GetAvatarUrl() ?? ""
                },
                Color = new Color(0x81DAF5)
            };

            foreach (var m in availModules)
            {
                eb.AddField(m.Name, m.GetHelpString(ctx, provider));
            }

            // eb.AddField("Bot", "?bot ----- Bot Stuff");
            return eb.Build();
        }

        public static Embed GetEmbed(this ModuleInfo mod, SocketCommandContext ctx, IServiceProvider provider)
        {
            var eb = new EmbedBuilder
            {
                Title = mod.Name.Transform(To.TitleCase),
                Description = "Commands:",
                Author = new EmbedAuthorBuilder
                {
                    Name = ctx.Guild.CurrentUser.Nickname ?? ctx.Guild.CurrentUser.Username,
                    IconUrl = ctx.Client.CurrentUser.GetAvatarUrl() ?? ""
                },
                Color = new Color(0x81DAF5)
            };

            var availableCommands = mod.Commands.Distinct(new CommandNameComparer())
                .Where(c => c.CanExecute(ctx, provider));
            foreach (var c in availableCommands)
            {
                eb.AddField(c.Aliases.Humanize(), $"__{c.Summary}__ - *{c.Remarks}*");
            }

            return eb.Build();
        }

        public static string GetHelpString(this ModuleInfo mod, ICommandContext ctx, IServiceProvider provider)
        {
            var availableCommands = mod.Commands.Distinct(new CommandNameComparer())
                .Where(c => c.CanExecute(ctx, provider))
                .Select(c => $"?{c.Aliases.First()} ----- {c.Summary ?? "No summary available."}");

            return string.Join("\n", availableCommands);
        }

        public static Embed GetEmbed(this CommandInfo cmd, SocketCommandContext ctx)
        {
            var aliases = cmd.Aliases.Any(a => a != cmd.Name)
                ? $"**Aliases:** {string.Join(", ", cmd.Aliases.Where(a => a != cmd.Name))}\n\n"
                : "";
            var name = cmd.Aliases.First().Split().Last()
                .Equals(cmd.Name, StringComparison.CurrentCultureIgnoreCase)
                ? cmd.Aliases.FirstOrDefault()
                : cmd.Name;
            var parameters = string.Join(" ", cmd.Parameters.Select(p => $"{p.Name}"));
            var signature = $"\n\n\tSyntax: `?{name} {parameters}`";
            var example = $"\n\tExample: `{cmd.Remarks ?? "No Remarks"}`";

            var eb = new EmbedBuilder
            {
                Title = cmd.Aliases.First().Transform(To.TitleCase),
                Description = $"{aliases}**__{cmd.Summary}__**{signature}{example}",
                Author = new EmbedAuthorBuilder
                {
                    Name = ctx.Guild.CurrentUser.Nickname ?? ctx.Guild.CurrentUser.Username,
                    IconUrl = ctx.Client.CurrentUser.GetAvatarUrl() ?? ""
                },
                Color = new Color(0x81DAF5),
                Fields = cmd.GetParameterFields().ToList(),
                Footer = new EmbedFooterBuilder
                {
                    Text = "*: Optional parameter."
                }
            };

            return eb.Build();
        }

        public static IEnumerable<EmbedFieldBuilder> GetParameterFields(this CommandInfo cmd)
        {
            var fieldValue = "\u200B";
            foreach (var p in cmd.Parameters)
            {
                var pName = p.Name.Humanize();

                if (p.IsOptional)
                {
                    fieldValue = $"[{pName}* = {p.DefaultValue}{(p.IsRemainder ? "...]" : "]")} - {p.Summary}";
                }
                else
                {
                    fieldValue = $"<{pName} {(p.IsRemainder ? "...>" : ">")} {p.Summary}";
                }

                var field = new EmbedFieldBuilder
                {
                    Name = pName,
                    Value = fieldValue
                };
                yield return field;

                if (!p.Type.GetTypeInfo().IsEnum)
                {
                    continue;
                }

                var enumValues = Enum.GetNames(p.Type).Cast<string>().OrderBy(x => x);
                var enumField = new EmbedFieldBuilder
                {
                    Name = $"Possible values of {pName}:",
                    Value = enumValues.Humanize()
                };
                yield return enumField;
            }
        }
    }
}