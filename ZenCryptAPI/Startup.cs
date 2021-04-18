using Domain.Services;
using Infrastructure.EF;
using Infrastructure.EF.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Domain.Services.Forum;
using Domain.Services.Repositories;
using Domain.Services.User;
using Infrastructure.EF.Repositories;
using Microsoft.Extensions.Options;
using Neo4jClient;
using Newtonsoft.Json.Serialization;
using Services.Forum;
using Services.User;

namespace ZenCryptAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EntityContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("ASPNETCORE_SQL_CONNECTION_STRING") ?? throw new InvalidOperationException("No sql connection string provided!")).UseLazyLoadingProxies());

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("ASPNETCORE_JWT_TOKEN") ?? throw new InvalidOperationException("No jwt token provided!")))
                    };
                });

            var neo4JClient = new GraphClient(new Uri(Environment.GetEnvironmentVariable("ASPNETCORE_NEO_CONNECTION_STRING") ?? throw new InvalidOperationException("No neo4j connection string provided!"))) 
            {
                JsonContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            neo4JClient.ConnectAsync().Wait();

            services.AddSingleton(Configuration);
            services.AddSingleton<IGraphClient>(neo4JClient);
            services.AddScoped(typeof(ISQLRepository<>), typeof(SQLRepository<>));
            services.AddScoped(typeof(INeoRepository<>), typeof(NeoRepository<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();


            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ZenCryptAPI", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddCors(o => o.AddPolicy("CurPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZenCryptAPI v1"));

            UpdateDatabase(app);

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseCors("CurPolicy");
            app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // Automatically updates database on startup
        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<EntityContext>();
            context?.Database.Migrate();
        }
    }
}
