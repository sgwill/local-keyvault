using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalKeyVault
{
    public class LocalKeyVaultMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public LocalKeyVaultMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("/secrets"))
            {
                string key = context.Request.Path.Value.Replace("/", "").Replace("secrets", "");
                if (!string.IsNullOrEmpty(_config.GetValue<string>($"KeyVaultValues:{key}")))
                {
                    var bundle = new SecretBundle(value: _config.GetValue<string>($"KeyVaultValues:{key}"));

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(bundle));
                }
            } 

            await _next.Invoke(context);
        }
    }

    public static class LocalKeyVaultMiddlewareExtension
    {
        public static IApplicationBuilder UseLocalKeyVault(this IApplicationBuilder app)
        {
            app.UseMiddleware<LocalKeyVaultMiddleware>();

            return app;
        }
    }
}