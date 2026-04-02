package vista;

import javax.swing.*;
import java.awt.Color;
import modelo.Usuario;

/**
 * Ventana de inicio de sesion
 */
public class ViewLogin extends JFrame {

    // Componentes de la interfaz
    private JTextField txtUsuario;
    private JPasswordField txtPassword;
    private JButton btnLogin;

    public ViewLogin() {
        configurarVentana();
        crearComponentes();
        agregarEventos();
    }

    private void configurarVentana() {
        setTitle("Acceso al Sistema");
        setSize(350, 250);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLayout(null);
        setLocationRelativeTo(null);
    }

    private void crearComponentes() {
        // Campo de usuario
        JLabel lblUser = new JLabel("Usuario:");
        lblUser.setBounds(40, 40, 80, 25);
        add(lblUser);

        txtUsuario = new JTextField();
        txtUsuario.setBounds(130, 40, 150, 25);
        add(txtUsuario);

        // Campo de contraseña
        JLabel lblPass = new JLabel("Contraseña:");
        lblPass.setBounds(40, 80, 80, 25);
        add(lblPass);

        txtPassword = new JPasswordField();
        txtPassword.setBounds(130, 80, 150, 25);
        add(txtPassword);

        // Botón de ingreso
        btnLogin = new JButton("Entrar");
        btnLogin.setBounds(100, 140, 130, 35);
        btnLogin.setBackground(new Color(50, 150, 250));
        btnLogin.setForeground(Color.WHITE);
        add(btnLogin);
    }

    // lo que pasa cuando se da click en entrar
    private void agregarEventos() {
        btnLogin.addActionListener(e -> {
            String user = txtUsuario.getText().trim();
            String pass = new String(txtPassword.getPassword());

            // validacion de campos vacios
            if (user.isEmpty() || pass.isEmpty()) {
                JOptionPane.showMessageDialog(this,
                    "Por favor ingrese usuario y contraseña",
                    "Campos vacíos", JOptionPane.WARNING_MESSAGE);
                return;
            }

            // usuario quemado
            Usuario admin = new Usuario("admin", "1234");

            if (admin.autenticar(user, pass)) {
                dispose(); // Cierra la ventana de login
                new ViewMenuPrincipal(user).setVisible(true);
            } else {
                JOptionPane.showMessageDialog(this,
                    "Usuario o contraseña incorrectos",
                    "Error de acceso", JOptionPane.ERROR_MESSAGE);
            }
        });
    }
}
