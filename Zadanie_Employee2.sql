IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee2')
BEGIN
    CREATE TABLE Employee2 (
        PersonalNumber VARCHAR(50) PRIMARY KEY CHECK (LEN(PersonalNumber) <=10),
        FirstName VARCHAR(50) NOT NULL,
        Surname VARCHAR(50) NOT NULL,
        DateOfBirth DATETIME NOT NULL,
        IdentificationNumber INT UNIQUE NOT NULL, 
		CONSTRAINT UQ_Emp2 UNIQUE (FirstName,Surname)
    );
END;

DROP TABLE IF EXISTS Employee2
SELECT * FROM Employee2

INSERT INTO Employee2 (PersonalNumber, FirstName, Surname, DateOfBirth, IdentificationNumber) 
VALUES ('123456789', 'John', 'Doe', '1990-01-01', 1000), 
		('amdh22.a', 'Suzanne', 'Doe', '1990-02-02', 1001),
		('987.nfh', 'Elizabeth', 'Taylor', '1999-03-03', 1002),
		('9111.09.m', 'Jane', 'Smith', '2000-09-28', 1003),
		('diw-11-11', 'Bob', 'Marley', '1995-09-30', 1004),
		('diw-11-12', 'David', 'Johnson', '1995-09-30', 1005),
		('diw-11-13', 'Roger', 'David', '1995-09-30', 1006),
		('diw-11-14', 'Rafael', 'David', '1995-09-30', 1007)



INSERT INTO Employee2 (PersonalNumber, FirstName, Surname, DateOfBirth, IdentificationNumber) 
VALUES  ('9111.09.m', 'Jane', 'Smith', '2000-09-28', 1003),
		('987.nfh', 'Elizabeth', 'Taylor', '1999-03-03', 1002),
		('amdh22.a', 'Suzanne', 'Doe', '1990-02-02', 1001),
		('diw-11-11', 'Bob', 'Marley', '1995-09-30', 1004),
		('diw-11-13', 'Roger', 'David', '1995-09-30', 1006),
		('diw-11-14', 'Rafael', 'David', '1995-09-30', 1007)




INSERT INTO Employee2 (PersonalNumber, FirstName, Surname, DateOfBirth, IdentificationNumber) 
VALUES ('987.nfh', 'Leonardo', 'Michale', '1999-01-01', 1028)
DELETE FROM Employee WHERE FirstName = 'Leonardo'

INSERT INTO Employee2 (PersonalNumber, FirstName, Surname, DateOfBirth, IdentificationNumber) 
VALUES ('4', 'Emil', 'Lopez', '1999-01-01', 1031)
DELETE FROM Employee WHERE FirstName = 'David' AND Surname = 'Lopez'



--Add employee to the table
CREATE PROCEDURE AddEmployee2 
@PersonalNumber VARCHAR (50),
@FirstName VARCHAR (50),
@Surname VARCHAR(50),
@DateOfBirth DATETIME,
@IdentificationNumber INT
AS
BEGIN
	INSERT INTO Employee2 (PersonalNumber, FirstName, Surname, DateOfBirth, IdentificationNumber) 
	VALUES (@PersonalNumber, @FirstName, @Surname, @DateOfBirth, @IdentificationNumber)
END

EXEC AddEmployee2 '2', 'Jano', 'Doe', '1990-01-01', 10010

SELECT * FROM Employee2

DROP PROCEDURE GetEmpDetailsByPN2
--Get Lead Details By Id
CREATE PROCEDURE GetEmpDetailsByPN2
@PersonalNumber	VARCHAR(50)								
AS
BEGIN
	SELECT * FROM Employee2 
	WHERE PersonalNumber=@PersonalNumber
END

EXEC GetEmpDetailsByPN2  2





CREATE PROCEDURE InsertEmployee2
    @PersonalNumber VARCHAR(50),
    @FirstName VARCHAR(50),
    @Surname VARCHAR(50),
    @DateOfBirth DATETIME,
    @IdentificationNumber INT
