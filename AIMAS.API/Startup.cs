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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AIMAS.Data.Inventory;
using AIMAS.Data.Identity;


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
          options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        });

      // Get Connection String
      var connection = Configuration.GetConnectionString("Backend-DB");
      // Add DB To Services
      services.AddEntityFrameworkNpgsql()
      .AddDbContext<InventoryContext>(options =>
        options.UseNpgsql(connection, npgoptions =>
          npgoptions.MigrationsAssembly("AIMAS.API")
      ))
      .AddDbContext<IdentityContext>(options =>
        options.UseNpgsql(connection, npgoptions =>
          npgoptions.MigrationsAssembly("AIMAS.API")
      ));

      // Add Identity Services
      services.AddIdentity<UserModel_DB, RoleModel_DB>(options =>
      {
        options.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<IdentityContext>()
      .AddDefaultTokenProviders();

      // Add Auth Services
      services.AddAuthentication();
      services.ConfigureApplicationCookie(options =>
      {
        options.LoginPath = "/test";
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

      logger.AddConsole(Configuration.GetSection("Logging"));
      logger.AddDebug();
      log = logger.CreateLogger<Startup>();


      if (HostingEnvironment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseAuthentication();

      app.UseMvcWithDefaultRoute();

      if (Configuration.GetSection("Settings").GetValue<bool>("InitializeDB"))
        InitializeDB();
    }

    public void InitializeDB()
    {
      try
      {
        ServiceProvider.GetRequiredService<InventoryDB>().Initialize();

        ServiceProvider.GetRequiredService<IdentityDB>().Initialize();
      }
      catch (Exception ex)
      {
        log.LogError(ex, "An error occurred while seeding the database.");
      }
    }
  }
}
