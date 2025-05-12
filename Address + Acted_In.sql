CREATE TABLE Address (
    AddressID INT PRIMARY KEY,
    Street VARCHAR(255),
    City VARCHAR(100),
    PostalCode VARCHAR(20),
    Country VARCHAR(100)
);

CREATE TABLE Acted_in (
    ActorID INT,
    MovieID INT,
    PRIMARY KEY (ActorID, MovieID),
    FOREIGN KEY (ActorID) REFERENCES Actor(ActorID),
    FOREIGN KEY (MovieID) REFERENCES Movie(MovieID)
);
