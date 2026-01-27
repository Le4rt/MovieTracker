using Npgsql;
using MovieTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Admin
    {
        private readonly Database _db;

        private static readonly string[] AllowedGenres =
        {
            "Comedy", "Action", "Romance", "Drama",
            "History", "Fantasy", "Sci-Fi", "Horror", "Turk"
        };

        public Admin(Database db)
        {
            _db = db;
        }

        // ===============================
        // Movie Management
        // ===============================
        public bool AddMovie(string title, string genre, int releaseYear, int durationMinutes,
                             string language, string description = "")
        {
            if (Array.IndexOf(AllowedGenres, genre) == -1)
            {
                Console.WriteLine("Invalid genre.");
                return false;
            }

            try
            {
                _db.Open();

                string query = @"
                    INSERT INTO Movie (Title, Genre, ReleaseYear, DurationMinutes, Language, Description)
                    VALUES (@title, @genre, @releaseYear, @duration, @language, @description);";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("title", title);
                cmd.Parameters.AddWithValue("genre", genre);
                cmd.Parameters.AddWithValue("releaseYear", releaseYear);
                cmd.Parameters.AddWithValue("duration", durationMinutes);
                cmd.Parameters.AddWithValue("language", language);
                cmd.Parameters.AddWithValue("description", description);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding movie: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        public bool DeleteMovie(int movieID)
        {
            try
            {
                _db.Open();

                string query = "DELETE FROM Movie WHERE MovieID = @movieID;";
                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("movieID", movieID);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting movie: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        // ===============================
        // User Management
        // ===============================
        public List<(int UserID, string Username)> GetAllUsers()
        {
            var users = new List<(int, string)>();

            try
            {
                _db.Open();

                string query = @"SELECT UserID, Username FROM ""User"" ORDER BY UserID;";
                using var cmd = new NpgsqlCommand(query, _db.Connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add((reader.GetInt32(0), reader.GetString(1)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching users: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return users;
        }

        // ===============================
        // Subscription Management
        // ===============================
        public bool UpdateSubscription(int userID, string subscriptionType,
                                       DateTime startDate, DateTime? endDate, bool isActive)
        {
            if (subscriptionType != "Free" && subscriptionType != "Premium")
            {
                Console.WriteLine("Invalid subscription type.");
                return false;
            }

            try
            {
                _db.Open();

                string query = @"
                    UPDATE Subscription
                    SET SubscriptionType = @type,
                        StartDate = @start,
                        EndDate = @end,
                        IsActive = @active
                    WHERE UserID = @userID;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("type", subscriptionType);
                cmd.Parameters.AddWithValue("start", startDate);
                cmd.Parameters.AddWithValue("end", (object?)endDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("active", isActive);
                cmd.Parameters.AddWithValue("userID", userID);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating subscription: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }
    }
}