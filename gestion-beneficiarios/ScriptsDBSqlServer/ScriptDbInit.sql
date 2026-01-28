CREATE DATABASE GestionBeneficiariosDB;
GO

USE GestionBeneficiariosDB;
GO

CREATE TABLE IdentityDocument(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	Abbreviation VARCHAR(10) NOT NULL,
	Country VARCHAR(50) NOT NULL,
	Length INT NOT NULL,
	IsNumeric BIT NOT NULL,
	IsActive BIT
);
GO

CREATE TABLE Beneficiary (
	Id INT IDENTITY(1,1) PRIMARY KEY,
	FirstName VARCHAR(100) NOT NULL,
	LastName VARCHAR(100) NOT NULL,
	IdentityDocumentId INT NOT NULL,
	DocumentNumber VARCHAR(20) NOT NULL,
	BirthDate DATE NOT NULL,
	Gender CHAR(1) NOT NULL,

	CONSTRAINT FK_Beneficiary_IdentityDocument
	FOREIGN KEY (IdentityDocumentId)
	REFERENCES IdentityDocument(Id),

	CONSTRAINT CK_Beneficiary_Gender
	CHECK (Gender IN ('M', 'F'))

);
GO

INSERT INTO IdentityDocument
(Name, Abbreviation, Country, Length, IsNumeric, IsActive)
VALUES
('Documento Nacional de Identidad', 'DNI', 'Perú', 8, 1, 1),
('Pasaporte', 'PAS', 'Perú', 9, 0, 1);
GO

INSERT INTO Beneficiary
(FirstName, LastName, IdentityDocumentId, DocumentNumber, BirthDate, Gender)
VALUES
('Juan Carlos', 'Pérez Gómez', 1, '12345678', '1990-05-10', 'M'),
('María Elena', 'Lopez Ruiz', 2, 'XK3456789', '1995-08-22', 'F');
GO