using Npgsql;
using System;
using System.Collections.Generic;
using MovieTracker;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Rating
    {
        private readonly Database _db;

        public Rating(Database db)
        {
            _db = db;
        }

        // Rating data transfer object
        public class RatingItem
        {
            public int RatingID { get; set; }
            public string MovieTitle { get; set; } = ""; // avoid nullable warnings
            public int Score { get; set; }
            public string Comment { get; set; } = "";    // avoid nullable warnings
            public DateTime RatingDate { get; set; }
        }

        // ===============================
        // Get all ratings by a user
        // ===============================
        public List<RatingItem> GetUserRatings(int userID)
        {
            var ratings = new List<RatingItem>();

            try
            {
                _db.Open();
                string query = @"
                    SELECT r.RatingID, m.Title, r.Score, r.Comment, r.RatingDate
                    FROM Rating r
                    JOIN Movie m ON r.MovieID = m.MovieID
                    WHERE r.UserID = @userID
                    ORDER BY r.RatingDate DESC;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ratings.Add(new RatingItem
                    {
                        RatingID = reader.GetInt32(0),
                        MovieTitle = reader.IsDBNull(1) ? "" : reader.GetString(1),
                        Score = reader.GetInt32(2),
                        Comment = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        RatingDate = reader.GetDateTime(4)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching ratings: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return ratings;
        }

        // ===============================
        // Add a new rating
        // ===============================
        public bool AddRating(int userID, int movieID, int score, string comment)
        {
            if (score < 1 || score > 5)
            {
                Console.WriteLine("Score must be between 1 and 5.");
                return false;
            }

            try
            {
                _db.Open();
                string query = @"
                    INSERT INTO Rating (UserID, MovieID, Score, Comment)
                    VALUES (@userID, @movieID, @score, @comment);";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("movieID", movieID);
                cmd.Parameters.AddWithValue("score", score);
                cmd.Parameters.AddWithValue("comment", comment ?? "");

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding rating: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        // ===============================
        // Update an existing rating
        // ===============================
        public bool UpdateRating(int ratingID, int score, string comment)
        {
            if (score < 1 || score > 5)
            {
                Console.WriteLine("Score must be between 1 and 5.");
                return false;
            }

            try
            {
                _db.Open();
                string query = @"
                    UPDATE Rating 
                    SET Score = @score, Comment = @comment, RatingDate = CURRENT_DATE
                    WHERE RatingID = @ratingID;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("ratingID", ratingID);
                cmd.Parameters.AddWithValue("score", score);
                cmd.Parameters.AddWithValue("comment", comment ?? "");

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating rating: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        // ===============================
        // Remove a rating
        // ===============================
        public bool RemoveRating(int userID, int ratingID)
        {
            try
            {
                _db.Open();
                string query = "DELETE FROM Rating WHERE RatingID=@ratingID AND UserID=@userID";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("ratingID", ratingID);
                cmd.Parameters.AddWithValue("userID", userID);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error removing rating: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }
    }
}
