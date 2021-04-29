using System;
using System.Text;
using Domain.Services.Forum;
using Domain.Services.Repositories;
using Domain.Services.User;
using HotChocolate.AspNetCore;
using Infrastructure.EF.Context;
using Infrastructure.EF.GraphQL;
using Infrastructure.EF.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Neo4jClient;
using Newtonsoft.Json.Serialization;
using Services.Forum;
using Services.User;
using ZenCryptAPI.Graphql;

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
                options.UseSqlServer("Data Source=192.168.1.117,1433;Initial Catalog=DevPersonal;User Id=sa;Password=Reepje171;")
                    .UseLazyLoadingProxies());

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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            "testa"))
                    };
                });

            var neo4JClient =
                new GraphClient(new Uri("http://192.168.1.117:7474/"))
                {
                    JsonContractResolver = new CamelCasePropertyNamesContractResolver()
                };

            neo4JClient.ConnectAsync().Wait();

            services.AddHttpContextAccessor();

            services.AddSingleton(Configuration);
            services.AddSingleton<IGraphClient>(neo4JClient);
            services.AddScoped(typeof(ISQLRepository<>), typeof(SQLRepository<>));
            services.AddScoped(typeof(INeoRepository<>), typeof(NeoRepository<>));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IForumService, ForumService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddAuthorization();
            services.AddGraphQLServer()
                .AddQueryType<Query>().AddFiltering().AddSorting()
                .AddMutationType<Mutation>()
                .AddAuthorization()
                .AddErrorFilter<GraphQlErrorFilter>()
                .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);

            services.AddCors(o => o.AddPolicy("CurPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            UpdateDatabase(app);
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();

            app.UsePlayground();

            app.UseCors("CurPolicy");
            app.Use((context, next) =>
            {
                context.Items["__CorsMiddlewareInvoked"] = true;
                return next();
            });

            app.UseEndpoints(x => x.MapGraphQL());
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