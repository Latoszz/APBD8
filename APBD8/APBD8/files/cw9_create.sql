-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-04-20 04:46:38.52

-- tables
-- Table: Client
CREATE TABLE Client (
    IdClient int  NOT NULL IDENTITY,
    FirstName nvarchar(120)  NOT NULL,
    LastName nvarchar(120)  NOT NULL,
    Email nvarchar(120)  NOT NULL,
    Telephone nvarchar(120)  NOT NULL,
    Pesel nvarchar(120)  NOT NULL,
    CONSTRAINT Client_pk PRIMARY KEY  (IdClient)
);

-- Table: Client_Trip
CREATE TABLE Client_Trip (
    IdClient int  NOT NULL,
    IdTrip int  NOT NULL,
    RegisteredAt datetime  NOT NULL,
    PaymentDate datetime  NULL,
    CONSTRAINT Client_Trip_pk PRIMARY KEY  (IdClient,IdTrip)
);

-- Table: Country
CREATE TABLE Country (
    IdCountry int  NOT NULL IDENTITY,
    Name nvarchar(120)  NOT NULL,
    CONSTRAINT Country_pk PRIMARY KEY  (IdCountry)
);

-- Table: Country_Trip
CREATE TABLE Country_Trip (
    IdCountry int  NOT NULL,
    IdTrip int  NOT NULL,
    CONSTRAINT Country_Trip_pk PRIMARY KEY  (IdCountry,IdTrip)
);

-- Table: Trip
CREATE TABLE Trip (
    IdTrip int  NOT NULL IDENTITY,
    Name nvarchar(120)  NOT NULL,
    Description nvarchar(220)  NOT NULL,
    DateFrom datetime  NOT NULL,
    DateTo datetime  NOT NULL,
    MaxPeople int  NOT NULL,
    CONSTRAINT Trip_pk PRIMARY KEY  (IdTrip)
);

-- foreign keys
-- Reference: Country_Trip_Country (table: Country_Trip)
ALTER TABLE Country_Trip ADD CONSTRAINT Country_Trip_Country
    FOREIGN KEY (IdCountry)
    REFERENCES Country (IdCountry);

-- Reference: Country_Trip_Trip (table: Country_Trip)
ALTER TABLE Country_Trip ADD CONSTRAINT Country_Trip_Trip
    FOREIGN KEY (IdTrip)
    REFERENCES Trip (IdTrip);

-- Reference: Table_5_Client (table: Client_Trip)
ALTER TABLE Client_Trip ADD CONSTRAINT Table_5_Client
    FOREIGN KEY (IdClient)
    REFERENCES Client (IdClient);

-- Reference: Table_5_Trip (table: Client_Trip)
ALTER TABLE Client_Trip ADD CONSTRAINT Table_5_Trip
    FOREIGN KEY (IdTrip)
    REFERENCES Trip (IdTrip);


INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
VALUES
    ('Emma', 'Taylor', 'emma.taylor@example.com', '678-901-2345', '67890123456'),
    ('Oliver', 'Martin', 'oliver.martin@example.com', '789-012-3456', '78901234567'),
    ('Sophia', 'Clark', 'sophia.clark@example.com', '890-123-4567', '89012345678'),
    ('Liam', 'Rodriguez', 'liam.rodriguez@example.com', '901-234-5678', '90123456789'),
    ('Mia', 'Lewis', 'mia.lewis@example.com', '012-345-6789', '01234567890'),
    ('John', 'Doe', 'john.doe@example.com', '123-456-7890', '12345678901'),
    ('Jane', 'Smith', 'jane.smith@example.com', '234-567-8901', '23456789012'),
    ('Robert', 'Johnson', 'robert.johnson@example.com', '345-678-9012', '34567890123'),
    ('Alice', 'Williams', 'alice.williams@example.com', '456-789-0123', '45678901234'),
    ('Michael', 'Brown', 'michael.brown@example.com', '567-890-1234', '56789012345');
INSERT INTO Country (Name)
VALUES
    ('Italy'),
    ('Spain'),
    ('Japan'),
    ('Australia'),
    ('Brazil'),
    ('USA'),
    ('Canada'),
    ('Mexico'),
    ('France'),
    ('Germany');
INSERT INTO Trip (Name, Description, DateFrom, DateTo, MaxPeople)
VALUES
    ('Cultural Japan', 'Exploring the culture and history of Japan', '2024-11-01', '2024-11-15', 12),
    ('Italian Delights', 'Food and wine tour in Italy', '2024-12-01', '2024-12-10', 16),
    ('Spanish Fiesta', 'Enjoying festivals in Spain', '2025-01-01', '2025-01-10', 20),
    ('Aussie Adventure', 'Adventures in Australia', '2025-02-01', '2025-02-15', 22),
    ('Brazilian Carnival', 'Experience the Carnival in Brazil', '2025-03-01', '2025-03-10', 18),
    ('Trip to New York', 'A fun trip to the Big Apple', '2024-06-01', '2024-06-10', 20),
    ('Beach Vacation', 'Relaxing beach vacation', '2024-07-01', '2024-07-10', 15),
    ('Mountain Hiking', 'Adventure in the mountains', '2024-08-01', '2024-08-10', 10),
    ('European Tour', 'Visit multiple European countries', '2024-09-01', '2024-09-20', 25),
    ('Safari Adventure', 'Exciting safari in Africa', '2024-10-01', '2024-10-10', 18);
INSERT INTO Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate)
VALUES
    (1, 10, '2024-05-01', '2024-05-02'),
    (1, 3, '2024-05-03', '2024-05-04'),
    (2, 8, '2024-05-05', '2024-05-06'),
    (2, 4, '2024-05-07', '2024-05-08'),
    (3, 9, '2024-05-09', '2024-05-10'),
    (3, 5, '2024-05-11', '2024-05-12'),
    (4, 6, '2024-05-13', '2024-05-14'),
    (4, 1, '2024-05-15', '2024-05-16'),
    (5, 7, '2024-05-17', '2024-05-18'),
    (5, 2, '2024-05-19', '2024-05-20'),
    (12, 6, '2024-05-21', '2024-05-22'),
    (13, 7, '2024-05-23', '2024-05-24'),
    (14, 8, '2024-05-25', '2024-05-26'),
    (15, 9, '2024-05-27', '2024-05-28'),
    (16, 10, '2024-05-29', '2024-05-30');

INSERT INTO Country_Trip (IdCountry, IdTrip)
VALUES
    (1002, 1),
    (1002, 3),
    (2, 2),
    (1004, 4),
    (1005, 3),
    (1006, 5),
    (1007, 4),
    (1006, 1),
    (1011, 5),
    (1005, 2),
    (1006, 6),
    (1007, 7),
    (1008, 8),
    (1009, 9),
    (1010, 10);

-- End of file.

