using Npgsql;
using System;
using System.Collections.Generic;
using MovieTracker;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class Payment
    {
        private readonly Database _db;

        private static readonly string[] AllowedMethods = { "CreditCard", "PayPal", "BankTransfer" };
        private static readonly string[] AllowedStatuses = { "Completed", "Pending", "Failed" };

        public Payment(Database db)
        {
            _db = db;
        }

        // Payment data transfer object
        public class PaymentItem
        {
            public int PaymentID { get; set; }
            public decimal Amount { get; set; }
            public string PaymentMethod { get; set; }
            public string Status { get; set; }
            public DateTime PaymentDate { get; set; }
        }

        // ===============================
        // Get all payments for a user
        // ===============================
        public List<PaymentItem> GetUserPayments(int userID)
        {
            var payments = new List<PaymentItem>();

            try
            {
                _db.Open();
                string query = @"
                    SELECT PaymentID, Amount, PaymentMethod, Status, PaymentDate
                    FROM Payment
                    WHERE UserID = @userID
                    ORDER BY PaymentDate DESC;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    payments.Add(new PaymentItem
                    {
                        PaymentID = reader.GetInt32(0),
                        Amount = reader.GetDecimal(1),
                        PaymentMethod = reader.GetString(2),
                        Status = reader.GetString(3),
                        PaymentDate = reader.GetDateTime(4)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching payments: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return payments;
        }

        // ===============================
        // Add a new payment
        // ===============================
        public bool AddPayment(int userID, decimal amount, string paymentMethod, string status)
        {
            if (Array.IndexOf(AllowedMethods, paymentMethod) == -1)
            {
                Console.WriteLine("Invalid payment method.");
                return false;
            }

            if (Array.IndexOf(AllowedStatuses, status) == -1)
            {
                Console.WriteLine("Invalid payment status.");
                return false;
            }

            try
            {
                _db.Open();
                string query = @"
                    INSERT INTO Payment (UserID, Amount, PaymentMethod, Status)
                    VALUES (@userID, @amount, @paymentMethod, @status);";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);
                cmd.Parameters.AddWithValue("amount", amount);
                cmd.Parameters.AddWithValue("paymentMethod", paymentMethod);
                cmd.Parameters.AddWithValue("status", status);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding payment: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }

        // ===============================
        // Get latest payment for a user
        // ===============================
        public PaymentItem GetLatestPayment(int userID)
        {
            PaymentItem payment = null;

            try
            {
                _db.Open();
                string query = @"
                    SELECT PaymentID, Amount, PaymentMethod, Status, PaymentDate
                    FROM Payment
                    WHERE UserID = @userID
                    ORDER BY PaymentDate DESC
                    LIMIT 1;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("userID", userID);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    payment = new PaymentItem
                    {
                        PaymentID = reader.GetInt32(0),
                        Amount = reader.GetDecimal(1),
                        PaymentMethod = reader.GetString(2),
                        Status = reader.GetString(3),
                        PaymentDate = reader.GetDateTime(4)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching latest payment: " + ex.Message);
            }
            finally
            {
                _db.Close();
            }

            return payment;
        }
    }
}
