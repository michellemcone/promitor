﻿using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Promitor.Core;

namespace Promitor.Scraper.Host
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            var httpPort = DetermineHttpPort();
            var endpointUrl = $"http://+:{httpPort}";

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel(kestrelServerOptions =>
                {
                    kestrelServerOptions.AddServerHeader = false;
                })
                .UseUrls(endpointUrl)
                .UseStartup<Startup>()
                .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static int DetermineHttpPort()
        {
            var rawConfiguredHttpPort = Environment.GetEnvironmentVariable(EnvironmentVariables.Runtime.HttpPort);
            if (int.TryParse(rawConfiguredHttpPort, out int configuredHttpPort))
            {
                return configuredHttpPort;
            }

            return 80;
        }
    }
}