AS
BEGIN
    INSERT INTO Employee2 (PersonalNumber, FirstName, Surname, DateOfBirth, IdentificationNumber)
    VALUES (@PersonalNumber, @FirstName, @Surname, @DateOfBirth, @IdentificationNumber)
END

EXEC InsertEmployee2 2











SELECT * FROM Employee2



DROP PROCEDURE EditEmp2
--Edit Employee
CREATE PROCEDURE EditEmp2
	@PersonalNumber VARCHAR(50),
	@FirstName VARCHAR(100),
	@Surname VARCHAR(100),
	@DateOfBirth DATETIME,
	@IdentificationNumber INT
AS
BEGIN
	UPDATE Employee2
	SET FirstName = @FirstName, Surname = @Surname,
		DateOfBirth = @DateOfBirth, IdentificationNumber = @IdentificationNumber
	WHERE PersonalNumber = @PersonalNumber
END

EXEC EditEmp2 2

SELECT * FROM Employee2
DROP PROCEDURE DeleteEmp2
--Delete Record 
CREATE PROCEDURE DeleteEmp2
@PersonalNumber VARCHAR(50) 
AS
BEGIN
	DELETE FROM Employee2 
	WHERE PersonalNumber = @PersonalNumber
END

SELECT * FROM Employee2
EXEC DeleteEmp2 2









---------------------------------------------------------------------------------------------------City table


IF NOT EXISTS (SELECT 2 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'City2')
BEGIN
    CREATE TABLE City2 (
		CityIdentificator INT IDENTITY(1,1) PRIMARY KEY,
        CityName VARCHAR(50) NOT NULL,
        Country VARCHAR(50) NOT NULL,
		Longitude FLOAT,
        Latitude  FLOAT
    );
END;

SELECT * FROM City2
DROP TABLE City2


INSERT INTO City2 (CityName, Country, Longitude, Latitude) 
VALUES ('New York', 'USA', 12.354, 11.145), 
		('New York', 'USA', 12.354, 11.145),
		('Madrid', 'Spain', 10.354, 11.145),
		('Tokyo', 'Japan', 14.354, 24.654),
		('Paris', 'France', 27.354, 11.524)






DROP PROCEDURE GetCityByCI2
--Stored Procedure for Edit City
CREATE PROCEDURE GetCityByCI2
	@CityIdentificator INT
AS
BEGIN
	SELECT * FROM City2
	WHERE CityIdentificator=@CityIdentificator
END

EXEC GetCityByCI2 1




SELECT * FROM City2



DROP PROCEDURE DeleteCity2
--Delete Record 
CREATE PROCEDURE DeleteCity2
	@CityIdentificator VARCHAR(50) 
AS
BEGIN
	DELETE FROM City2 
	WHERE CityIdentificator = @CityIdentificator
END

EXEC DeleteCity2 4
SELECT * FROM City2

DROP PROCEDURE EditCity2
--Edit City
CREATE PROCEDURE EditCity2
	@CityIdentificator VARCHAR(50),
	@CityName VARCHAR(50),
	@Country VARCHAR(50),
	@Longitude FLOAT,
	@Latitude FLOAT
AS
BEGIN
	UPDATE City2 
	SET CityName = @CityName,
		Country = @Country, Longitude = @Longitude, Latitude = @Latitude
	WHERE CityIdentificator = @CityIdentificator
END

EXEC EditCity2  2

































--------------------------------------------------------------------------------------------CP table



DROP TABLE CP2
SELECT * FROM CP2

