package vista;

import controlador.MovimientoController;
import modelo.Ahorro;
import javax.swing.*;
import javax.swing.border.EmptyBorder;
import java.awt.*;

/**
 * Menu principal del sistema.
 * Desde aqui se accede a los 5 modulos: Movimientos, Ahorros, Inversiones,
 * Estadisticas e Historico Mensual.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: tiene controladores y un objeto Ahorro
 * - ENCAPSULAMIENTO: los atributos son privados
 * - REUTILIZACION: usa EstiloApp para crear botones con estilo unificado
 */
public class ViewMenuPrincipal extends JFrame {

    // Colores sobrios exclusivos del menu (tonos oscuros y neutros)
    private static final Color FONDO_MENU = new Color(30, 30, 30);
    private static final Color PANEL_LATERAL = new Color(38, 38, 38);
    private static final Color TEXTO_SECUNDARIO = new Color(160, 160, 160);
    private static final Color BORDE_TARJETA = new Color(55, 55, 55);
    private static final Color HOVER_CARD = new Color(48, 48, 48);

    // Colores de los iconos de cada modulo (sobrios, desaturados)
    private static final Color AZUL_ACERO = new Color(70, 130, 180);
    private static final Color VERDE_OLIVA = new Color(85, 140, 95);
    private static final Color GRIS_AZULADO = new Color(95, 120, 140);
    private static final Color INDIGO = new Color(100, 90, 150);
    private static final Color BRONCE = new Color(160, 130, 80);
    private static final Color ROJO_APAGADO = new Color(160, 70, 70);

    // Atributos privados (ENCAPSULAMIENTO)
    private String usuarioActual;
    private MovimientoController movimientoController;
    private Ahorro ahorro;

    // Constructor - recibe el nombre del usuario que inicio sesion
    public ViewMenuPrincipal(String usuario) {
        this.usuarioActual = usuario;
        this.movimientoController = new MovimientoController();
        this.ahorro = new Ahorro();

        setTitle("Finanzas Personales");
        setSize(520, 580);
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        getContentPane().setBackground(FONDO_MENU);
        setLayout(new BorderLayout());

        add(crearHeaderProfesional(), BorderLayout.NORTH);
        add(crearPanelModulos(), BorderLayout.CENTER);
        add(crearPiePagina(), BorderLayout.SOUTH);
    }

    // Header elegante con linea decorativa
    private JPanel crearHeaderProfesional() {
        JPanel header = new JPanel();
        header.setLayout(new BoxLayout(header, BoxLayout.Y_AXIS));
        header.setBackground(FONDO_MENU);
        header.setBorder(new EmptyBorder(25, 30, 15, 30));

        JLabel lblTitulo = new JLabel("Finanzas Personales");
        lblTitulo.setFont(new Font("Segoe UI Light", Font.PLAIN, 26));
        lblTitulo.setForeground(Color.WHITE);
        lblTitulo.setAlignmentX(Component.LEFT_ALIGNMENT);

        JLabel lblUser = new JLabel("Sesion: " + usuarioActual);
        lblUser.setFont(new Font("Segoe UI", Font.PLAIN, 13));
        lblUser.setForeground(TEXTO_SECUNDARIO);
        lblUser.setAlignmentX(Component.LEFT_ALIGNMENT);

        // Linea decorativa sutil
        JPanel linea = new JPanel();
        linea.setMaximumSize(new Dimension(Integer.MAX_VALUE, 1));
        linea.setBackground(BORDE_TARJETA);
        linea.setAlignmentX(Component.LEFT_ALIGNMENT);

        header.add(lblTitulo);
        header.add(Box.createVerticalStrut(4));
        header.add(lblUser);
        header.add(Box.createVerticalStrut(12));
        header.add(linea);

        return header;
    }

    // Panel central con tarjetas de modulos
    private JPanel crearPanelModulos() {
        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(FONDO_MENU);
        panel.setBorder(new EmptyBorder(8, 25, 8, 25));

        panel.add(crearTarjetaModulo("Ingresos y Gastos", "Registrar movimientos financieros",
                AZUL_ACERO, e -> {
                    setVisible(false);
                    new ViewMovimientos(usuarioActual, this, movimientoController).setVisible(true);
                }));
        panel.add(Box.createVerticalStrut(6));

        panel.add(crearTarjetaModulo("Ahorros", "Depositar y retirar fondos de ahorro",
                VERDE_OLIVA, e -> new AhorroView(ahorro).setVisible(true)));
        panel.add(Box.createVerticalStrut(6));

        panel.add(crearTarjetaModulo("Inversiones", "Simular rendimiento de inversiones",
                GRIS_AZULADO, e -> new InversionView(ahorro).setVisible(true)));
        panel.add(Box.createVerticalStrut(6));

        panel.add(crearTarjetaModulo("Estadisticas", "Reportes y graficas financieras",
                INDIGO, e -> new EstadisticaView().setVisible(true)));
        panel.add(Box.createVerticalStrut(6));

        panel.add(crearTarjetaModulo("Historico Mensual", "Consulta de meses anteriores (solo lectura)",
                BRONCE, e -> new ViewHistorico(movimientoController).setVisible(true)));

        return panel;
    }

