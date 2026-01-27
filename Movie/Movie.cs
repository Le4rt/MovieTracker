using Npgsql;
using System;
using MovieTracker;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Movie
    {
        private readonly Database _db;

        public Movie(Database db)
        {
            _db = db;
        }

        // Movie data transfer object
        public class MovieInfo
        {
            public int MovieID { get; set; }
            public string Title { get; set; }
            public string Genre { get; set; }
            public int ReleaseYear { get; set; }
            public int DurationMinutes { get; set; }
            public string Language { get; set; }
            public string Description { get; set; } // optional, if needed
        }

        // ===============================
        // Get all movies
        // ===============================
        public List<MovieInfo> GetAllMovies()
        {
            var movies = new List<MovieInfo>();
            try
            {
                _db.Open();

                string query = @"
                    SELECT MovieID, Title, Genre, ReleaseYear, DurationMinutes, Language, Description
                    FROM Movie
                    ORDER BY Title;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    movies.Add(new MovieInfo
                    {
                        MovieID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Genre = reader.GetString(2),
                        ReleaseYear = reader.GetInt32(3),
                        DurationMinutes = reader.GetInt32(4),
                        Language = reader.GetString(5),
                        Description = reader.IsDBNull(6) ? "" : reader.GetString(6)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching movies: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return movies;
        }

        // ===============================
        // Search movies by title
        // ===============================
        public List<MovieInfo> SearchMovies(string keyword)
        {
            var movies = new List<MovieInfo>();
            try
            {
                _db.Open();

                string query = @"
                    SELECT MovieID, Title, Genre, ReleaseYear, DurationMinutes, Language, Description
                    FROM Movie
                    WHERE Title ILIKE @keyword
                    ORDER BY Title;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("keyword", "%" + keyword + "%");

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    movies.Add(new MovieInfo
                    {
                        MovieID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Genre = reader.GetString(2),
                        ReleaseYear = reader.GetInt32(3),
                        DurationMinutes = reader.GetInt32(4),
                        Language = reader.GetString(5),
                        Description = reader.IsDBNull(6) ? "" : reader.GetString(6)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error searching movies: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return movies;
        }

        // ===============================
        // Add movie (optional, for internal use)
        // ===============================
        public bool AddMovie(string title, string genre, int releaseYear,
                             int durationMinutes, string language, string description = "")
        {
            try
            {
                _db.Open();

                string query = @"
                    INSERT INTO Movie (Title, Genre, ReleaseYear, DurationMinutes, Language, Description)
                    VALUES (@title, @genre, @year, @duration, @language, @description);";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("title", title);
                cmd.Parameters.AddWithValue("genre", genre);
                cmd.Parameters.AddWithValue("year", releaseYear);
                cmd.Parameters.AddWithValue("duration", durationMinutes);
                cmd.Parameters.AddWithValue("language", language);
                cmd.Parameters.AddWithValue("description", description);

                cmd.ExecuteNonQuery();
                return true;
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
    }
}