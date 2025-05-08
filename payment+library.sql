-- payment and library

create table payment(
	PaymentNo int primary key,
	RentalId int foreign key REFERENCES RentingOrder(RentalID),
	amount DECIMAL(10, 2),
	PaymentDate Date,
	Method VARCHAR(MAX)
);

create table library(
	tapeID int primary key,
	movieID int foreign key REFERENCES movie(movieID)
);