    /**
     * Crea una tarjeta de modulo con titulo, descripcion e indicador de color.
     * Cada tarjeta es un boton completo con estilo de card oscuro.
     */
    private JPanel crearTarjetaModulo(String titulo, String descripcion,
                                       Color colorIndicador, java.awt.event.ActionListener accion) {
        JPanel tarjeta = new JPanel(new BorderLayout());
        tarjeta.setBackground(PANEL_LATERAL);
        tarjeta.setMaximumSize(new Dimension(Integer.MAX_VALUE, 65));
        tarjeta.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(BORDE_TARJETA, 1),
                new EmptyBorder(10, 14, 10, 14)));
        tarjeta.setCursor(new Cursor(Cursor.HAND_CURSOR));

        // Indicador de color a la izquierda
        JPanel indicador = new JPanel();
        indicador.setPreferredSize(new Dimension(4, 0));
        indicador.setBackground(colorIndicador);

        // Textos
        JPanel textos = new JPanel();
        textos.setLayout(new BoxLayout(textos, BoxLayout.Y_AXIS));
        textos.setBackground(PANEL_LATERAL);
        textos.setBorder(new EmptyBorder(0, 10, 0, 0));

        JLabel lblTitulo = new JLabel(titulo);
        lblTitulo.setFont(new Font("Segoe UI Semibold", Font.PLAIN, 14));
        lblTitulo.setForeground(new Color(230, 230, 230));

        JLabel lblDesc = new JLabel(descripcion);
        lblDesc.setFont(new Font("Segoe UI", Font.PLAIN, 11));
        lblDesc.setForeground(TEXTO_SECUNDARIO);

        textos.add(lblTitulo);
        textos.add(Box.createVerticalStrut(2));
        textos.add(lblDesc);

        // Flecha indicadora
        JLabel flecha = new JLabel("\u203A");
        flecha.setFont(new Font("Segoe UI", Font.PLAIN, 22));
        flecha.setForeground(TEXTO_SECUNDARIO);

        tarjeta.add(indicador, BorderLayout.WEST);
        tarjeta.add(textos, BorderLayout.CENTER);
        tarjeta.add(flecha, BorderLayout.EAST);

        // Click en toda la tarjeta
        tarjeta.addMouseListener(new java.awt.event.MouseAdapter() {
            @Override
            public void mouseClicked(java.awt.event.MouseEvent e) {
                accion.actionPerformed(null);
            }
            @Override
            public void mouseEntered(java.awt.event.MouseEvent e) {
                tarjeta.setBackground(HOVER_CARD);
                textos.setBackground(HOVER_CARD);
            }
            @Override
            public void mouseExited(java.awt.event.MouseEvent e) {
                tarjeta.setBackground(PANEL_LATERAL);
                textos.setBackground(PANEL_LATERAL);
            }
        });

        return tarjeta;
    }

    // Pie de pagina con boton de cerrar sesion discreto
    private JPanel crearPiePagina() {
        JPanel panel = new JPanel(new BorderLayout());
        panel.setBackground(FONDO_MENU);
        panel.setBorder(new EmptyBorder(10, 25, 18, 25));

        // Linea separadora
        JPanel linea = new JPanel();
        linea.setMaximumSize(new Dimension(Integer.MAX_VALUE, 1));
        linea.setPreferredSize(new Dimension(0, 1));
        linea.setBackground(BORDE_TARJETA);

        JButton btnCerrar = new JButton("Cerrar sesion");
        btnCerrar.setFont(new Font("Segoe UI", Font.PLAIN, 12));
        btnCerrar.setForeground(ROJO_APAGADO);
        btnCerrar.setBackground(FONDO_MENU);
        btnCerrar.setBorderPainted(false);
        btnCerrar.setFocusPainted(false);
        btnCerrar.setCursor(new Cursor(Cursor.HAND_CURSOR));
        btnCerrar.setHorizontalAlignment(SwingConstants.LEFT);
        btnCerrar.addActionListener(e -> {
            dispose();
            new ViewLogin().setVisible(true);
        });

        panel.add(linea, BorderLayout.NORTH);
        panel.add(btnCerrar, BorderLayout.SOUTH);
        return panel;
    }
}
