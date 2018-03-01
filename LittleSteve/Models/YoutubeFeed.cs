using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LittleSteve.Data.Entities;


namespace LittleSteve.Models
{
    public class YoutubeFeed
    {
        public IEnumerable<YoutubeVideo> YoutubeVideos { get; set; }
        
    }
}
