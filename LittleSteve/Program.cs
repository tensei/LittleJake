using System;
using System.Threading.Tasks;
using Serilog;
using Serilog.Exceptions;

namespace LittleSteve
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                // .WriteTo.File(new JsonFormatter(renderMessage: true),"log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithExceptionDetails()
                .CreateLogger();
            try
            {
                await new SteveBot().StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
           
        }
    }
}