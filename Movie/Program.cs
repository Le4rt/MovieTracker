using MovieTracker; // Namespace of your classes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    class Program
    {
        static void Main()
        {
            Database db = new Database("Host=localhost;Port=5432;Username=postgres;Password=1234;Database=MovieTracker");

            User userManager = new User(db);
            Movie movieManager = new Movie(db);
            Watchlist watchlistManager = new Watchlist(db);
            Rating ratingManager = new Rating(db);
            Subscription subscriptionManager = new Subscription(db);
            Payment paymentManager = new Payment(db);
            Admin adminManager = new Admin(db);

            Console.WriteLine("==== Welcome to MovieTracker ====");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": Register(userManager); break;
                    case "2": Login(userManager, movieManager, watchlistManager, ratingManager, subscriptionManager, paymentManager, adminManager); break;
                    case "0": exit = true; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        static void Register(User userManager)
        {
            Console.WriteLine("\n-- Register --");
            Console.Write("First Name: "); string first = Console.ReadLine();
            Console.Write("Last Name: "); string last = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("Password: "); string password = Console.ReadLine();
            Console.Write("Date of Birth (yyyy-mm-dd): "); DateTime dob = DateTime.Parse(Console.ReadLine());

            bool success = userManager.Register(first, last, email, password, dob, username);
            Console.WriteLine(success ? "Registration successful!" : "Registration failed.");
        }

        static void Login(User userManager, Movie movieManager, Watchlist watchlistManager, Rating ratingManager, Subscription subscriptionManager, Payment paymentManager, Admin adminManager)
        {
            Console.WriteLine("\n-- Login --");
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("Password: "); string password = Console.ReadLine();

            bool success = userManager.Login(username, password);
            if (!success)
            {
                Console.WriteLine("Login failed.");
                return;
            }

            Console.WriteLine($"Welcome, {userManager.Username}!");
            int userID = userManager.UserID;

            bool logout = false;
            while (!logout)
            {
                Console.WriteLine("\n-- Menu --");
                Console.WriteLine("1. View Movies");
                Console.WriteLine("2. Watchlist");
                Console.WriteLine("3. Ratings");
                Console.WriteLine("4. Subscription");
                Console.WriteLine("5. Payments");
                Console.WriteLine("0. Logout");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ShowMovies(movieManager); break;
                    case "2": ManageWatchlist(watchlistManager, movieManager, userID); break;
                    case "3": ManageRatings(ratingManager, movieManager, userID); break;
                    case "4": ManageSubscription(subscriptionManager, userID); break;
                    case "5": ManagePayments(paymentManager, userID); break;
                    case "0": logout = true; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        // ===============================
        // Movies
        // ===============================
        static void ShowMovies(Movie movieManager)
        {
            var movies = movieManager.GetAllMovies();
            foreach (var m in movies)
            {
                Console.WriteLine($"{m.MovieID} - {m.Title} ({m.Genre}, {m.ReleaseYear}) - {m.DurationMinutes} min - {m.Language}");
            }
        }

        // ===============================
        // Watchlist
        // ===============================
        static void AddMovieToWatchlist(Watchlist watchlistManager, Movie movieManager, int userID)
        {
            var movies = movieManager.GetAllMovies();
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies available in the database.");
                return;
            }

            Console.WriteLine("\n-- Available Movies --");
            foreach (var m in movies)
            {
                Console.WriteLine($"{m.MovieID} - {m.Title}");
            }

            Console.Write("Enter MovieID to add: ");
            if (int.TryParse(Console.ReadLine(), out int movieID))
            {
                bool success = watchlistManager.AddToWatchlist(userID, movieID);
                Console.WriteLine(success ? "Movie added to watchlist!" : "Failed to add movie.");
            }
            else
            {
                Console.WriteLine("Invalid MovieID.");
            }
        }

        static void RemoveMovieFromWatchlist(Watchlist watchlistManager, List<Watchlist.WatchlistItem> list, int userID)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Your watchlist is empty. Nothing to remove.");
                return;
            }

            Console.Write("Enter WatchlistID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int watchlistID))
            {
                bool success = watchlistManager.RemoveFromWatchlist(userID, watchlistID);
                Console.WriteLine(success ? "Movie removed from watchlist." : "Failed to remove movie.");
            }
            else
            {
                Console.WriteLine("Invalid WatchlistID.");
            }
        }

        static void ManageWatchlist(Watchlist watchlistManager, Movie movieManager, int userID)
        {
            bool back = false;
            while (!back)
            {
                var list = watchlistManager.GetUserWatchlist(userID);

                Console.WriteLine("\n-- Your Watchlist --");
                if (list.Count == 0)
                    Console.WriteLine("Your watchlist is empty.");
                else
                    foreach (var w in list)
                        Console.WriteLine($"{w.WatchlistID} - {w.MovieTitle} (Added: {w.AddedDate:d})");

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add a movie");
                Console.WriteLine("2. Remove a movie");
                Console.WriteLine("0. Back to main menu");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddMovieToWatchlist(watchlistManager, movieManager, userID); break;
                    case "2": RemoveMovieFromWatchlist(watchlistManager, list, userID); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }

        // ===============================
        // Ratings
        // ===============================
        static void ManageRatings(Rating ratingManager, Movie movieManager, int userID)
        {
            bool back = false;
            while (!back)
            {
                var ratings = ratingManager.GetUserRatings(userID);

                Console.WriteLine("\n-- Your Ratings --");
                if (ratings.Count == 0)
                    Console.WriteLine("You have not rated any movies yet.");
                else
                    foreach (var r in ratings)
                        Console.WriteLine($"{r.RatingID} - {r.MovieTitle} - {r.Score}/5 - {r.Comment} ({r.RatingDate:d})");

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add a rating");
                Console.WriteLine("2. Remove a rating");
                Console.WriteLine("0. Back to main menu");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddRating(ratingManager, movieManager, userID);
                        break;
                    case "2":
                        RemoveRating(ratingManager, ratings, userID);
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void AddRating(Rating ratingManager, Movie movieManager, int userID)
        {
            var movies = movieManager.GetAllMovies();
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies available to rate.");
                return;
            }

            Console.WriteLine("\n-- Movies --");
            foreach (var m in movies)
                Console.WriteLine($"{m.MovieID} - {m.Title}");

            Console.Write("Enter MovieID to rate: ");
            if (!int.TryParse(Console.ReadLine(), out int movieID))
            {
                Console.WriteLine("Invalid MovieID.");
                return;
            }

            Console.Write("Enter score (1-5): ");
            if (!int.TryParse(Console.ReadLine(), out int score) || score < 1 || score > 5)
            {
                Console.WriteLine("Invalid score.");
                return;
            }

            Console.Write("Enter comment: ");
            string comment = Console.ReadLine();

            bool success = ratingManager.AddRating(userID, movieID, score, comment);
            Console.WriteLine(success ? "Rating added!" : "Failed to add rating.");
        }

        static void RemoveRating(Rating ratingManager, List<Rating.RatingItem> ratings, int userID)
        {
            if (ratings.Count == 0)
            {
                Console.WriteLine("No ratings to remove.");
                return;
            }

            Console.Write("Enter RatingID to remove: ");
            if (int.TryParse(Console.ReadLine(), out int ratingID))
            {
                bool success = ratingManager.RemoveRating(userID, ratingID);
                Console.WriteLine(success ? "Rating removed." : "Failed to remove rating.");
            }
            else
            {
                Console.WriteLine("Invalid RatingID.");
            }
        }

        // ===============================
        // Subscription
        // ===============================
        static void ManageSubscription(Subscription subscriptionManager, int userID)
        {
            bool back = false;
            while (!back)
            {
                var sub = subscriptionManager.GetUserSubscription(userID);
                Console.WriteLine("\n-- Your Subscription --");
                if (sub == null)
                    Console.WriteLine("You have no subscription.");
                else
                    Console.WriteLine($"{sub.SubscriptionType} - Active: {sub.IsActive} - Start: {sub.StartDate:d} - End: {(sub.EndDate.HasValue ? sub.EndDate.Value.ToString("d") : "N/A")}");

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add/Change Subscription");
                Console.WriteLine("0. Back to main menu");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter subscription type (e.g., Premium, Basic): ");
                        string type = Console.ReadLine();
                        subscriptionManager.AddOrUpdateSubscription(userID, type);
                        Console.WriteLine("Subscription updated!");
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        // ===============================
        // Payments
        // ===============================
        static void ManagePayments(Payment paymentManager, int userID)
        {
            bool back = false;
            while (!back)
            {
                var payments = paymentManager.GetUserPayments(userID);
                Console.WriteLine("\n-- Your Payments --");
                if (payments.Count == 0)
                    Console.WriteLine("You have no payments yet.");
                else
                    foreach (var p in payments)
                        Console.WriteLine($"{p.PaymentID} - {p.Amount:C} - {p.PaymentMethod} - {p.Status} ({p.PaymentDate:d})");

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add Payment");
                Console.WriteLine("0. Back to main menu");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter amount: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                        {
                            Console.WriteLine("Invalid amount.");
                            break;
                        }
                        Console.Write("Enter payment method (e.g., Credit Card): ");
                        string method = Console.ReadLine();
                        paymentManager.AddPayment(userID, amount, method, "Pending");
                        Console.WriteLine("Payment added!");
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
