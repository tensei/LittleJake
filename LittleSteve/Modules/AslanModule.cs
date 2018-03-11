using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using LittleSteve.Extensions;
using LittleSteve.Models;
using LittleSteve.Preconditions;
using LittleSteve.Services;
using Microsoft.Extensions.Options;

namespace LittleSteve.Modules
{
    [Name("Aslan")]
    public class AslanModule : ModuleBase<SteveBotCommandContext>
    {
        private readonly ImgurService _imgurService;
        private readonly BotConfig _config;
        private readonly HttpClient _client;

        public AslanModule(ImgurService imgurService,BotConfig config, HttpClient client)
        {
            _imgurService = imgurService;
            _config = config;
            _client = client;
        }
        [Command("aslan",RunMode = RunMode.Async)]
        [Alias("cat")]
        [ThrottleCommand]
        [Summary("Get a picture of Aslan")]
        [Remarks("?aslan Do I look cute today")]
        public async Task Aslan([Remainder] string question = null)
        {
            
            var album = await _imgurService.GetAlbumAsync(_config.ImgurAlbumId);

            var image = album.ImgurData.Images.Random();

           
              var stream = await _client.GetStreamAsync(image.Link);
              await  Context.Channel.SendFileAsync(stream,image.Link.Split('/').Last());
            
        }

        
    }
}
