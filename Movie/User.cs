using Npgsql;
using System;
using MovieTracker;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class User
    {
        public int UserID { get; private set; }
        public int PersonID { get; private set; }
        public string Username { get; private set; }
        public bool IsActive { get; private set; }

        private readonly Database _db;

        public User(Database db)
        {
            _db = db;
        }

        // ===============================
        // Register a new user
        // ===============================
       public bool Register(string firstName, string lastName, string email, string password,
                     DateTime dateOfBirth, string username)
{
    try
    {
        _db.Open();

        // Insert into Person table
        string personQuery = @"
            INSERT INTO Person (FirstName, LastName, Email, Password, DateOfBirth)
            VALUES (@first, @last, @email, @pass, @dob)
            RETURNING PersonID;";

        using var personCmd = new NpgsqlCommand(personQuery, _db.Connection);
        personCmd.Parameters.AddWithValue("first", firstName);
        personCmd.Parameters.AddWithValue("last", lastName);
        personCmd.Parameters.AddWithValue("email", email);
        personCmd.Parameters.AddWithValue("pass", password); // consider hashing
        personCmd.Parameters.AddWithValue("dob", dateOfBirth);

        int personId = Convert.ToInt32(personCmd.ExecuteScalar());

        // Insert into User table
        string userQuery = @"
            INSERT INTO ""User"" (PersonID, Username)
            VALUES (@personId, @username)
            RETURNING UserID;";

        using var userCmd = new NpgsqlCommand(userQuery, _db.Connection);
        userCmd.Parameters.AddWithValue("personId", personId);
        userCmd.Parameters.AddWithValue("username", username);

        UserID = Convert.ToInt32(userCmd.ExecuteScalar());
        PersonID = personId;
        Username = username;
        IsActive = true;

        return true;
    }
    catch (PostgresException ex) when (ex.SqlState == "23505") // duplicate key
    {
        Console.WriteLine("This username is already taken. Please choose another one.");
        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error registering user: " + ex.Message);
        return false;
    }
    finally
    {
        _db.Close();
    }
}


        // ===============================
        // Login user
        // ===============================
        public bool Login(string username, string password)
        {
            try
            {
                _db.Open();

                string query = @"
                    SELECT u.UserID, u.PersonID, u.Username, u.IsActive
                    FROM ""User"" u
                    JOIN Person p ON u.PersonID = p.PersonID
                    WHERE u.Username = @username AND p.Password = @password;";

                using var cmd = new NpgsqlCommand(query, _db.Connection);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    UserID = reader.GetInt32(0);
                    PersonID = reader.GetInt32(1);
                    Username = reader.GetString(2);
                    IsActive = reader.GetBoolean(3);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error logging in: " + ex.Message);
                return false;
            }
            finally
            {
                _db.Close();
            }
        }
    }
}
