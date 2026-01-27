-- 1️⃣ People born in the 1990s
SELECT * FROM Person
WHERE EXTRACT(YEAR FROM DateOfBirth) BETWEEN 1990 AND 1999;

-- 2️⃣ People whose first name starts with 'A' and last name starts with 'H'
SELECT * FROM Person
WHERE FirstName LIKE 'A%' AND LastName LIKE 'H%';

-- 3️⃣ People with PersonID greater than 15
SELECT * FROM Person
WHERE PersonID > 15;

-- 4️⃣ People whose email ends with 'mail.com'
SELECT * FROM Person
WHERE Email LIKE '%mail.com';

-- 5️⃣ People whose first name contains 'er'
SELECT * FROM Person
WHERE FirstName LIKE '%Er%';

-- 6️⃣ People born before 1995
SELECT * FROM Person
WHERE DateOfBirth < '1995-01-01';

-- 7️⃣ People born after 2000
SELECT * FROM Person
WHERE DateOfBirth > '2000-01-01';

-- 8️⃣ People whose last name has exactly 5 letters
SELECT * FROM Person
WHERE LENGTH(LastName) = 5;

-- 9️⃣ People whose first name is either 'Arben' or 'Ilir'
SELECT * FROM Person
WHERE FirstName IN ('Arben', 'Ilir');

-- 🔟 People whose first name starts with 'L' or 'R' (use OR)
SELECT * FROM Person
WHERE FirstName LIKE 'L%' OR FirstName LIKE 'R%';

-- 6️⃣ Users with active accounts
SELECT * FROM "User"
WHERE IsActive = TRUE;

-- 7️⃣ Users whose username starts with 'a'
SELECT * FROM "User"
WHERE Username LIKE 'a%';

-- 8️⃣ Users with Premium subscription (join with Subscription)
SELECT u.Username, s.SubscriptionType
FROM "User" u
JOIN Subscription s ON u.UserID = s.UserID
WHERE s.SubscriptionType = 'Premium';

-- 9️⃣ Users who joined after 2024-02-01
SELECT * FROM "User"
WHERE JoinDate > '2024-02-01';

-- 🔟 Users who have never been active (IsActive = FALSE)
SELECT * FROM "User"
WHERE IsActive = FALSE;

-- Query 1: Show all movies in each user's watchlist
-- Shows all users, and movies in their watchlist if they exist
SELECT 
    u.Username,
    m.Title AS MovieTitle,
    w.AddedDate
FROM "User" u
LEFT JOIN Watchlist w ON u.UserID = w.UserID
LEFT JOIN Movie m ON w.MovieID = m.MovieID
ORDER BY u.Username, w.AddedDate;

-- Query 2: Show all ratings given by users
-- Shows all users, and their ratings if they exist
SELECT
    u.Username,
    m.Title AS MovieTitle,
    r.Score,
    r.Comment,
    r.RatingDate
FROM "User" u
LEFT JOIN Rating r ON u.UserID = r.UserID
LEFT JOIN Movie m ON r.MovieID = m.MovieID
ORDER BY u.Username, r.RatingDate;


-- Query 3: Show subscription info for all users
-- Joins Subscription with User to display username, subscription type,
-- start and end dates, and whether the subscription is active
SELECT 
    u.Username, 
    s.SubscriptionType, 
    s.StartDate, 
    s.EndDate, 
    s.IsActive
FROM Subscription s
JOIN "User" u ON s.UserID = u.UserID;

-- Query 4: Show payment details for users
-- Joins Payment with User to display username, amount paid,
-- payment method, status, and payment date
SELECT 
    u.Username, 
    p.Amount, 
    p.PaymentMethod, 
    p.Status, 
    p.PaymentDate
FROM Payment p
JOIN "User" u ON p.UserID = u.UserID;


