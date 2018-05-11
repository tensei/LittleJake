using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LittleSteve.Data
{
    //This is only used for code first Migrations
    public class JakeBotDesignTimeContextFactory : IDesignTimeDbContextFactory<JakeBotContext>
    {
        public JakeBotContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();
            var options = new DbContextOptionsBuilder<JakeBotContext>();

            options.UseNpgsql(config.GetSection("ConnectionString").Value);
            return new JakeBotContext(options.Options);
        }
    }
}