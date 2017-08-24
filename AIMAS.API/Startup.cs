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
using AIMAS.Data.Inventory;
using AIMAS.Data.Identity;

namespace AIMAS.API
{
  public class Startup
  {
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
      services.AddMvc();

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

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider sep, ILoggerFactory logger)
    {
      HostingEnvironment = env;
      ServiceProvider = sep;
      LoggerFactory = logger;

      if (HostingEnvironment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseMvc();

      if (Configuration.GetSection("Settings").GetValue<bool>("InitializeDB"))
        InitializeDB();
    }

    public static void InitializeDB()
    {
      try
      {
        InventoryContext.Initialize(ServiceProvider.GetRequiredService<InventoryContext>());
        IdentityContext.Initialize(ServiceProvider.GetRequiredService<IdentityContext>());
      }
      catch (Exception ex)
      {
        var logger = LoggerFactory.CreateLogger<Startup>();
        logger.LogError(ex, "An error occurred while seeding the database.");
      }
    }
  }
}
