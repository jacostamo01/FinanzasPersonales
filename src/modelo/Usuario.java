package modelo;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import conexion.Conexion;

/**
 * autenticacion de usuario y contr
 */
public class Usuario {

    private String username;
    private String password;

    // constructor para usuario y cntraseña
    public Usuario(String username, String password) {
        this.username = username;
        this.password = password;
    }

    /**
     * aqui verificamos si el usuario y contraseña son correctos o noVerifica si el usuario y contraseña ingresados son correctos
     */
    public boolean autenticar(String user, String pass) {
        String sql = "SELECT * FROM usuarios WHERE username = ? AND password = ?";

        try (Connection conn = Conexion.obtenerConexion();
             PreparedStatement stmt = conn.prepareStatement(sql)) {

            stmt.setString(1, user);
            stmt.setString(2, pass);

            ResultSet resultado = stmt.executeQuery();
            return resultado.next(); 

        } catch (SQLException e) {
            System.out.println("Error al autenticar: " + e.getMessage());
            return false;
        }
    }

    // Getters y Setters

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
