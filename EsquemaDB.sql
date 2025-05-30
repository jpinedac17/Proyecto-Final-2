-- Crear base de datos
CREATE DATABASE InvestigacionesAI;

-- Crear tabla
CREATE TABLE Investigaciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Prompt NVARCHAR(MAX) NOT NULL,
    Respuesta NVARCHAR(MAX) NOT NULL,
    FechaCreacion DATETIME DEFAULT GETDATE()
);