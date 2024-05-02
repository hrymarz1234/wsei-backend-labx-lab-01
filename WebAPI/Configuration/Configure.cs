using Infrastructure.EF.Entities;
using Infrastructure.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Jose;
using Newtonsoft.Json;

namespace WebAPI.Configuration
{
    public static class Configure
    {

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services
                .AddDbContext<QuizDbContext>()
                .AddIdentity<UserEntity, UserRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                })
                .AddEntityFrameworkStores<QuizDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
                opt.AddPolicy("Email", policy =>
                {
                    policy.RequireClaim("email");
                });
            });
            services
                .AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    if (jwtSettings.Secret != null)
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                            ClockSkew = TimeSpan.FromSeconds(60)
                        };
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenException))
                            {
                                context.Response.Headers.Add("Token-expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("401 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject("403 Not authorized");
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

        public static async void AddUsers(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRole>>();

                var userEmailExists = await userManager.FindByEmailAsync("karol@wsei.edu.pl");
                var userNameExists = await userManager.FindByNameAsync("karol");
                if (userEmailExists == null && userNameExists == null)
                {
                    var user = new UserEntity() { Email = "karol@wsei.edu.pl", UserName = "karol" };
                    string password = "silneHaslo123@"; 

                    var saved = await userManager.CreateAsync(user,password);

                    if (saved.Succeeded)
                    {
                        if (!await roleManager.RoleExistsAsync("USER"))
                        {
                            await roleManager.CreateAsync(new UserRole() { Name = "USER" });
                        }

                        user = await userManager.FindByEmailAsync("karol@wsei.edu.pl");

                        if (user != null && roleManager != null)
                        {
                            await userManager.AddToRoleAsync(user, "USER");
                        }
                    }
                    else
                    {
                        foreach (var error in saved.Errors)
                        {
                            Console.WriteLine($"Error: {error.Code}, Description: {error.Description}");
                        }
                    }
                }
            }
        }
    }
}
