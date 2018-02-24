using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;


namespace LittleSteve
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new JsonFormatter(renderMessage: true))
               // .WriteTo.File(new JsonFormatter(renderMessage: true),"log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithExceptionDetails()
                .CreateLogger();

            Log.Information("test");
            try
            {
              
                await new SteveBot().StartAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, "Shits Fucked Up");
            }
        }
    }
}