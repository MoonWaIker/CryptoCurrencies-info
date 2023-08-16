using System.Data;
using Cryptocurrencies_info.Models.DataBase;
using Cryptocurrencies_info.Services.Interfaces.Connection;
using Npgsql;

namespace Cryptocurrencies_info.Services.DataBase
{
    public class PostgreSql : IConnectionGetter, IConnectionFiller
    {
        // Hardcodes
        private const string tableName = "CoinMarket";
        private readonly string connectionString;
        private readonly ILogger<PostgreSql> logger;

        public PostgreSql(IServiceProvider serviceProvider)
        {
            // Initialize logger
            logger = serviceProvider.GetRequiredService<ILogger<PostgreSql>>();

            // Set configurations
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            string host = configuration.GetValue<string>("host") ?? throw new ArgumentNullException(nameof(serviceProvider), "Host must be not null");
            int? port = configuration.GetValue<int>("port");
            string? username = configuration.GetValue<string>("username");
            string? database = configuration.GetValue<string>("database");
            bool? trustedConnection = configuration.GetValue<bool>("trustedConnection");
            bool? errorDetail = configuration.GetValue<bool>("errorDetail");
            connectionString = $"Host={host};{(port is not null ? $"Port={port};" : string.Empty)}" +
            $"{(username is not null ? $"Username={username};" : string.Empty)}" +
            $"{(database is not null ? $"Database={database};" : string.Empty)}" +
            $"{(trustedConnection is not null ? $"Trust Server Certificate={trustedConnection};" : string.Empty)}" +
            $"{(errorDetail is not null ? $"Include Error Detail={errorDetail};" : string.Empty)}";
        }

        // Add markets to sql
        public async Task AddMarkets(CoinGeckoMarket[] markets)
        {
            // Opening connection
            using NpgsqlConnection connection = new(connectionString);
            await connection.OpenAsync();

            // Initialize values
            string values = string.Join(',', markets
            .Select((market, index) => $"(@name{index}, @base{index}, @target{index}, @trust{index}, @link{index}, @logo{index})"));

            // Initialize query
            using NpgsqlCommand cmd = new(@$"INSERT INTO {$"\"{tableName}\""} (Name, Base, Target, Trust, Link, Logo)
        SELECT DISTINCT ON (Name, Base, Target) Name, Base, Target, Trust, Link, Logo
        FROM (VALUES {values}) AS Market(Name, Base, Target, Trust, Link, Logo)
        ON CONFLICT (name, base, target)
        DO
        UPDATE SET trust = EXCLUDED.trust, link = EXCLUDED.link, logo = EXCLUDED.logo;", connection);
            cmd.Transaction = connection.BeginTransaction();

            // Adding values of markets
            for (int i = 0; i < markets.Length; i++)
            {
                _ = cmd.Parameters.AddWithValue($"@name{i}", markets[i].Name);
                _ = cmd.Parameters.AddWithValue($"@base{i}", markets[i].Base);
                _ = cmd.Parameters.AddWithValue($"@target{i}", markets[i].Target);
                _ = cmd.Parameters.AddWithValue($"@trust{i}", markets[i].Trust);
                _ = cmd.Parameters.AddWithValue($"@link{i}", markets[i].Link ?? string.Empty);
                _ = cmd.Parameters.AddWithValue($"@logo{i}", markets[i].Logo ?? string.Empty);
            }
            try
            {
                // Execute
                _ = cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            // Rollback
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                try
                {
                    cmd.Transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    logger.LogError(exRollback.Message, exRollback);
                }
            }
        }

        // Delete all data in sql
        public void RefreshTable()
        {
            // Initializing connection
            using NpgsqlConnection connection = new(connectionString);
            connection.Open();

            // Initializing command
            using NpgsqlCommand cmd = new($"TRUNCATE TABLE \"{tableName}\";", connection);
            cmd.Transaction = connection.BeginTransaction();

            // Execute
            try
            {
                _ = cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                // Rollback
                try
                {
                    cmd.Transaction.Rollback();
                }
                catch (Exception exRollback)
                {
                    logger.LogError(exRollback.Message, exRollback);
                }
            }
        }

        // Read and return data from sql
        // TODO May you can do it async and elaborate each line as task
        public CoinGeckoMarket[] GetMarkets(IEnumerable<MarketBase> markets)
        {
            try
            {
                // Initialize bases
                // Initialize names for where
                IEnumerable<IGrouping<string, MarketBase>> nameBase = markets
                    .GroupBy(market => market.Name);

                string namesWhere = string.Join(" OR ", nameBase
                    .Select((market, index) => $"Name = @name{index}"));

                // Initialize Base & Target for where
                IEnumerable<IGrouping<string, MarketBase>> identifiresBase = markets
                    .GroupBy(market => $"{market.Base} {market.Target}");

                string identifiresWhere = string.Join(" OR ", identifiresBase
                    .Select((market, id) => $"Base = @base{id} AND Target = @target{id}"));

                // Opening connection
                using NpgsqlConnection connection = new(connectionString);
                connection.Open();

                // Initialize query
                using NpgsqlCommand cmd = new($"SELECT * FROM \"{tableName}\" WHERE ({namesWhere}) AND ({identifiresWhere});", connection);

                // Initialize Names
                string[] names = nameBase
                    .Select(market => market.Key)
                    .ToArray();
                for (int i = 0; i < names.Length; i++)
                {
                    _ = cmd.Parameters.AddWithValue($"name{i}", names[i]);
                }

                // Initialize Base & Target
                string[][] identifires = identifiresBase
                    .Select(market => market.Key.Split(' '))
                    .ToArray();
                for (int i = 0; i < identifires.Length; i++)
                {
                    _ = cmd.Parameters.AddWithValue($"base{i}", identifires[i][0]);
                    _ = cmd.Parameters.AddWithValue($"target{i}", identifires[i][1]);
                }

                // Execute
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                return IConnectionGetter.ParseMarkets(reader);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}