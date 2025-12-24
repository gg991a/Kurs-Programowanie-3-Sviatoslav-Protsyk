CREATE TABLE [dbo].[Produkty] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Nazwa]       NVARCHAR (100)  NOT NULL,
    [Cena]        DECIMAL (10, 2) NOT NULL,
    [KategoriaId] INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([KategoriaId]) REFERENCES [dbo].[Kategorie] ([Id])
);

