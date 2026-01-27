using Npgsql;
using System;
using MovieTracker;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Database
    {
        private readonly string _connectionString;
        public NpgsqlConnection Connection { get; private set; }

        // ===============================
        // Default connection settings
        // Change these if you move to a new PC
        // ===============================
        private const string DefaultHost = "localhost";
        private const int DefaultPort = 5432;
        private const string DefaultDb = "MovieTracker"; // <-- change to your DB
        private const string DefaultUser = "postgres";
        private const string DefaultPassword = "1234";

        // Default constructor uses default connection
        public Database()
        {
            _connectionString = $"Host={DefaultHost};Port={DefaultPort};Database={DefaultDb};Username={DefaultUser};Password={DefaultPassword}";
        }

        // Custom constructor if you want to override any setting
        public Database(string host, int port, string dbName, string username, string password)
        {
            _connectionString = $"Host={host};Port={port};Database={dbName};Username={username};Password={password}";
        }

        // Optional: constructor with full connection string
        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Open()
        {
            try
            {
                if (Connection == null)
                    Connection = new NpgsqlConnection(_connectionString);

                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to database: " + ex.Message);
                throw;
            }
        }

        public void Close()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
                Connection.Close();
        }
    }
}