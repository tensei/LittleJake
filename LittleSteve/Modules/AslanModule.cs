using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Extensions;
using LittleSteve.Models;
using LittleSteve.Services;
using Microsoft.Extensions.Options;

namespace LittleSteve.Modules
{

    public class AslanModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly ImgurService _imgurService;
        private readonly BotConfig _config;

        public AslanModule(ImgurService imgurService,BotConfig config)
        {
            _imgurService = imgurService;
            _config = config;
        }
        [Command("aslan",RunMode = RunMode.Async)]
        public async Task Aslan([Remainder] string question = null)
        {
            var album = await _imgurService.GetAlbumAsync(_config.ImgurAlbumId);

            var image = album.ImgurData.Images.Random();

            using (var client = new HttpClient())
            {
                var stream = await client.GetStreamAsync(image.Link);
              await  Context.Channel.SendFileAsync(stream,image.Link.Split('/').Last());
            }
        }
}
}
