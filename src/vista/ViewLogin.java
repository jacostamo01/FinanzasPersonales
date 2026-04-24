package vista;

import javax.swing.*;
import java.awt.*;
import modelo.Usuario;

/**
 * Ventana de inicio de sesion.
 * El usuario ingresa sus credenciales y se validan contra la BD.
 * Tambien permite registrar nuevos usuarios.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame (ventana de Swing)
 * - COMPOSICION: crea un objeto Usuario para autenticar
 * - ENCAPSULAMIENTO: los componentes son privados
 * - METODO ESTATICO: llama a Usuario.registrar() sin crear objeto
 */
public class ViewLogin extends JFrame {

    // Componentes de la interfaz (privados - ENCAPSULAMIENTO)
    private JTextField txtUsuario;
    private JPasswordField txtPassword;

    // Constructor - configura la ventana al crear el objeto
    public ViewLogin() {
        EstiloApp.configurarVentana(this, "Finanzas Personales - Login", 400, 320);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLayout(new BorderLayout());

        add(EstiloApp.crearHeader("FINANZAS PERSONALES"), BorderLayout.NORTH);
        add(crearFormulario(), BorderLayout.CENTER);
        add(crearBotones(), BorderLayout.SOUTH);
    }

    // Crea el formulario con campos de usuario y contrase\u00f1a
    private JPanel crearFormulario() {
        JPanel panel = new JPanel(new GridBagLayout());
        panel.setBackground(EstiloApp.FONDO);
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(8, 10, 8, 10);
        gbc.fill = GridBagConstraints.HORIZONTAL;

        JLabel lblUser = EstiloApp.crearLabel("Usuario:");
        gbc.gridx = 0; gbc.gridy = 0;
        panel.add(lblUser, gbc);

        txtUsuario = new JTextField(15);
        EstiloApp.aplicarEstiloCampo(txtUsuario);
        gbc.gridx = 1;
        panel.add(txtUsuario, gbc);

        JLabel lblPass = EstiloApp.crearLabel("Contrasena:");
        gbc.gridx = 0; gbc.gridy = 1;
        panel.add(lblPass, gbc);

        txtPassword = new JPasswordField(15);
        EstiloApp.aplicarEstiloCampo(txtPassword);
        gbc.gridx = 1;
        panel.add(txtPassword, gbc);

        return panel;
    }

    // Crea los botones de login y registro
    private JPanel crearBotones() {
        JPanel panel = new JPanel(new FlowLayout(FlowLayout.CENTER, 15, 15));
        panel.setBackground(EstiloApp.FONDO);

        JButton btnLogin = EstiloApp.crearBoton("Iniciar Sesion", EstiloApp.PRIMARIO);
        btnLogin.addActionListener(e -> iniciarSesion());

        JButton btnRegistrar = EstiloApp.crearBoton("Crear Cuenta", EstiloApp.EXITO);
        btnRegistrar.addActionListener(e -> registrarUsuario());

        panel.add(btnLogin);
        panel.add(btnRegistrar);
        return panel;
    }

    // Valida credenciales contra la BD y abre el menu principal
    private void iniciarSesion() {
        String user = txtUsuario.getText().trim();
        String pass = new String(txtPassword.getPassword());

        if (user.isEmpty() || pass.isEmpty()) {
            JOptionPane.showMessageDialog(this,
                "Por favor ingrese usuario y contrase\u00f1a",
                "Campos vacios", JOptionPane.WARNING_MESSAGE);
            return;
        }

        // METODO ESTATICO: no necesita crear objeto Usuario
        if (Usuario.autenticar(user, pass)) {
            dispose();
            new ViewMenuPrincipal(user).setVisible(true);
        } else {
            JOptionPane.showMessageDialog(this,
                "Usuario o contrase\u00f1a incorrectos",
                "Error de acceso", JOptionPane.ERROR_MESSAGE);
        }
    }

    // Registra un nuevo usuario usando los campos del formulario
    private void registrarUsuario() {
        String user = txtUsuario.getText().trim();
        String pass = new String(txtPassword.getPassword());

        if (user.isEmpty() || pass.isEmpty()) {
            JOptionPane.showMessageDialog(this,
                "Ingrese usuario y contrase\u00f1a.",
                "Campos vacios", JOptionPane.WARNING_MESSAGE);
            return;
        }

        // METODO ESTATICO: no necesita crear un objeto Usuario
        if (Usuario.registrar(user, pass)) {
            JOptionPane.showMessageDialog(this,
                "Usuario creado exitosamente. Ya puede iniciar sesion.",
                "Registro exitoso", JOptionPane.INFORMATION_MESSAGE);
        } else {
            JOptionPane.showMessageDialog(this,
                "El usuario ya existe o hay error de conexion.",
                "Error", JOptionPane.ERROR_MESSAGE);
        }
    }
}
