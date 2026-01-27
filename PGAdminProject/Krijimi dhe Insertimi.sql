-- ===============================
-- 1. DROP TABLES IF EXIST (clean start)
-- ===============================
DROP TABLE IF EXISTS Payment;
DROP TABLE IF EXISTS Subscription;
DROP TABLE IF EXISTS Rating;
DROP TABLE IF EXISTS Watchlist;
DROP TABLE IF EXISTS "Admin";
DROP TABLE IF EXISTS "User";
DROP TABLE IF EXISTS Movie;
DROP TABLE IF EXISTS Person;

-- ===============================
-- 2. PERSON TABLE
-- ===============================
CREATE TABLE Person (
    PersonID SERIAL PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    DateOfBirth DATE,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ===============================
-- 3. USER TABLE
-- ===============================
CREATE TABLE "User" (
    UserID SERIAL PRIMARY KEY,
    PersonID INT UNIQUE NOT NULL,
    Username VARCHAR(100) UNIQUE NOT NULL,
    JoinDate DATE DEFAULT CURRENT_DATE,
    IsActive BOOLEAN DEFAULT TRUE,
    CONSTRAINT fk_user_person
        FOREIGN KEY (PersonID)
        REFERENCES Person(PersonID)
        ON DELETE CASCADE
);

-- ===============================
-- 4. ADMIN TABLE
-- ===============================
CREATE TABLE "Admin" (
    AdminID SERIAL PRIMARY KEY,
    PersonID INT UNIQUE NOT NULL,
    AccessLevel VARCHAR(30),
    HireDate DATE,
    CONSTRAINT fk_admin_person
        FOREIGN KEY (PersonID)
        REFERENCES Person(PersonID)
        ON DELETE CASCADE
);

-- ===============================
-- 5. MOVIE TABLE
-- ===============================
CREATE TABLE Movie (
    MovieID SERIAL PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Genre VARCHAR(100)
        CHECK (Genre IN ('Comedy', 'Action', 'Romance', 'Drama', 'History', 'Fantasy', 'Sci-Fi', 'Horror', 'Turk')),
    ReleaseYear INT NOT NULL,
    DurationMinutes INT NOT NULL,
    Language VARCHAR(50),
    Description TEXT,
    AddedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ===============================
-- 6. WATCHLIST TABLE
-- ===============================
CREATE TABLE Watchlist (
    WatchlistID SERIAL PRIMARY KEY,
    UserID INT NOT NULL,
    MovieID INT NOT NULL,
    AddedDate DATE DEFAULT CURRENT_DATE,
    CONSTRAINT fk_watchlist_user
        FOREIGN KEY (UserID)
        REFERENCES "User"(UserID)
        ON DELETE CASCADE,
    CONSTRAINT fk_watchlist_movie
        FOREIGN KEY (MovieID)
        REFERENCES Movie(MovieID)
        ON DELETE CASCADE,
    CONSTRAINT unique_watchlist UNIQUE (UserID, MovieID)
);

-- ===============================
-- 7. RATING TABLE
-- ===============================
CREATE TABLE Rating (
    RatingID SERIAL PRIMARY KEY,
    Score INT CHECK (Score BETWEEN 1 AND 5),
    Comment TEXT,
    RatingDate DATE DEFAULT CURRENT_DATE,
    UserID INT NOT NULL,
    MovieID INT NOT NULL,
    CONSTRAINT fk_rating_user
        FOREIGN KEY (UserID)
        REFERENCES "User"(UserID)
        ON DELETE CASCADE,
    CONSTRAINT fk_rating_movie
        FOREIGN KEY (MovieID)
        REFERENCES Movie(MovieID)
        ON DELETE CASCADE
);

-- ===============================
-- 8. SUBSCRIPTION TABLE
-- ===============================
CREATE TABLE Subscription (
    SubscriptionID SERIAL PRIMARY KEY,
    UserID INT NOT NULL,
    SubscriptionType VARCHAR(30)
        CHECK (SubscriptionType IN ('Free', 'Premium')),
    StartDate DATE NOT NULL,
    EndDate DATE,
    IsActive BOOLEAN DEFAULT TRUE,
    CONSTRAINT fk_subscription_user
        FOREIGN KEY (UserID)
        REFERENCES "User"(UserID)
        ON DELETE CASCADE
);

-- ===============================
-- 9. PAYMENT TABLE
-- ===============================
CREATE TABLE Payment (
    PaymentID SERIAL PRIMARY KEY,
    UserID INT NOT NULL,
    Amount DECIMAL(8,2) NOT NULL,
    PaymentDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    PaymentMethod VARCHAR(30)
        CHECK (PaymentMethod IN ('CreditCard', 'PayPal', 'BankTransfer')),
    Status VARCHAR(20)
        CHECK (Status IN ('Completed', 'Pending', 'Failed')),
    CONSTRAINT fk_payment_user
        FOREIGN KEY (UserID)
        REFERENCES "User"(UserID)
        ON DELETE CASCADE
);

-- ===============================
-- 10. INSERT PERSONS
-- ===============================
INSERT INTO Person (FirstName, LastName, Email, Password, DateOfBirth) VALUES
('Arben','Hoxha','arben.hoxha@mail.com','pass1','1995-01-10'),
('Elira','Krasniqi','elira.k@mail.com','pass2','1998-03-22'),
('Besnik','Gashi','besnik.g@mail.com','pass3','1994-07-14'),
('Anila','Berisha','anila.b@mail.com','pass4','1999-11-02'),
('Ilir','Meta','ilir.meta@mail.com','pass5','1989-05-19'),
('Drita','Shehu','drita.s@mail.com','pass6','1997-06-30'),
('Erion','Kola','erion.k@mail.com','pass7','1996-09-12'),
('Arta','Zeqiri','arta.z@mail.com','pass8','2000-02-08'),
('Gent','Musa','gent.m@mail.com','pass9','1993-12-21'),
('Flora','Leka','flora.l@mail.com','pass10','1998-10-01'),
('Altin','Bajrami','altin.b@mail.com','pass11','1992-04-18'),
('Rina','Sadiku','rina.s@mail.com','pass12','2001-01-27'),
('Ledion','Pepa','ledion.p@mail.com','pass13','1995-08-09'),
('Sara','Dervishi','sara.d@mail.com','pass14','1999-06-11'),
('Valon','Ismaili','valon.i@mail.com','pass15','1990-03-05'),
('Era','Shala','era.sh@mail.com','pass16','2002-07-17'),
('Blendi','Rama','blendi.r@mail.com','pass17','1988-12-03'),
('Ina','Halili','ina.h@mail.com','pass18','1997-09-29'),
('Lorik','Tahiri','lorik.t@mail.com','pass19','1994-02-14'),
('Klea','Morina','klea.m@mail.com','pass20','2000-11-25'),
('Fation','Beqiri','fation.b@mail.com','pass21','1996-04-06'),
('Alma','Rexha','alma.r@mail.com','pass22','1998-08-19'),
('Naim','Qosja','naim.q@mail.com','pass23','1985-01-01'),
('Teuta','Haziri','teuta.h@mail.com','pass24','1991-05-23'),
('Luan','Kelmendi','luan.k@mail.com','pass25','1987-10-10');


-- ===============================
-- 11. INSERT USERS
-- ===============================
INSERT INTO "User" (PersonID, Username) VALUES
(1,'arben95'),(2,'elira98'),(3,'besnik94'),(4,'anila99'),(5,'ilir89'),
(6,'drita97'),(7,'erion96'),(8,'arta00'),(9,'gent93'),(10,'flora98'),
(11,'altin92'),(12,'rina01'),(13,'ledion95'),(14,'sara99'),(15,'valon90'),
(16,'era02'),(17,'blendi88'),(18,'ina97'),(19,'lorik94'),(20,'klea00');

-- ===============================
-- 12. INSERT ADMINS
-- ===============================
INSERT INTO "Admin" (PersonID, AccessLevel, HireDate) VALUES
(21,'SuperAdmin','2021-01-01'),
(22,'ContentAdmin','2022-06-15');

-- ===============================
-- 13. INSERT MOVIES
-- ===============================
-- ===============================
-- 13. INSERT MOVIES (Expanded)
-- ===============================
INSERT INTO Movie (Title, Genre, ReleaseYear, DurationMinutes, Language) VALUES
('Shok','Drama',2015,90,'Albanian'),
('Bota','Drama',2014,104,'Albanian'),
('Zana','Drama',2019,97,'Albanian'),
('Hive','Drama',2021,84,'Albanian'),
('Slogans','History',2001,90,'Albanian'),
('Inception','Sci-Fi',2010,148,'English'),
('Titanic','Romance',1997,195,'English'),
('Avatar','Fantasy',2009,162,'English'),
('Gladiator','Action',2000,155,'English'),
('Joker','Drama',2019,122,'English'),
('Interstellar','Sci-Fi',2014,169,'English'),
('Matrix','Sci-Fi',1999,136,'English'),
('Godfather','Drama',1972,175,'English'),
('Scarface','Drama',1983,170,'English'),
('Rocky','Drama',1976,120,'English'),
('Rambo','Action',1982,93,'English'),
('The Dark Knight','Action',2008,152,'English'),
('Fight Club','Drama',1999,139,'English'),
('Parasite','Drama',2019,132,'Korean'),
('Oldboy','Action',2003,120,'Korean');

-- ===============================
-- 14. INSERT SUBSCRIPTIONS
-- ===============================
INSERT INTO Subscription (UserID, SubscriptionType, StartDate, EndDate) VALUES
(1,'Premium','2024-01-03','2024-06-15'),
(2,'Free','2024-01-01',NULL),
(3,'Premium','2024-02-10','2024-11-30'),
(4,'Free','2024-01-01',NULL),
(5,'Free','2024-01-01',NULL),
(6,'Premium','2024-01-18','2024-12-05'),
(7,'Free','2024-01-01',NULL),
(8,'Premium','2024-03-05','2024-12-22'),
(9,'Premium','2024-01-25','2024-07-30'),
(10,'Free','2024-01-01',NULL),
(11,'Free','2024-01-01',NULL),
(12,'Premium','2024-02-15','2024-10-20'),
(13,'Premium','2024-01-30','2024-08-25'),
(14,'Free','2024-01-01',NULL),
(15,'Premium','2024-02-22','2024-11-10'),
(16,'Free','2024-01-01',NULL),
(17,'Premium','2024-03-01','2024-12-18'),
(18,'Free','2024-01-01',NULL),
(19,'Premium','2024-01-12','2024-09-30'),
(20,'Free','2024-01-01',NULL);

-- ===============================
-- 15. INSERT PAYMENTS
-- ===============================
INSERT INTO Payment (UserID, Amount, PaymentMethod, Status) VALUES
(1,9.99,'CreditCard','Completed'),
(3,9.99,'PayPal','Completed'),
(6,9.99,'BankTransfer','Completed'),
(8,9.99,'CreditCard','Completed'),
(9,9.99,'PayPal','Completed'),
(12,9.99,'CreditCard','Completed'),
(13,9.99,'BankTransfer','Completed'),
(15,9.99,'PayPal','Completed'),
(17,9.99,'CreditCard','Completed'),
(19,9.99,'BankTransfer','Completed');

-- ===============================
-- 16. INSERT WATCHLISTS (Variable)
-- ===============================
INSERT INTO Watchlist (UserID, MovieID) VALUES
-- User 1: 3 movies
(1,(SELECT MovieID FROM Movie WHERE Title='Shok')),
(1,(SELECT MovieID FROM Movie WHERE Title='Inception')),
(1,(SELECT MovieID FROM Movie WHERE Title='Titanic')),

-- User 2: 1 movie
(2,(SELECT MovieID FROM Movie WHERE Title='Bota')),

-- User 3: 2 movies
(3,(SELECT MovieID FROM Movie WHERE Title='Zana')),
(3,(SELECT MovieID FROM Movie WHERE Title='Hive')),

-- User 4: 1 movie
(4,(SELECT MovieID FROM Movie WHERE Title='Slogans')),

-- User 5: 2 movies
(5,(SELECT MovieID FROM Movie WHERE Title='Avatar')),
(5,(SELECT MovieID FROM Movie WHERE Title='Gladiator')),

-- User 6: 3 movies
(6,(SELECT MovieID FROM Movie WHERE Title='Joker')),
(6,(SELECT MovieID FROM Movie WHERE Title='Interstellar')),
(6,(SELECT MovieID FROM Movie WHERE Title='Matrix')),

-- User 7: 1 movie
(7,(SELECT MovieID FROM Movie WHERE Title='Godfather')),

-- User 8: 2 movies
(8,(SELECT MovieID FROM Movie WHERE Title='Scarface')),
(8,(SELECT MovieID FROM Movie WHERE Title='Rocky')),

-- User 9: 3 movies
(9,(SELECT MovieID FROM Movie WHERE Title='Rambo')),
(9,(SELECT MovieID FROM Movie WHERE Title='The Dark Knight')),
(9,(SELECT MovieID FROM Movie WHERE Title='Fight Club')),

-- User 10: 1 movie
(10,(SELECT MovieID FROM Movie WHERE Title='Parasite')),

-- Users 11-20 (just one each for simplicity)
(11,(SELECT MovieID FROM Movie WHERE Title='Oldboy')),
(12,(SELECT MovieID FROM Movie WHERE Title='Shok')),
(13,(SELECT MovieID FROM Movie WHERE Title='Bota')),
(14,(SELECT MovieID FROM Movie WHERE Title='Zana')),
(15,(SELECT MovieID FROM Movie WHERE Title='Hive')),
(16,(SELECT MovieID FROM Movie WHERE Title='Slogans')),
(17,(SELECT MovieID FROM Movie WHERE Title='Inception')),
(18,(SELECT MovieID FROM Movie WHERE Title='Titanic')),
(19,(SELECT MovieID FROM Movie WHERE Title='Avatar')),
(20,(SELECT MovieID FROM Movie WHERE Title='Gladiator'));

-- ===============================
-- 17. INSERT RATINGS (Variable 1-3 per user, for 15 users)
-- ===============================
INSERT INTO Rating (Score, Comment, UserID, MovieID) VALUES
-- User 1: 3 ratings
(5,'Shume i mire',1,(SELECT MovieID FROM Movie WHERE Title='Shok')),
(4,'Interesant',1,(SELECT MovieID FROM Movie WHERE Title='Inception')),
(5,'Fantastik',1,(SELECT MovieID FROM Movie WHERE Title='Titanic')),

-- User 2: 2 ratings
(4,'Mire',2,(SELECT MovieID FROM Movie WHERE Title='Bota')),
(3,'Ok',2,(SELECT MovieID FROM Movie WHERE Title='Zana')),

-- User 3: 2 ratings
(5,'Kryeveper',3,(SELECT MovieID FROM Movie WHERE Title='Zana')),
(4,'Ja vlen',3,(SELECT MovieID FROM Movie WHERE Title='Hive')),

-- User 4: 1 rating
(3,'Mire',4,(SELECT MovieID FROM Movie WHERE Title='Slogans')),

-- User 5: 3 ratings
(4,'Interesant',5,(SELECT MovieID FROM Movie WHERE Title='Avatar')),
(5,'Fantastik',5,(SELECT MovieID FROM Movie WHERE Title='Gladiator')),
(5,'Kryeveper',5,(SELECT MovieID FROM Movie WHERE Title='Titanic')),

-- User 6: 2 ratings
(5,'I mrekullueshem',6,(SELECT MovieID FROM Movie WHERE Title='Joker')),
(4,'Ja vlen',6,(SELECT MovieID FROM Movie WHERE Title='Interstellar')),

-- User 7: 1 rating
(4,'Super',7,(SELECT MovieID FROM Movie WHERE Title='Godfather')),

-- User 8: 3 ratings
(5,'Perfekt',8,(SELECT MovieID FROM Movie WHERE Title='Scarface')),
(4,'Me pelqeu',8,(SELECT MovieID FROM Movie WHERE Title='Rocky')),
(5,'Fantastik',8,(SELECT MovieID FROM Movie WHERE Title='Rambo')),

-- User 9: 2 ratings
(5,'Shume i mire',9,(SELECT MovieID FROM Movie WHERE Title='The Dark Knight')),
(4,'Interesant',9,(SELECT MovieID FROM Movie WHERE Title='Fight Club')),

-- User 10: 1 rating
(3,'Ok',10,(SELECT MovieID FROM Movie WHERE Title='Parasite')),

-- User 11: 1 rating
(4,'Nice',11,(SELECT MovieID FROM Movie WHERE Title='Oldboy')),

-- User 12: 2 ratings
(5,'Super',12,(SELECT MovieID FROM Movie WHERE Title='Shok')),
(4,'Ja vlen',12,(SELECT MovieID FROM Movie WHERE Title='Bota')),

-- User 13: 1 rating
(5,'Excellent',13,(SELECT MovieID FROM Movie WHERE Title='Bota')),

-- User 14: 1 rating
(3,'Not bad',14,(SELECT MovieID FROM Movie WHERE Title='Zana')),

-- User 15: 3 ratings
(4,'Nice',15,(SELECT MovieID FROM Movie WHERE Title='Hive')),
(5,'Perfect',15,(SELECT MovieID FROM Movie WHERE Title='Titanic')),
(5,'Awesome',15,(SELECT MovieID FROM Movie WHERE Title='Inception'));
