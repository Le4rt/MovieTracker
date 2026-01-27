using Npgsql;
using System;
using System.Collections.Generic;
using MovieTracker;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Subscription
    {
        private readonly Database _db;

        public Subscription(Database db)
        {
            _db = db;
        }

        // Subscription data transfer object
        public class SubscriptionItem
        {
            public int SubscriptionID { get; set; }
            public string SubscriptionType { get; set; } = ""; // avoid nullable warnings
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool IsActive { get; set; }
        }

        // ===============================
        // Get subscription by user
        // ===============================
        public SubscriptionItem GetUserSubscription(int userID)
        {
            SubscriptionItem subscription = null;

            try
            {
                _db.Open();
                string query = @"
                    SELECT SubscriptionID, SubscriptionType, StartDate, EndDate, IsActive
                    FROM Subscription
                    WHERE UserID = @userID;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    subscription = new SubscriptionItem
                    {
                        SubscriptionID = reader.GetInt32(0),
                        SubscriptionType = reader.GetString(1),
                        StartDate = reader.GetDateTime(2),
                        EndDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        IsActive = reader.GetBoolean(4)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching subscription: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return subscription;
        }

        // ===============================
        // Add or Update a subscription
        // ===============================
        public bool AddOrUpdateSubscription(int userID, string newType, DateTime? endDate = null)
        {
            if (newType != "Free" && newType != "Premium")
            {
                Console.WriteLine("Invalid subscription type.");
                return false;
            }

            var existing = GetUserSubscription(userID);
            try
            {
                _db.Open();

                if (existing == null)
                {
                    // Insert new subscription
                    string insertQuery = @"
                        INSERT INTO Subscription (UserID, SubscriptionType, StartDate, EndDate, IsActive)
                        VALUES (@userID, @newType, @startDate, @endDate, TRUE);";

                    using var insertCmd = new NpgsqlCommand(insertQuery, _db.Connection);
                    insertCmd.Parameters.AddWithValue("userID", userID);
                    insertCmd.Parameters.AddWithValue("newType", newType);
                    insertCmd.Parameters.AddWithValue("startDate", DateTime.Today);
                    insertCmd.Parameters.AddWithValue("endDate", (object?)endDate ?? DBNull.Value);

                    return insertCmd.ExecuteNonQuery() > 0;
                }
                else
                {
                    // Update existing subscription
                    string updateQuery = @"
                        UPDATE Subscription
                        SET SubscriptionType = @newType, EndDate = @endDate, IsActive = TRUE
                        WHERE SubscriptionID = @subscriptionID;";

                    using var updateCmd = new NpgsqlCommand(updateQuery, _db.Connection);
                    updateCmd.Parameters.AddWithValue("subscriptionID", existing.SubscriptionID);
                    updateCmd.Parameters.AddWithValue("newType", newType);
                    updateCmd.Parameters.AddWithValue("endDate", (object?)endDate ?? DBNull.Value);

                    return updateCmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding/updating subscription: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        // ===============================
        // Check if subscription is active
        // ===============================
        public bool IsSubscriptionActive(int userID)
        {
            var subscription = GetUserSubscription(userID);
            if (subscription == null) return false;

            if (!subscription.IsActive) return false;

            if (subscription.EndDate.HasValue && subscription.EndDate.Value < DateTime.Today)
            {
                return false;
            }

            return true;
        }
    }
}
