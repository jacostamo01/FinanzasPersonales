package vista;

import controlador.MovimientoController;
import controlador.EstadisticaController;
import modelo.Ahorro;
import javax.swing.*;
import java.awt.*;

/**
 * Menu principal del sistema.
 * Desde aqui se accede a los 4 modulos: Movimientos, Ahorros, Inversiones, Estadisticas.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: tiene controladores y un objeto Ahorro
 * - ENCAPSULAMIENTO: los atributos son privados
 */
public class ViewMenuPrincipal extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private String usuarioActual;
    private EstadisticaController estadisticaCtrl;
    private MovimientoController movimientoController;
    private Ahorro ahorro; // Se comparte entre Ahorros e Inversiones

    // Constructor - recibe el nombre del usuario que inicio sesion
    public ViewMenuPrincipal(String usuario) {
        this.usuarioActual = usuario;
        this.estadisticaCtrl = new EstadisticaController();
        this.movimientoController = new MovimientoController();
        this.ahorro = new Ahorro();
        configurarVentana();
        crearPanelBienvenida();
        crearBotonesModulos();
        crearBotonCerrarSesion();
    }

    private void configurarVentana() {
        setTitle("Finanzas Personales - Menu Principal");
        setSize(600, 400);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout());
    }

    // Mensaje de bienvenida en la parte superior
    private void crearPanelBienvenida() {
        JLabel lblBienvenida = new JLabel("Bienvenido, " + usuarioActual, SwingConstants.CENTER);
        lblBienvenida.setFont(new Font("Arial", Font.BOLD, 18));
        lblBienvenida.setBorder(BorderFactory.createEmptyBorder(15, 0, 15, 0));
        add(lblBienvenida, BorderLayout.NORTH);
    }

    // Crea los 4 botones para acceder a cada modulo
    private void crearBotonesModulos() {
        JPanel panelCentral = new JPanel(new GridLayout(4, 1, 10, 10));
        panelCentral.setBorder(BorderFactory.createEmptyBorder(20, 40, 20, 40));

        // Boton: Ingresos y Gastos
        JButton btnMovimientos = crearBoton("Ingresos y Gastos (Movimientos)", new Color(50, 150, 250));
        btnMovimientos.addActionListener(e -> {
            this.setVisible(false);
            new ViewMovimientos(usuarioActual, this, movimientoController).setVisible(true);
        });
        panelCentral.add(btnMovimientos);

        // Boton: Ahorros (pasa el objeto ahorro compartido)
        JButton btnAhorros = crearBoton("Ahorros", new Color(46, 204, 113));
        btnAhorros.addActionListener(e -> {
            new AhorroView(ahorro).setVisible(true);
        });
        panelCentral.add(btnAhorros);

        // Boton: Inversiones (tambien recibe el ahorro compartido)
        JButton btnInversiones = crearBoton("Inversiones", new Color(26, 188, 156));
        btnInversiones.addActionListener(e -> {
            new InversionView(ahorro).setVisible(true);
        });
        panelCentral.add(btnInversiones);

        // Boton: Estadisticas
        JButton btnEstadisticas = crearBoton("Estadisticas y Reportes", new Color(155, 89, 182));
        btnEstadisticas.addActionListener(e -> {
            EstadisticaView view = new EstadisticaView();
            view.setControlador(estadisticaCtrl);
            view.setVisible(true);
        });
        panelCentral.add(btnEstadisticas);

        add(panelCentral, BorderLayout.CENTER);
    }

    // Boton para cerrar sesion y volver al login
    private void crearBotonCerrarSesion() {
        JButton btnCerrarSesion = new JButton("Cerrar Sesion");
        btnCerrarSesion.setBackground(new Color(231, 76, 60));
        btnCerrarSesion.setForeground(Color.WHITE);
        btnCerrarSesion.addActionListener(e -> {
            dispose();
            new ViewLogin().setVisible(true);
        });

        JPanel panelInferior = new JPanel();
        panelInferior.setBorder(BorderFactory.createEmptyBorder(5, 0, 10, 0));
        panelInferior.add(btnCerrarSesion);
        add(panelInferior, BorderLayout.SOUTH);
    }

    /**
     * Metodo auxiliar para crear botones con el mismo estilo.
     * Evita repetir codigo (principio DRY: Don't Repeat Yourself).
     */
    private JButton crearBoton(String texto, Color colorFondo) {
        JButton boton = new JButton(texto);
        boton.setFont(new Font("Arial", Font.PLAIN, 14));
        boton.setBackground(colorFondo);
        boton.setForeground(Color.WHITE);
        return boton;
    }
}
