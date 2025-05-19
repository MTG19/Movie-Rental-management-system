CREATE TABLE rentingOrder (
    RentalID INT PRIMARY KEY,
    rentingDate DATE,
    returnDate DATE,
    UserID INT,
    FOREIGN KEY (UserID) REFERENCES user(UserID)
);

CREATE TABLE rentingDetail (
    RentalID INT,
    TapeID INT,
    rentingPrice DOUBLE,
    PRIMARY KEY (RentalID, TapeID),
    FOREIGN KEY (RentalID) REFERENCES rentingOrder(RentalID),
    FOREIGN KEY (TapeID) REFERENCES Library(TapeID)
);
