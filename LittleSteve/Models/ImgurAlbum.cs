using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LittleSteve.Models
{
    public class ImgurAlbum
    {
        [JsonProperty("data")]
        public Data ImgurData { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }
    public class Data
    {
        [JsonProperty("images")]
        public List<Image> Images { get; set; }
    }
}
