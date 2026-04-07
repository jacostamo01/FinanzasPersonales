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
 * - ENCAPSULAMIENTO: atributos privados con getters
 * - RESPONSABILIDAD: esta clase se encarga de autenticar al usuario contra la BD
 */
public class Usuario {

    // Atributos privados (ENCAPSULAMIENTO)
    private String username;
    private String password;

    // Constructor - crea un usuario con nombre y contraseña
    public Usuario(String username, String password) {
        this.username = username;
        this.password = password;
    }

    /**
     * Verifica si el usuario y contraseña existen en la base de datos.
     * Usa PreparedStatement para evitar inyeccion SQL.
     * Retorna true si las credenciales son correctas, false si no.
     */
    public boolean autenticar(String user, String pass) {
        String sql = "SELECT * FROM usuarios WHERE username = ? AND password = ?";
        Connection conn = Conexion.obtenerConexion();

        if (conn == null) {
            System.out.println("No se pudo conectar a la base de datos.");
            return false;
        }

        try (PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, user);
            stmt.setString(2, pass);

            ResultSet resultado = stmt.executeQuery();
            return resultado.next();
        } catch (SQLException e) {
            System.out.println("Error al autenticar: " + e.getMessage());
            return false;
        }
    }
}