CREATE TABLE CP2 (
    CP_Identificator INT IDENTITY(1,1) PRIMARY KEY,
    CP_Date DATETIME DEFAULT GETDATE(),
    EmployeePersonalNumber VARCHAR(50) NOT NULL UNIQUE,
    StartCityID INT NOT NULL,
    EndCityID INT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,
    TransportationMode VARCHAR (50) CHECK (TransportationMode IN ('Služobné auto', 'Autobus', 'MHD', 'Pešo', 'Vlak', 'Taxi', 'Lietadlo') OR TransportationMode IS NULL ),
    Status VARCHAR(50) CHECK (Status IN ('Vytvorený', 'Schválený', 'Vyúètovaný', 'Zrušený') OR Status IS NULL ),
    FOREIGN KEY (EmployeePersonalNumber) REFERENCES Employee2 (PersonalNumber) ON DELETE CASCADE,
    FOREIGN KEY (StartCityID) REFERENCES City2 (CityIdentificator) ON DELETE CASCADE,
    FOREIGN KEY (EndCityID) REFERENCES City2 (CityIdentificator)
);

SELECT * FROM CP2

SELECT * FROM City2

SELECT * FROM Employee2
SELECT * FROM City2

SELECT * FROM CP2
DELETE FROM CP2

INSERT INTO CP2 (EmployeePersonalNumber, StartCityID, 
                EndCityID, StartTime, EndTime, TransportationMode, Status)
VALUES
('9111.09.m', 2, 2, '2022-01-01 08:00', '2022-01-01 18:00', 'Taxi', 'Vytvorený'),
('987.nfh', 4, 5,  '2022-02-02 09:00', '2022-02-02 17:00', 'MHD', 'Schválený'),
('diw-11-11', 5, 5, '2022-03-03 10:00', '2022-03-03 20:00', 'Autobus', 'Vyúètovaný'),
('diw-11-14', 2, 5, '2022-04-04 11:00', '2022-04-04 19:00', NULL, NULL);

INSERT INTO CP2 (EmployeePersonalNumber, StartCityID, 
                EndCityID, StartTime, EndTime, TransportationMode, Status)
VALUES
('987.nfh', 2, 2, '2022-01-01 08:00', '2022-01-01 18:00', 'Taxi', 'Vytvorený')
DELETE FROM CP2 WHERE EmployeePersonalNumber = '987.nfh'

--Stored Procedure for Edit CP2
CREATE PROCEDURE GetCPByPN2
	@CP_Identificator INT
AS
BEGIN
	SELECT * FROM CP2
	WHERE CP_Identificator=@CP_Identificator
END

EXEC GetCPByPN 4


DROP PROCEDURE GetAllCp2
--SELECT * FROM CP2s
CREATE PROCEDURE GetAllCp2
AS 
BEGIN
	SELECT * FROM CP2
END

EXEC GetAllCp2

SELECT * FROM CP2

DROP PROCEDURE GetCPByPN2;

SELECT * FROM City2

SELECT * FROM CP2


    EmployeePersonalNumber VARCHAR(50) NOT NULL,
    StartCityID INT NOT NULL,
    EndCityID INT NOT NULL,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NOT NULL,

DROP PROCEDURE EditCP2
--Edit CP
CREATE PROCEDURE EditCP2
	@CP_Identificator INT,
	@CP_Date VARCHAR(100),
	@EmployeePersonalNumber VARCHAR(50),
	@StartCityID INT,
	@EndCityID INT,
	@StartTime DATETIME,
	@EndTime DATETIME
AS
BEGIN
	UPDATE CP2 
	SET CP_Date  = @CP_Date , EmployeePersonalNumber = @EmployeePersonalNumber,
		StartCityID = @StartCityID, EndCityID = @EndCityID, 
		StartTime = @StartTime, EndTime = @EndTime
	WHERE CP_Identificator = @CP_Identificator
END

EXEC EditCP2 2
 







 
--Stored Procedure for Edit CP
CREATE PROCEDURE GetCPByPN2
	@CP_Identificator INT
AS
BEGIN
	SELECT * FROM CP2
	WHERE CP_Identificator=@CP_Identificator
END

EXEC GetCPByPN2 4






SELECT * FROM CP2
DROP PROCEDURE DeleteCP2
--Delete CP 
CREATE PROCEDURE DeleteCP2
	@CP_Identificator INT 
AS
BEGIN
	DELETE FROM CP2 
	WHERE CP_Identificator = @CP_Identificator
END

EXEC DeleteCP2 3