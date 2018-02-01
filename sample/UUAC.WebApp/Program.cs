using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace UUAC.WebApp
{
    public class Program
    {
     
        static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) {

            var hostingConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
               .UseConfiguration(hostingConfig)
               .UseStartup<Startup>()
               .Build();
        }
           
    }
}
