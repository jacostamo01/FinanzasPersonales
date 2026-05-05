-- Script SQL para crear las tablas faltantes en finanzas_db
-- Compatible con la estructura existente

USE finanzas_db;

-- Tabla de ahorros
CREATE TABLE IF NOT EXISTS ahorros (
	id INT AUTO_INCREMENT PRIMARY KEY,
	usuario_id INT,
	monto_objetivo DOUBLE NOT NULL,
	monto_actual DOUBLE DEFAULT 0,
	descripcion VARCHAR(200),
	fecha_creacion TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	fecha_objetivo DATETIME NULL,
	FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);

-- Tabla de inversiones
CREATE TABLE IF NOT EXISTS inversiones (
	id INT AUTO_INCREMENT PRIMARY KEY,
	usuario_id INT,
	monto_inicial DOUBLE NOT NULL,
	valor_actual DOUBLE NOT NULL,
	tasa_interes DOUBLE NOT NULL,
	descripcion VARCHAR(200),
	fecha_inicio TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	fecha_vencimiento DATETIME NULL,
	FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);

-- Separar la tabla movimientos en ingresos y gastos para mejor compatibilidad con EF Core
CREATE TABLE IF NOT EXISTS ingresos (
	id INT AUTO_INCREMENT PRIMARY KEY,
	usuario_id INT,
	monto DOUBLE NOT NULL,
	descripcion VARCHAR(200),
	fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS gastos (
	id INT AUTO_INCREMENT PRIMARY KEY,
	usuario_id INT,
	monto DOUBLE NOT NULL,
	descripcion VARCHAR(200),
	categoria VARCHAR(100),
	fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);

-- Consultas de verificación
SELECT 'Tablas creadas exitosamente' AS mensaje;
SHOW TABLES;
