CREATE Database MovieRental

use [MovieRental];
GO

CREATE TABLE Genre (
    GenreID INT PRIMARY KEY,
    GenreName VARCHAR(50) NOT NULL
);

CREATE TABLE Address (
    AddressID INT PRIMARY KEY,
    Street VARCHAR(255),
    City VARCHAR(100),
    PostalCode VARCHAR(20),
    Country VARCHAR(100)
);


CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY,
    SupplierName VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Phone VARCHAR(20),
    AddressID INT,
    FOREIGN KEY (AddressID) REFERENCES Address(AddressID)
);

CREATE TABLE Actor (
    ActorID INT PRIMARY KEY,
    Name NVARCHAR(100),
    BirthDate DATE
);

CREATE TABLE Movie (
    MovieID INT PRIMARY KEY,
    Title NVARCHAR(200),
    SupplierID INT,
    GenreID INT,
    LeadActorID INT,
    RentalPrice DECIMAL(10,2),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (GenreID) REFERENCES Genre(GenreID),
    FOREIGN KEY (LeadActorID) REFERENCES Actor(ActorID)
);

CREATE TABLE Acted_in (
    ActorID INT,
    MovieID INT,
    PRIMARY KEY (ActorID, MovieID),
    FOREIGN KEY (ActorID) REFERENCES Actor(ActorID),
    FOREIGN KEY (MovieID) REFERENCES Movie(MovieID)
);


CREATE TABLE Users
(

UserID int,
name varchar(50) not null,
email varchar(50) not null,
phone varchar(20) unique,
Creditcardnumber varchar(20) unique,
constraint user_pk primary key (UserID)

);

CREATE TABLE Subscription (
    SubscriptionID INT PRIMARY KEY,
    UserID INT,
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
    SubscribingDate DATE,
    PrepaidMonths INT,
    EndDate DATE
);

CREATE TABLE rentingOrder (
    RentalID INT PRIMARY KEY,
    rentingDate DATE,
    returnDate DATE,
    UserID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE payment(
	PaymentNo int primary key,
	RentalId int foreign key REFERENCES rentingOrder(RentalID),
	amount DECIMAL(10, 2),
	PaymentDate Date,
	Method VARCHAR(MAX)
);

CREATE TABLE library (
	tapeID int primary key,
	movieID int foreign key REFERENCES movie(movieID)
);


CREATE TABLE rentingDetail (
    RentalID INT,
    TapeID INT,
    rentingPrice DECIMAL,
    PRIMARY KEY (RentalID, TapeID),
    FOREIGN KEY (RentalID) REFERENCES rentingOrder(RentalID),
    FOREIGN KEY (TapeID) REFERENCES Library(TapeID)
);

ALTER TABLE Users ADD Password VARCHAR(100) NOT NULL DEFAULT '1234';
ALTER TABLE Users ADD Role VARCHAR(20) DEFAULT 'user';

INSERT INTO Users (Name, Email, Phone, CreditCardNumber, Password, Role)
VALUES ('Admin Israa', 'israa@gmail.com', '01094415513', '4567123412341234', '12345', 'admin');


INSERT INTO Subscription (SubscriptionID, UserID, SubscribingDate, PrepaidMonths, EndDate)
VALUES (1, 1, '2025-05-06', 3, '2025-08-06');

select*from [Users];
select*from Subscription;

ALTER TABLE rentingDetail
DROP CONSTRAINT FK__rentingDe__Movie__5812160E;

ALTER TABLE rentingDetail
ADD CONSTRAINT FK_rentingDetail_Movie
FOREIGN KEY (MovieID) REFERENCES Movie(MovieID)
ON DELETE CASCADE;