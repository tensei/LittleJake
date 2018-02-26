using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LittleSteve.Data
{
    //This is only used for code first Migrations
    public class SteveBotDesignTimeContextFactory : IDesignTimeDbContextFactory<SteveBotContext>
    {
        public SteveBotContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            var options = new DbContextOptionsBuilder<SteveBotContext>();
         
            options.UseNpgsql(config.GetSection("ConnectionString").Value);
            return new SteveBotContext(options.Options);
        }
    }
}
