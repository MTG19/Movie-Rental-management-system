use [Movie rental];
GO

create table [user]
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
    UserID INT FOREIGN KEY REFERENCES [User](UserID),
    SubscribingDate DATE,
    PrepaidMonths INT,
    EndDate DATE
);

INSERT INTO [User] (UserID, Name, Email, Phone, CreditCardNumber)
VALUES (1, 'Shosho', 'shosho@email.com', '0123456789', '4567123412341234');

INSERT INTO Subscription (SubscriptionID, UserID, SubscribingDate, PrepaidMonths, EndDate)
VALUES (1, 1, '2025-05-06', 3, '2025-08-06');

select*from [user];
select*from Subscription;