using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SkyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var args = System.Environment.GetCommandLineArgs();
            args.ToList().ForEach(w =>
            {
                System.Console.WriteLine(w);
            });
            int configIndex = args.ToList().FindIndex(p => { return p == "--f"; });
            string configPath = "";
            if (configIndex > 0)
            {
                if (args.Length < (configIndex + 1 + 1))
                {
                    throw new Exception("参数不正确");
                }
                configPath = args.ToList().ElementAt(configIndex + 1);
            }
            var builder = new ConfigurationBuilder()
                .AddCommandLine(args)
                .SetBasePath(env.ContentRootPath);
            if (System.IO.File.Exists(configPath) == false)
            {
                configPath = "appsettings.json";
            }
            Console.WriteLine(configPath);
            Console.WriteLine(env.EnvironmentName);
            builder.AddJsonFile(configPath, optional: true, reloadOnChange: true);
            builder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

            builder.AddJsonFile("skyapm.json", optional: true, reloadOnChange: true);

            var cfg = builder.Build();
            Configuration = cfg;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
