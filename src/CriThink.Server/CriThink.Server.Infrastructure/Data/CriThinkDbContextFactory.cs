using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CriThink.Server.Infrastructure.Data
{
    public class CriThinkDbContextFactory : IDesignTimeDbContextFactory<CriThinkDbContext>
    {
        public CriThinkDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CriThinkDbContext>();

            string connectionString;
            if (args?.Any() == true)
            {
                connectionString = args[0];
            }
            else
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddUserSecrets(Assembly.Load("CriThink.Server.Web"))
                    .AddEnvironmentVariables()
                    .Build();

                connectionString = config.GetConnectionString("CriThinkDbPgSqlConnection");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("Connection string is missing. Please use 'database update -- <connection-string>'");
            }

            optionsBuilder
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention(System.Globalization.CultureInfo.InvariantCulture);

            return new CriThinkDbContext(optionsBuilder.Options);
        }
    }
}
