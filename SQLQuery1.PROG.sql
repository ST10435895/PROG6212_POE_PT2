CREATE DATABASE GUTClaimsDB;
GO
USE GUTClaimsDB;

CREATE TABLE Role (
    RoleId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

CREATE TABLE [User] (
    UserId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100) UNIQUE,
    RoleId INT FOREIGN KEY REFERENCES Role(RoleId)
);

CREATE TABLE Programme (
    ProgrammeId INT IDENTITY PRIMARY KEY,
    Code NVARCHAR(20),
    Name NVARCHAR(100)
);

CREATE TABLE Claim (
    ClaimId INT IDENTITY PRIMARY KEY,
    UserId INT FOREIGN KEY REFERENCES [User](UserId),
    ProgrammeId INT FOREIGN KEY REFERENCES Programme(ProgrammeId),
    MonthYear NVARCHAR(20),
    TotalAmount DECIMAL(18,2),
    CurrentStatus NVARCHAR(30) DEFAULT 'Pending',
    SubmittedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET(),
    Remarks NVARCHAR(255)
);

CREATE TABLE ClaimItem (
    ClaimItemId INT IDENTITY PRIMARY KEY,
    ClaimId INT FOREIGN KEY REFERENCES Claim(ClaimId),
    ItemType NVARCHAR(50),
    Description NVARCHAR(255),
    Hours DECIMAL(18,2),
    Amount DECIMAL(18,2)
);

CREATE TABLE SupportingDocument (
    DocumentId INT IDENTITY PRIMARY KEY,
    ClaimId INT FOREIGN KEY REFERENCES Claim(ClaimId),
    FileName NVARCHAR(255),
    FileUrl NVARCHAR(255),
    UploadedBy INT
);

CREATE TABLE ClaimStatusHistory (
    HistoryId INT IDENTITY PRIMARY KEY,
    ClaimId INT FOREIGN KEY REFERENCES Claim(ClaimId),
    Status NVARCHAR(30),
    ChangedBy INT,
    ChangedAt DATETIMEOFFSET DEFAULT SYSDATETIMEOFFSET(),
    Comment NVARCHAR(255)
);