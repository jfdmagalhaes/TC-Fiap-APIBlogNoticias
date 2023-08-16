create database db_noticias;

IF OBJECT_ID(N'dbo.Noticias') IS NOT NULL  
   DROP TABLE [dbo].Noticias;  
GO

use db_noticias
CREATE TABLE Noticias (
    Id INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(255) NOT NULL,
    Conteudo NVARCHAR(MAX) NOT NULL,
    DataPublicacao DATETIME NOT NULL,
    Autor NVARCHAR(255) NOT NULL
);

GO