create database db_noticias;

IF OBJECT_ID(N'dbo.Noticia') IS NOT NULL  
   DROP TABLE [dbo].Noticia;  
GO

use db_noticias
CREATE TABLE Noticia (
    Id INT PRIMARY KEY IDENTITY,
    Titulo NVARCHAR(255) NOT NULL,
    Conteudo NVARCHAR(MAX) NOT NULL,
    DataPublicacao DATETIME NOT NULL,
    Autor NVARCHAR(255) NOT NULL
);

GO