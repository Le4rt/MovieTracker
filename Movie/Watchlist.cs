using Npgsql;
using System;
using System.Collections.Generic;
using MovieTracker;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Watchlist
    {
        private readonly Database _db;

        public Watchlist(Database db)
        {
            _db = db;
        }

        // Watchlist data transfer object
        public class WatchlistItem
        {
            public int WatchlistID { get; set; }
            public string MovieTitle { get; set; }
            public DateTime AddedDate { get; set; }
        }

        // ===============================
        // Get all movies in user's watchlist
        // ===============================
        public List<WatchlistItem> GetUserWatchlist(int userID)
        {
            var list = new List<WatchlistItem>();

            try
            {
                _db.Open();
                string query = @"
                    SELECT w.WatchlistID, m.Title, w.AddedDate
                    FROM Watchlist w
                    JOIN Movie m ON w.MovieID = m.MovieID
                    WHERE w.UserID = @userID
                    ORDER BY w.AddedDate DESC;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new WatchlistItem
                    {
                        WatchlistID = reader.GetInt32(0),
                        MovieTitle = reader.GetString(1),
                        AddedDate = reader.GetDateTime(2)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching watchlist: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return list;
        }

        // ===============================
        // Add a movie to user's watchlist
        // ===============================
        public bool AddToWatchlist(int userID, int movieID)
        {
            try
            {
                _db.Open();
                string query = @"
                    INSERT INTO Watchlist (UserID, MovieID) 
                    VALUES (@userID, @movieID)
                    ON CONFLICT (UserID, MovieID) DO NOTHING;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("movieID", movieID);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding to watchlist: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        // ===============================
        // Remove a movie from user's watchlist
        // ===============================
        public bool RemoveFromWatchlist(int userID, int movieID)
        {
            try
            {
                _db.Open();
                string query = @"
                    DELETE FROM Watchlist 
                    WHERE UserID = @userID AND MovieID = @movieID;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("movieID", movieID);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing from watchlist: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }
    }
}