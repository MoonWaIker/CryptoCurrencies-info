﻿using Cryptocurrencies_info.Models.DataBase;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrencies_info.Services.DataBase
{
    public class MicrosoftSql : EntityFramework
    {

        // Hardcodes
        private readonly string connectionString;

        public MicrosoftSql(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            // Set configurations
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var database = configuration.GetSection("DataBase");

            // Connection string
            string? databaseName = database.GetValue<string>("database");
            bool trustedConnection = database.GetValue<bool>("trustedConnection");
            connectionString = @$"Server=(localdb)\mssqllocaldb;Database={databaseName};Trusted_Connection={trustedConnection};";

            // Markets setting
            Markets = Set<CoinGeckoMarket>();
        }

        // Configuration of connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _ = optionsBuilder.EnableSensitiveDataLogging();
            _ = optionsBuilder.UseSqlServer(connectionString);
        }
    }
}