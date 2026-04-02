package modelo;

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
        return this.username.equals(user) && this.password.equals(pass);
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
