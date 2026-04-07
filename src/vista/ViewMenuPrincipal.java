package vista;

import controlador.MovimientoController;
import javax.swing.*;

import Controller.EstadisticaController;

import java.awt.*;

/**
 * Ventana principal
 * ingresos y gastos
 * ahorros e inversiones
 * estadísticas y reportes
 */
public class ViewMenuPrincipal extends JFrame {
    private String usuarioActual;
    private EstadisticaController EstadisticaController;
    private MovimientoController movimientoController;

    public ViewMenuPrincipal(String usuario) {
        this.usuarioActual = usuario;
        this.EstadisticaController = new EstadisticaController();
        this.movimientoController = new MovimientoController();
        configurarVentana();
        crearPanelBienvenida(usuario);
        crearBotonesModulos();
        crearBotonCerrarSesion();
    }

    private void configurarVentana() {
        setTitle("Finanzas Personales - Menú Principal");
        setSize(600, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout());
    }

    // mensaje de bienvenida
    private void crearPanelBienvenida(String usuario) {
        JLabel lblBienvenida = new JLabel("Bienvenido, " + usuario, SwingConstants.CENTER);
        lblBienvenida.setFont(new Font("Arial", Font.BOLD, 18));
        lblBienvenida.setBorder(BorderFactory.createEmptyBorder(15, 0, 15, 0));
        add(lblBienvenida, BorderLayout.NORTH);
    }

    // botones para entrar a cada modulo
    private void crearBotonesModulos() {
        JPanel panelCentral = new JPanel(new GridLayout(3, 1, 10, 10));
        panelCentral.setBorder(BorderFactory.createEmptyBorder(20, 40, 20, 40));

        // ingresos y gastos
        JButton btnMovimientos = crearBoton(
            "Ingresos y Gastos (Movimientos)",
            new Color(50, 150, 250)
        );
        btnMovimientos.addActionListener(e -> {
            this.setVisible(false); // oculta el menu
            new ViewMovimientos(usuarioActual, this, movimientoController).setVisible(true);
        });
        panelCentral.add(btnMovimientos);

        //ahorros e inversiones
        JButton btnAhorros = crearBoton(
            "Ahorros e Inversiones",
            new Color(46, 204, 113)
        );
        btnAhorros.addActionListener(e -> {
            JOptionPane.showMessageDialog(this,
                "Aquí irá el módulo de Ahorros e Inversiones",
                "Módulo Ahorros", JOptionPane.INFORMATION_MESSAGE);
        });
        panelCentral.add(btnAhorros);

        // estadistica
        JButton btnEstadisticas = crearBoton(
            "Estadísticas y Reportes",
            new Color(155, 89, 182)
        );
        btnEstadisticas.addActionListener(e -> {
          EstadisticaView view = new EstadisticaView();
          view.setVisible(true);
        });
        panelCentral.add(btnEstadisticas);
        add(panelCentral, BorderLayout.CENTER);
    }

    

    // boton cerrar sesion
    private void crearBotonCerrarSesion() {
        JButton btnCerrarSesion = new JButton("Cerrar Sesión");
        btnCerrarSesion.setBackground(new Color(231, 76, 60));
        btnCerrarSesion.setForeground (Color.WHITE);
        btnCerrarSesion.addActionListener(e -> {
            
            dispose(); // Cierra el menú
            new ViewLogin().setVisible(true);
        });

        JPanel panelInferior = new JPanel();
        panelInferior.setBorder(BorderFactory.createEmptyBorder(5, 0, 10, 0));
        panelInferior.add(btnCerrarSesion);
        add(panelInferior, BorderLayout.SOUTH);
    }

    /**
     * crear botones con el mismo estilo.
     */
    private JButton crearBoton(String texto, Color colorFondo) {
        JButton boton = new JButton(texto);
        boton.setFont(new Font("Arial", Font.PLAIN, 14));
        boton.setBackground(colorFondo);
        boton.setForeground(Color.WHITE);
        return boton;
    }
}
