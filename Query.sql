CREATE DATABASE MotoShop

USE MotoShop

CREATE TABLE Brand
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	BrandName VARCHAR(50) NOT NULL
)

CREATE TABLE Model
(
	Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	BrandId INT FOREIGN KEY(BrandId) 
		REFERENCES dbo.Brand(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	ModelName VARCHAR(50) NOT NULL
)

CREATE TABLE Type
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	TypeName VARCHAR(50) NOT NULL
)

CREATE TABLE Customer
(
	Login VARCHAR(50) NOT NULL PRIMARY KEY,
	Password VARCHAR(100) NOT NULL,
	Email VARCHAR(50) NOT NULL,
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	Phone CHAR(12),
	Role VARCHAR(9) NOT NULL DEFAULT 'User',
	CONSTRAINT EmailIsValid CHECK 
	(Email like '%___@___%'),
	CONSTRAINT PhoneIsValid CHECK 
	(Phone like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)

CREATE TABLE Motorcycle
(
	VIN CHAR(17) NOT NULL PRIMARY KEY,
	ModelId INT FOREIGN KEY(ModelId) 
		REFERENCES dbo.Model(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	TypeId INT FOREIGN KEY(TypeId) 
		REFERENCES dbo.Type(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
	EngineDisplacement FLOAT NOT NULL,
	Price FLOAT,
	Manufactored DATE NOT NULL,
	PhotoPath VARCHAR(60),
	Visible BIT DEFAULT 1,
	CONSTRAINT VinIsValid CHECK 
	(LEN(VIN) = 17)
)

CREATE TABLE Purchase
(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Customer VARCHAR(50) FOREIGN KEY(Customer)
		REFERENCES dbo.Customer(Login) ON DELETE SET NULL ON UPDATE NO ACTION,
	MotorcycleVIN CHAR(17) FOREIGN KEY(MotorcycleVIN) 
		REFERENCES dbo.Motorcycle(VIN) ON DELETE CASCADE ON UPDATE SET NULL
)

CREATE PROCEDURE Search  @Value VARCHAR(40) AS
	SELECT VIN, BrandName, ModelName, TypeName, EngineDisplacement, Manufactored, PhotoPath, Price 
	FROM Motorcycle JOIN Model ON Motorcycle.ModelId = Model.Id 
		JOIN Brand ON Model.BrandId = Brand.Id JOIN Type ON Motorcycle.TypeId = Type.Id
	WHERE VIN IN (
		SELECT VIN FROM (
			SELECT VIN, CONCAT(BrandName, ' ', ModelName, ' ', TypeName, ' ', EngineDisplacement, ' ', EngineDisplacement*1000) as SearchField  FROM Motorcycle JOIN Model ON Motorcycle.ModelId = Model.Id 
			JOIN Brand ON Model.BrandId = Brand.Id JOIN Type ON Motorcycle.TypeId = Type.Id
		) AS SubQuery
		WHERE SearchField LIKE '%' + @Value + '%'
	) AND Visible = 1

CREATE TRIGGER null_visible_after_purchase ON Purchase
AFTER INSERT AS
	UPDATE Motorcycle SET Visible = 0
	WHERE VIN = (SELECT MotorcycleVIN FROM inserted)

CREATE TRIGGER type_delete
ON Type
AFTER DELETE AS
	UPDATE Motorcycle SET Visible = 0
	WHERE TypeId IS NULL

CREATE TRIGGER brand_delete
ON Brand
AFTER DELETE AS
	UPDATE Motorcycle SET Visible = 0
	WHERE ModelId IN (SELECT Id FROM Model WHERE BrandId IS NULL)

CREATE TRIGGER model_delete
ON Model
AFTER DELETE AS 
	UPDATE Motorcycle SET Visible = 0
	WHERE ModelId IS NULL
