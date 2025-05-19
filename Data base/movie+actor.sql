
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
