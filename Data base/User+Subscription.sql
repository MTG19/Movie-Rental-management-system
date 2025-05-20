
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

