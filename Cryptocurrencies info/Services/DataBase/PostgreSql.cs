using Cryptocurrencies_info.Models.Cryptocurrencies;
using Cryptocurrencies_info.Models.DataBase;
using Npgsql;

namespace Cryptocurrencies_info.Services.DataBase
{
    public class PostgreSql : IConnection
    {
        // Hardcodes
        private const string tableName = "CoinMarket";
        private readonly string connectionString;

        public PostgreSql(IConfiguration configuration)
        {
            string host = configuration.GetValue<string>("host") ?? throw new ArgumentNullException(nameof(configuration), "Host must be not null");
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
        public void AddMarkets(Market[] markets)
        {
            try
            {
                // Initialize values
                string values = string.Join(',', markets
                .Select((market, index) => $"(@name{index}, @base{index}, @target{index}, @trust{index}, @link{index}, @logo{index})"));

                // Opening connection
                using NpgsqlConnection connection = new(connectionString);
                connection.Open();

                // Initialize query
                using NpgsqlCommand cmd = new(@$"INSERT INTO {$"\"{tableName}\""} (Name, Base, Target, Trust, Link, Logo)
            OVERRIDING SYSTEM VALUE
            VALUES {values};", connection);

                // Adding mvalues of markets
                for (int i = 0; i < markets.Length; i++)
                {
                    _ = cmd.Parameters.AddWithValue($"@name{i}", markets[i].Name);
                    _ = cmd.Parameters.AddWithValue($"@base{i}", markets[i].Base);
                    _ = cmd.Parameters.AddWithValue($"@target{i}", markets[i].Target);
                    _ = cmd.Parameters.AddWithValue($"@trust{i}", markets[i].Trust);
                    _ = cmd.Parameters.AddWithValue($"@link{i}", markets[i].Link ?? string.Empty);
                    _ = cmd.Parameters.AddWithValue($"@logo{i}", markets[i].Logo ?? string.Empty);
                }

                // Execute
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Delete all data in sql
        public void RefreshTable()
        {
            try
            {
                using NpgsqlConnection connection = new(connectionString);
                connection.Open();
                using NpgsqlCommand cmd = new($"TRUNCATE TABLE \"{tableName}\";", connection);
                _ = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Read and return data from sql
        public Market[] GetMarkets(MarketBase[] markets)
        {
            try
            {
                // Doing query
                // Initialize Names for where
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

                return IConnection.ParseMarkets(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}