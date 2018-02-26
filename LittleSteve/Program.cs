using System;
using System.IO;
using System.Threading.Tasks;
using LittleSteve.Models;
using Newtonsoft.Json;
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
                Log.Error(e, "Shits Fucked Up");
            }
        }
    }
}