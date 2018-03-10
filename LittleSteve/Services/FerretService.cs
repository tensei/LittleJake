using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LittleSteve.Models;
using Newtonsoft.Json;

namespace LittleSteve.Services
{
    public class FerretService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public FerretService()
        {
            
        }

        public async Task<FerretPicture> GetFerretPicture()
        {
            var response = await _httpClient.GetStringAsync("https://polecat.me/api/ferret");

            return JsonConvert.DeserializeObject<FerretPicture>(response);
        }
    }
}
