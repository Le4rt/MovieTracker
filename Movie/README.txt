🎬 MovieTracker – C# Console Application
📌 Project Overview

MovieTracker is a C# console-based application that allows users to browse movies, manage watchlists, rate movies, subscribe to plans, and handle payments.
The application uses PostgreSQL as its database and connects via Npgsql.

This project demonstrates:

Object-Oriented Programming (OOP)

Database connectivity

CRUD operations

Console-based user interaction

Separation of concerns (Managers / Services)

🛠️ Technologies Used

C# (.NET)

PostgreSQL

Npgsql (PostgreSQL .NET data provider)

pgAdmin (for database management)

📁 Project Structure
MovieTracker/
│
├── Program.cs          // Application entry point & menus
├── Database.cs         // Database connection handler
│
├── User.cs             // User registration & login
├── Admin.cs            // Admin movie & subscription management
├── Movie.cs            // Movie browsing & searching
├── Watchlist.cs        // Watchlist management
├── Rating.cs           // Movie ratings
├── Subscription.cs     // User subscriptions
├── Payment.cs          // Payment handling

👤 User Features

Register & login

View all movies

Search movies by title

Add/remove movies from watchlist

Rate movies (1–5 stars)

Manage subscriptions (Free / Premium)

View and add payments

👑 Admin Features

Add new movies

Delete movies

View all users

Update user subscriptions

🔗 Database Connection

The database connection is handled in Database.cs.

Default Connection Settings:
Host=localhost
Port=5432
Database=MovieTracker
Username=postgres
Password=****


⚠️ Important:
Change these values if you run the project on a different machine or database.

▶️ How to Run the Project

Make sure PostgreSQL is running

Create the database and tables (see SQL project repository)

Open the project in Visual Studio Code

Restore NuGet packages (Npgsql)

Run the project on Terminal:

dotenet run




Use the console menu to register or log in

🧠 Notes

Passwords are stored as plain text (for educational purposes only)

SQL injection is prevented using parameterized queries

Enums are validated manually (e.g., genres, payment methods)

Designed for educational / student use

🚀 Possible Improvements

Password hashing

Role-based authentication

GUI (WPF / WinForms)

API-based architecture

Better error handling

Pagination for movie lists

📚 Educational Purpose

This project was created as part of a database & programming course to demonstrate how C# applications interact with PostgreSQL databases using real-world logic.