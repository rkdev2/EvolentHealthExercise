CREATE DATABASE EvolentHealthExercise;

Use EvolentHealthExercise;

CREATE TABLE Contact (
    ID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    FirstName varchar(50) NOT NULL,
	LastName varchar(50) NOT NULL,
    Email varchar(254) NOT NULL,/*Max length for email is 254 char*/
	PhoneNumber varchar(10) NOT NULL,
	Status BIT NOT NULL
);

