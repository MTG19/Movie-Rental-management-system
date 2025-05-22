CREATE Database MovieRental


use [MovieRental];
GO

CREATE TABLE Genre (
    GenreID INT PRIMARY KEY Identity(1,1),
    GenreName VARCHAR(50) NOT NULL
);


CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY Identity(1,1),
    SupplierName VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Phone VARCHAR(20),
    AddressID INT,
    FOREIGN KEY (AddressID) REFERENCES Address(AddressID)
);


CREATE TABLE Address (
    AddressID INT PRIMARY KEY Identity(1,1),
    Street VARCHAR(255),
    City VARCHAR(100),
    PostalCode VARCHAR(20),
    Country VARCHAR(100)
);


CREATE TABLE Actor (
    ActorID INT PRIMARY KEY Identity(1,1),
    Name NVARCHAR(100),
    BirthDate DATE
);

CREATE TABLE Movie (
    MovieID INT PRIMARY KEY Identity(1,1),
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


CREATE TABLE Users (
	UserID int Identity(1,1),
	name varchar(50) not null,
	email varchar(50) not null,
	phone varchar(20) unique,
	Creditcardnumber varchar(20) unique,
	Password VARCHAR(100) NOT NULL DEFAULT '1234',
	Role VARCHAR(20) DEFAULT 'user',
	constraint user_pk primary key (UserID)
);

CREATE TABLE Subscription (
    SubscriptionID INT PRIMARY KEY Identity(1,1),
    UserID INT,
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
    SubscribingDate DATE,
    PrepaidMonths INT,
    EndDate DATE
);

CREATE TABLE rentingOrder (
    RentalID INT PRIMARY KEY Identity(1,1),
    rentingDate DATE,
    returnDate DATE,
    UserID INT,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
 
CREATE TABLE payment(
	PaymentNo int primary key Identity(1,1),
	RentalId int foreign key REFERENCES rentingOrder(RentalID),
	amount DECIMAL(10, 2),
	PaymentDate Date,
	Method VARCHAR(MAX)
);
 
CREATE TABLE rentingDetail (
    RentalID INT,
    MovieID INT,
    rentingPrice DECIMAL,
    PRIMARY KEY (RentalID, MovieID),
    FOREIGN KEY (RentalID) REFERENCES rentingOrder(RentalID),
    FOREIGN KEY (MovieID) REFERENCES Movie(MovieID)
);

alter table Users
add constraint uniqueness unique (email)


INSERT INTO Users (Name, Email, Phone, CreditCardNumber, Password, Role)
VALUES ('user1', 'user@gmail.com', '0101234123', '4567123412341235', 'user1234', 'user'), ('Admin Israa', 'israa@gmail.com', '01094415513', '4567123412341234', '12345', 'admin');


INSERT INTO Subscription (UserID, SubscribingDate, PrepaidMonths, EndDate)
VALUES (1, '2025-05-06', 3, '2025-08-06');

select*from [Users];
select*from Subscription;