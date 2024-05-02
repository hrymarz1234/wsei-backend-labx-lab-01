using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Interfaces.Repository;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;
using Infrastructure.Memory.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using WebAPI.Validators;
using Infrastructure.EF;
using Infrastructure.Memory;
using Infrastructure.Services;
using Microsoft.OpenApi.Models;
using WebAPI.Configuration;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
            .AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddSingleton<IGenericRepository<Quiz, int>, MemoryGenericRepository<Quiz, int>>();
            builder.Services.AddSingleton<IGenericRepository<QuizItem, int>, MemoryGenericRepository<QuizItem, int>>();
            builder.Services.AddSingleton<IGenericRepository<QuizItemUserAnswer, string>, MemoryGenericRepository<QuizItemUserAnswer, string>>();
            builder.Services.AddSingleton<IQuizUserService, QuizUserService>();
            builder.Services.AddSingleton<IQuizAdminService, QuizAdminService>();
            /*builder.Services.AddScoped<IValidator<QuizItem>, QuizItemValidator>();
            builder.Services.AddTransient<IGenericGenerator<int>, IntGenerator>();
            builder.Services.AddDbContext<QuizDbContext>();
            builder.Services.AddTransient<IQuizUserService, QuizUserServiceEF>();*/

            builder.Services.AddSingleton<JwtSettings>();
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureJWT(new JwtSettings(builder.Configuration));
            builder.Services.ConfigureCors();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
              Enter 'Bearer' and then your token in the text input below.
              Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Quiz API",
                });
            });

            



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.AddUsers();

            app.MapControllers();

            app.Run();
        }
    }
}