-- Crear la base de datos
CREATE DATABASE BD_Seminarios;
GO

USE BD_Seminarios;
GO

-- 1. CREAR TABLA SEMINARIO
CREATE TABLE Seminario (
    CodigoSeminario VARCHAR(10) PRIMARY KEY,
    NombreCurso VARCHAR(100) NOT NULL,
    HorariosClase VARCHAR(50) NOT NULL,
    CapacidadSeminario INT NOT NULL CHECK (CapacidadSeminario >= 0),
    FotoUrl VARCHAR(255) NULL
);
GO

-- 2. CREAR TABLA REGISTRO ASISTENCIA
CREATE TABLE RegistroAsistencia (
    NumeroRegistro INT IDENTITY(1,1),
    CodigoSeminario VARCHAR(10) NOT NULL,
    CodigoEstudiante VARCHAR(10) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (CodigoSeminario, CodigoEstudiante),
    FOREIGN KEY (CodigoSeminario) REFERENCES Seminario(CodigoSeminario)
);
GO

-- 3. PROCEDURE LISTAR SEMINARIOS CON CAPACIDAD > 0
CREATE PROCEDURE sp_ListarSeminarios
AS
BEGIN
    SELECT CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl
    FROM Seminario
    WHERE CapacidadSeminario > 0;
END;
GO

-- 4. PROCEDURE INSERTAR REGISTRO Y ACTUALIZAR CAPACIDAD
CREATE PROCEDURE sp_RegistrarAsistencia
    @CodigoSeminario VARCHAR(10),
    @CodigoEstudiante VARCHAR(10),
    @NumeroRegistro INT OUTPUT
AS
BEGIN
    INSERT INTO RegistroAsistencia (CodigoSeminario, CodigoEstudiante)
    VALUES (@CodigoSeminario, @CodigoEstudiante);
    
    SET @NumeroRegistro = SCOPE_IDENTITY();
    
    UPDATE Seminario
    SET CapacidadSeminario = CapacidadSeminario - 1
    WHERE CodigoSeminario = @CodigoSeminario;
END;
GO

-- 5. BUSCAR SEMINARIO POR CODIGO
CREATE PROCEDURE sp_ObtenerSeminario
    @CodigoSeminario VARCHAR(10)
AS
BEGIN
    SELECT CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl
    FROM Seminario
    WHERE CodigoSeminario = @CodigoSeminario;
END;
GO

-- 6. INSERTAR 6 REGISTROS EN SEMINARIO CON FOTOS
INSERT INTO Seminario (CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl) 
VALUES ('SEM001', 'Programación Básica', 'Lunes 14:00-16:00', 30, 'https://images.unsplash.com/photo-1542831371-29b0f74f9713?w=500');

INSERT INTO Seminario (CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl) 
VALUES ('SEM002', 'Base de Datos SQL', 'Martes 16:00-18:00', 25, 'https://images.unsplash.com/photo-1544383835-bda2bc66a55d?w=500');

INSERT INTO Seminario (CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl) 
VALUES ('SEM003', 'Desarrollo Web', 'Miércoles 10:00-12:00', 35, 'https://images.unsplash.com/photo-1547658719-da2b51169166?w=500');

INSERT INTO Seminario (CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl) 
VALUES ('SEM004', 'Algoritmos', 'Jueves 14:00-16:00', 28, 'https://images.unsplash.com/photo-1509228468518-180dd4864904?w=500');

INSERT INTO Seminario (CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl) 
VALUES ('SEM005', 'Excel Avanzado', 'Viernes 16:00-18:00', 20, 'https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=500');

INSERT INTO Seminario (CodigoSeminario, NombreCurso, HorariosClase, CapacidadSeminario, FotoUrl) 
VALUES ('SEM006', 'Matemática Aplicada', 'Sábado 09:00-11:00', 32, 'https://images.unsplash.com/photo-1635070041078-e363dbe005cb?w=500');
GO