package conexion;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

/**
 * Clase que maneja la conexión a la base de datos MariaDB.
 * Usa XAMPP con la base de datos "finanzas_db".
 */
public class Conexion {

    // Datos de conexión a la base de datos
    private static final String URL = "jdbc:mariadb://localhost:3308/finanzas_db";
    private static final String USUARIO = "root";
    private static final String PASSWORD = ""; // XAMPP por defecto no tiene contraseña

    /**
     * Crea y retorna una conexión a la base de datos.
     * Retorna null si no se pudo conectar.
     */
    public static Connection obtenerConexion() {
        try {
            Connection conexion = DriverManager.getConnection(URL, USUARIO, PASSWORD);
            return conexion;
        } catch (SQLException e) {
            System.out.println("Error al conectar a la base de datos: " + e.getMessage());
            return null;
        }
    }
}