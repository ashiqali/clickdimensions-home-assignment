using SocialSyncPortal.DAL.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace SocialSyncPortal.Test.Helpers
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove already registered database context dependency
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<SocialSyncPortalDbContext>));

                services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbConnection));

                services.Remove(dbConnectionDescriptor);

                // Build configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                // Get connection string from configuration
                var connectionString = configuration.GetConnectionString("DefaultConnection");


                // Create open SqlConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqlConnection(connectionString);
                    connection.Open();

                    return connection;
                });

                // Create an in-memory SQLite database for testing
                services.AddDbContext<SocialSyncPortalDbContext>(options =>
                {
                    // Use SQLite in-memory database for testing
                    options.UseSqlite(new SqliteConnection("DataSource=:memory:"));
                });

                // Ensure the database is created and migrations are applied
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<SocialSyncPortalDbContext>();
                    db.Database.OpenConnection();                   
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
