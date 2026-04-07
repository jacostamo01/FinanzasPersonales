package conexion;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

/**
 * Clase encargada de la conexion a la base de datos MariaDB.
 * Usa XAMPP con la base de datos "finanzas_db".
 *
 * Conceptos de POO usados:
 * - CONSTANTES (static final): los datos de conexion no cambian
 * - METODO ESTATICO: se llama sin crear un objeto (Conexion.obtenerConexion())
 */
public class Conexion {

    // Constantes de conexion (no cambian en toda la ejecucion)
    private static final String URL = "jdbc:mariadb://localhost:3306/finanzas_db";
    private static final String USUARIO = "root";
    private static final String PASSWORD = "";

    /**
     * Crea y retorna una conexion a la base de datos.
     * Si no se puede conectar, retorna null.
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