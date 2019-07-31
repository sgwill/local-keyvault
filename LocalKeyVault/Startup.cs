using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace LocalKeyVault
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Use(async (HttpContext context, Func<Task> next) =>
            //{
            //    //do work before the invoking the rest of the pipeline

            //    if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("/secrets"))
            //    {
            //        string key = context.Request.Path.Value.Replace("/", "").Replace("secrets", "");
            //        if (!string.IsNullOrEmpty(Configuration.GetValue<string>($"KeyVaultValues:{key}")))
            //        {
            //            var bundle = new SecretBundle()
            //            {
            //                Value = Configuration.GetValue<string>($"KeyVaultValues:{key}"),
            //                Id = key
            //            };

            //            await context.Response.WriteAsync(JsonConvert.SerializeObject(bundle));
            //        }
            //    }

            //    await next.Invoke(); //let the rest of the pipeline run

            //    //do work after the rest of the pipeline has run     
            //});
            app.UseLocalKeyVault();
        }
    }
}