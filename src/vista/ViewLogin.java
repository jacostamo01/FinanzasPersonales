package vista;

import javax.swing.*;
import java.awt.Color;
import modelo.Usuario;

/**
 * Ventana de inicio de sesion.
 * El usuario ingresa sus credenciales y se validan contra la BD.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame (ventana de Swing)
 * - COMPOSICION: crea un objeto Usuario para autenticar
 * - ENCAPSULAMIENTO: los componentes son privados
 */
public class ViewLogin extends JFrame {

    // Componentes de la interfaz (privados - ENCAPSULAMIENTO)
    private JTextField txtUsuario;
    private JPasswordField txtPassword;
    private JButton btnLogin;

    // Constructor - configura la ventana al crear el objeto
    public ViewLogin() {
        configurarVentana();
        crearComponentes();
        agregarEventos();
    }

    // Configura las propiedades basicas de la ventana
    private void configurarVentana() {
        setTitle("Acceso al Sistema");
        setSize(350, 250);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLayout(null);
        setLocationRelativeTo(null);
    }

    // Crea y posiciona los elementos visuales
    private void crearComponentes() {
        JLabel lblUser = new JLabel("Usuario:");
        lblUser.setBounds(40, 40, 80, 25);
        add(lblUser);

        txtUsuario = new JTextField();
        txtUsuario.setBounds(130, 40, 150, 25);
        add(txtUsuario);

        JLabel lblPass = new JLabel("Contrase\u00f1a:");
        lblPass.setBounds(40, 80, 80, 25);
        add(lblPass);

        txtPassword = new JPasswordField();
        txtPassword.setBounds(130, 80, 150, 25);
        add(txtPassword);

        btnLogin = new JButton("Entrar");
        btnLogin.setBounds(100, 140, 130, 35);
        btnLogin.setBackground(new Color(50, 150, 250));
        btnLogin.setForeground(Color.WHITE);
        add(btnLogin);
    }

    // Define que pasa cuando se presiona el boton "Entrar"
    private void agregarEventos() {
        btnLogin.addActionListener(e -> {
            String user = txtUsuario.getText().trim();
            String pass = new String(txtPassword.getPassword());

            // Validar que no esten vacios
            if (user.isEmpty() || pass.isEmpty()) {
                JOptionPane.showMessageDialog(this,
                    "Por favor ingrese usuario y contrase\u00f1a",
                    "Campos vac\u00edos", JOptionPane.WARNING_MESSAGE);
                return;
            }

            // Crear objeto Usuario y autenticar contra la BD
            Usuario usuario = new Usuario(user, pass);

            if (usuario.autenticar(user, pass)) {
                dispose();
                new ViewMenuPrincipal(user).setVisible(true);
            } else {
                JOptionPane.showMessageDialog(this,
                    "Usuario o contrase\u00f1a incorrectos",
                    "Error de acceso", JOptionPane.ERROR_MESSAGE);
            }
        });
    }
}
