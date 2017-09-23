using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AIMAS.Data;
using AIMAS.Data.Inventory;
using AIMAS.Data.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AIMAS.API
{
  public class Startup
  {
    private ILogger log;

    public static IHostingEnvironment HostingEnvironment { get; set; }
    public static IConfiguration Configuration { get; set; }
    public static IServiceProvider ServiceProvider { get; set; }
    public static ILoggerFactory LoggerFactory { get; set; }


    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Json Settings
      services.AddMvc()
        .AddJsonOptions(options =>
        {
          options.SerializerSettings.Formatting = Formatting.None;
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
          options.SerializerSettings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new DefaultNamingStrategy() };
          options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
        });

      // Get Connection String
      var connection = Configuration.GetConnectionString("Backend-DB");
      // Add DB To Services
      services.AddEntityFrameworkNpgsql()
      .AddDbContext<AimasContext>(options =>
        options.UseNpgsql(connection, npgoptions =>
          npgoptions.MigrationsAssembly("AIMAS.API")
      ))
      .BuildServiceProvider();

      // Add Identity Services
      services.AddIdentity<UserModel_DB, RoleModel_DB>(options =>
      {
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<AimasContext>()
      .AddDefaultTokenProviders();

      // Add Auth Services
      services.AddAuthentication();
      services.AddAuthorization(options =>
      {
        //[Authorize(Policy = "RequireAdministratorRole")]
        options.AddPolicy("Admin", policy => policy.RequireRole(IdentityDB.Roles[0]));
        options.AddPolicy("Edit", policy => policy.RequireRole(IdentityDB.Roles[0], IdentityDB.Roles[1]));
        options.AddPolicy("view", policy => policy.RequireRole(IdentityDB.Roles[0], IdentityDB.Roles[1], IdentityDB.Roles[2]));
      });

      // Cookie Config
      services.ConfigureApplicationCookie(options =>
      {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
        options.Events.OnRedirectToLogin = context =>
        {
          context.Response.StatusCode = 401;
          return Task.CompletedTask;
        };
      });

      // Add application services.
      services.AddTransient<InventoryDB>();
      services.AddTransient<IdentityDB>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider sep, ILoggerFactory logger)
    {
      HostingEnvironment = env;
      ServiceProvider = sep;
      LoggerFactory = logger;

      log = LoggerFactory.CreateLogger<Startup>();

      // Midelware
      if (HostingEnvironment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      app.UseMvcWithDefaultRoute();

      // End

      if (Configuration.GetSection("Settings").GetValue<bool>("InitializeDB"))
        InitializeDB();
    }

    public void InitializeDB()
    {
      try
      {
        ServiceProvider.GetRequiredService<IdentityDB>().Initialize();

        ServiceProvider.GetRequiredService<InventoryDB>().Initialize();
      }
      catch (Exception ex)
      {
        log.LogError(ex, "An error occurred while seeding the database.");
      }
    }
  }
}
