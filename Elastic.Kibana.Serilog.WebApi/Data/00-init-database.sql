USE master
GO
IF NOT EXISTS (
    SELECT [name]
        FROM sys.databases
        WHERE [name] = N'Projeto42'
)
CREATE DATABASE Projeto42
GO

USE Projeto42
GO

CREATE TABLE Cidade
(
    Id INT IDENTITY PRIMARY KEY,
    Nome VARCHAR(100) not null,
    Uf char(2) not null
);
GO

CREATE TABLE Pessoa
(
    Id INT IDENTITY PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cpf char(11)
);
GO


DECLARE 
    @i INT = 1

while (@i <= 100)
BEGIN
     INSERT INTO Pessoa values ('Pessoa ' + CAST(@i as varchar), CAST(@i as varchar));

     SET @i = @i + 1;
END

INSERT INTO Cidade
SELECT DISTINCT Localidade, SiglaUf  FROM Correios.dbo.Ceps