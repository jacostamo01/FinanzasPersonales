package modelo;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import conexion.Conexion;

/**
 * Representa un usuario del sistema.
 *
 * Conceptos de POO usados:
 * - ENCAPSULAMIENTO: atributos privados
 * - METODOS ESTATICOS: autenticar y registrar no necesitan instancia
 * - TRY-WITH-RESOURCES: cierra conexiones automaticamente
 */
public class Usuario {

    /**
     * Verifica si el usuario y contraseña existen en la base de datos.
     * Usa try-with-resources para cerrar conexion automaticamente.
     */
    public static boolean autenticar(String user, String pass) {
        String sql = "SELECT id FROM usuarios WHERE username = ? AND password = ?";

        try (Connection conn = Conexion.obtenerConexion()) {
            if (conn == null) return false;

            try (PreparedStatement stmt = conn.prepareStatement(sql)) {
                stmt.setString(1, user);
                stmt.setString(2, pass);
                try (ResultSet rs = stmt.executeQuery()) {
                    return rs.next();
                }
            }
        } catch (SQLException e) {
            System.out.println("Error al autenticar: " + e.getMessage());
            return false;
        }
    }

    /**
     * METODO ESTATICO - Registra un nuevo usuario en la base de datos.
     * Verifica que el nombre de usuario no exista antes de insertarlo.
     * Usa try-with-resources para cerrar todos los recursos.
     */
    public static boolean registrar(String user, String pass) {
        try (Connection conn = Conexion.obtenerConexion()) {
            if (conn == null) return false;

            // Verificar que no exista
            try (PreparedStatement check = conn.prepareStatement(
                    "SELECT id FROM usuarios WHERE username = ?")) {
                check.setString(1, user);
                try (ResultSet rs = check.executeQuery()) {
                    if (rs.next()) return false;
                }
            }

            // Insertar el nuevo usuario
            try (PreparedStatement insert = conn.prepareStatement(
                    "INSERT INTO usuarios (username, password) VALUES (?, ?)")) {
                insert.setString(1, user);
                insert.setString(2, pass);
                insert.executeUpdate();
                return true;
            }
        } catch (SQLException e) {
            System.out.println("Error al registrar: " + e.getMessage());
            return false;
        }
    }
}
