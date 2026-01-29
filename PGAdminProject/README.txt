MovieTracker Database – PostgreSQL
📌 Project Overview

This repository contains the PostgreSQL database schema and sample data for the MovieTracker application.

The database is designed to support:

Users and admins

Movies and genres

Watchlists

Ratings & reviews

Subscriptions

Payments

It follows relational database design principles, uses foreign keys, constraints, and realistic test data.

🛠️ Technologies Used

PostgreSQL

pgAdmin 4

SQL (DDL & DML)

📁 Database Structure
🧑 Person

Stores personal information shared by both users and admins.

Key fields:

PersonID (SERIAL, PK)

Email (UNIQUE)

Password

DateOfBirth

👤 User

Represents application users.

Relationships:

One-to-one with Person

Key fields:

UserID (SERIAL, PK)

Username (UNIQUE)

IsActive

👑 Admin

Represents administrators.

Relationships:

One-to-one with Person

Key fields:

AdminID (SERIAL, PK)

AccessLevel

HireDate

🎥 Movie

Stores movie details.

Key fields:

MovieID (SERIAL, PK)

Title

Genre (CHECK constraint)

ReleaseYear

DurationMinutes

Language

📌 Watchlist

Many-to-many relationship between users and movies.

Constraints:

Unique (UserID, MovieID) pair

Cascade delete enabled

⭐ Rating

Stores user ratings and comments for movies.

Rules:

Score between 1 and 5

Linked to both User and Movie

💳 Subscription

Stores subscription details.

Types:

Free

Premium

Tracks:

Start & end dates

Active status

💰 Payment

Tracks subscription payments.

Supported methods:

CreditCard

PayPal

BankTransfer

Statuses:

Completed

Pending

Failed

🔗 Relationships Overview

Person → User (1:1)

Person → Admin (1:1)

User → Watchlist → Movie (M:N)

User → Rating → Movie

User → Subscription

User → Payment

All relationships enforce referential integrity with ON DELETE CASCADE.

▶️ How to Run the Database

Open pgAdmin

Create a new database:

CREATE DATABASE MovieTracker;


Open the Query Tool

Paste the SQL script from this repository

Execute the script (F5)

This will:

Drop existing tables (clean start)

Create all tables

Insert sample data

🧠 Notes

SERIAL is used for auto-incrementing primary keys

CHECK constraints enforce valid enum-like values

Passwords are stored as plain text for educational purposes

Sample data includes:

25 persons

20 users

2 admins

20 movies

Watchlists, ratings, subscriptions, and payments

🚀 Possible Improvements

Password hashing

Indexing for performance

Stored procedures

Triggers for subscription expiration

Views for reporting (Top rated movies, active users)

📚 Educational Purpose

This database was created as part of a student project to demonstrate:

Relational modeling

SQL constraints

Foreign keys

Realistic application data design