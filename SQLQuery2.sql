﻿CREATE TABLE RefreshTokens (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Token NVARCHAR(4000) NOT NULL,
    UserId INT NOT NULL,
    ExpiryDate DATETIME NOT NULL,
    CONSTRAINT FK_RefreshTokens_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);