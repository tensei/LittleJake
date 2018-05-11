using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace LittleJake
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File(new JsonFormatter(renderMessage: true), "log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithExceptionDetails()
                .CreateLogger();

            try
            {
                await new JakeBot().StartAsync();
            }
            catch (Exception e)
            {
                Log.Information(e, "Bot is donezo");
            }
        }
    }
}