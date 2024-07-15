USE master;
GO

-- Creación de la base de datos
CREATE DATABASE ListasDB;
GO

USE ListasDB;
GO

-- Creación de la tabla Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    Contraseña NVARCHAR(100) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);
GO

-- Creación de la tabla ListasDeReproduccion
CREATE TABLE ListasDeReproduccion (
    ListaID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);
GO

-- Creación de la tabla Canciones
CREATE TABLE Cancion (
    CancionID INT IDENTITY(1,1) PRIMARY KEY,
    ListaID INT,
    Titulo NVARCHAR(100) NOT NULL,
    Artista NVARCHAR(100),
    Album NVARCHAR(100),
    Duracion TIME,
	Enlace NVARCHAR(MAX) NULL,
    FechaAgregada DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ListaID) REFERENCES ListasDeReproduccion(ListaID)
);
GO
