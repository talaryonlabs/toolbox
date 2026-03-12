using System.Diagnostics.Contracts;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Talaryon.Toolbox.Hosting.Api;
using Talaryon.Toolbox.Hosting.Api.Attributes;
using Talaryon.Toolbox.Hosting.Api.Filters;

namespace Talaryon.Toolbox.Hosting;

public static class HostingExtensions
{
    [Pure]
    public static string ToQueryString<T>(T data)
    {
        return string.Join("&", typeof(T)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(v => v.CanRead)
            .Select(v =>
            {
                var attr = v.GetCustomAttributes<QueryMemberAttribute>().FirstOrDefault();
                var name = (attr is not null ? attr.Name : v.Name) ?? throw new InvalidOperationException();
                var value = v.GetValue(data) ?? "";

                return $"{name.ToLower()}={HttpUtility.UrlEncode(value.ToString())}";
            }));
    }

    extension(WebApplicationBuilder applicationBuilder)
    {
        public WebApplication BuildAsApi(ApiHostingOptions options)
        {
            var app = applicationBuilder.Build();

            app.MapControllers();

            if (options.IsTokenAuthenticationEnabled)
            {
                app
                    .UseAuthentication()
                    .UseAuthorization();
            }

            app
                .UseRateLimiter()
                .UseResponseCompression()
                .Use((context, next) =>
                {
                    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                    context.Response.Headers.Append("X-Frame-Options", "DENY");
                    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                    context.Response.Headers.Append("Referrer-Policy", "no-referrer");
                    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
                    return next();
                })
                .Use((context, next) => context.Request.ContentLength > 1024 * 1024
                    ? Task.FromResult<object>(context.Response.StatusCode = 413)
                    : next());

            return app;
        }
    }

    extension(IServiceCollection services)
    {
        public IServiceCollection AddApiComponents(ApiHostingOptions options)
        {
            services.AddRateLimiter(rateOptions =>
            {
                rateOptions.AddFixedWindowLimiter("fixed", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = options.RateLimit;
                    opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = options.QueueLimit;
                });
            });

            if (options.IsTokenAuthenticationEnabled)
            {
                services
                    .AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
                    .AddBearerToken(bearerOptions =>
                    {
                        bearerOptions.Events = new BearerTokenEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
                                    return Task.CompletedTask;

                                var token = authHeader
                                    .ToString()
                                    .Replace("Bearer", "")
                                    .Trim();

                                if (!options.AccessTokens.Contains(token)) return Task.CompletedTask;

                                var claims = new List<Claim>
                                {
                                    new(ClaimTypes.Name, "ApiToken")
                                };
                                var identity = new ClaimsIdentity(claims, BearerTokenDefaults.AuthenticationScheme);
                                var principal = new ClaimsPrincipal(identity);

                                context.Principal = principal;
                                context.Success();

                                return Task.CompletedTask;
                            }
                        };
                        bearerOptions.Validate();
                    });
                services
                    .AddAuthorization();
            }

            services
                .AddMvcCore()
                .AddMvcOptions(mvcOptions => { mvcOptions.Filters.Add(new ApiExceptionFilter(options.MediaType)); });

            services
                .AddControllers();

            services.AddResponseCompression(compressionOptions =>
            {
                compressionOptions.EnableForHttps = true;
                compressionOptions.Providers.Add<BrotliCompressionProvider>();
                compressionOptions.Providers.Add<GzipCompressionProvider>();
                compressionOptions.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat([
                    options.MediaType.MediaType.ToString()
                ]);
            });

            return services;
        }
    }
}