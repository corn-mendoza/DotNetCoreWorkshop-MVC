using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Pivotal.Helper;

namespace DotNetCoreWorkshop_MVC
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
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAd(options => Configuration.Bind("AzureAd", options))
            .AddCookie();

            services.AddMvc();

            // Use the Bound Service for connection string if it is found in a User Provided Service
            string dbString = Configuration.GetConnectionString("AttendeeContext");
            CFEnvironmentVariables _env = new CFEnvironmentVariables();
            var _connect = _env.getConnectionStringForDbService("user-provided", "AttendeeContext");
            if (!string.IsNullOrEmpty(_connect))
            {
                Console.WriteLine($"Using bound service connection string for data: {_connect}");
                dbString = _connect;
            }
            else
            {
                Console.WriteLine($"Using connection string from appsetings.json");
            }

            services.AddDbContext<AttendeeContext>(options =>
                    options.UseSqlServer(dbString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Perform some database initialisation.
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AttendeeContext>();

                // Database.Migrate() will perform a migration of the database. This will ensure that the target database
                // is in sync with current context model snapshot found in the Migrations folder.
                // The alternative is to use EnsureCreated(). This will create a database and tables, if not existent on server.
                // However, consequently this skips migration altogether. Future migrations will not be possible
                // and one must issue EnsureDeleted() each time at the end to pull down the database.  
                // Therefore use EnsureCreated()/EnsureDeleted() for testing/development purposes only.
                // Note: On CF, it appears that EnsureDeleted may not work. For MySql dbs, it tries to 
                // access the 'mysql' database and fails because our random bound user (via cf bind-service) cannot 
                // this internal database.

                // For clarity and compatibility, we'll stick to Database.Migrate()
                // We do migrate here because potentially, one would need to initialise
                // a database on CF that may only be internal to CF or do further migrations in future. 
                // This will require a snapshot to be created first via dotnet ef migrations add <somename>.  
                // If access to CF database is not possible, point to a local database first.

                dbContext.Database.Migrate();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
