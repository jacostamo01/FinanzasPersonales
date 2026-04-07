package vista;

import javax.swing.*;
import java.awt.*;
import controlador.EstadisticaController;

/**
 * Ventana del modulo de estadisticas financieras.
 * Muestra totales, balance, porcentajes y promedios desde la BD.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: usa EstadisticaController para obtener los datos
 * - SETTER (setControlador): permite inyectar el controlador desde afuera
 */
public class EstadisticaView extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private EstadisticaController controlador;
    private JTextArea txtResultados;
    private JButton btnCalcular;

    // Constructor - configura la ventana y sus componentes
    public EstadisticaView() {
        this.controlador = new EstadisticaController();
        configurarVentana();
        crearComponentes();
    }

    private void configurarVentana() {
        setTitle("Estadisticas Financieras");
        setSize(420, 400);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
    }

    private void crearComponentes() {
        // Panel superior con titulo
        JPanel jpTitulo = new JPanel();
        jpTitulo.setBackground(new Color(33, 97, 140));
        JLabel lbTitulo = new JLabel("ESTADISTICAS FINANCIERAS");
        lbTitulo.setForeground(Color.WHITE);
        lbTitulo.setFont(new Font("Segoe UI", Font.BOLD, 18));
        jpTitulo.add(lbTitulo);
        add(jpTitulo, BorderLayout.NORTH);

        // Panel central con boton y resultados
        JPanel panelCentro = new JPanel();
        panelCentro.setLayout(new BoxLayout(panelCentro, BoxLayout.Y_AXIS));
        panelCentro.setBackground(Color.WHITE);

        // Boton para calcular (jala datos de la BD al presionar)
        btnCalcular = new JButton("Calcular Estadisticas");
        btnCalcular.setBackground(new Color(52, 152, 219));
        btnCalcular.setForeground(Color.WHITE);
        btnCalcular.setFont(new Font("Segoe UI", Font.BOLD, 14));
        btnCalcular.setAlignmentX(CENTER_ALIGNMENT);
        btnCalcular.addActionListener(e -> calcular());

        // Panel de resultados
        JPanel jpResultado = new JPanel(new BorderLayout());
        jpResultado.setBorder(BorderFactory.createTitledBorder("RESULTADOS"));
        jpResultado.setBackground(new Color(245, 245, 245));

        txtResultados = new JTextArea();
        txtResultados.setFont(new Font("Monospaced", Font.PLAIN, 13));
        txtResultados.setEditable(false);
        txtResultados.setBackground(new Color(248, 249, 250));
        txtResultados.setBorder(BorderFactory.createEmptyBorder(10, 10, 10, 10));
        JScrollPane scroll = new JScrollPane(txtResultados);
        jpResultado.add(scroll, BorderLayout.CENTER);

        panelCentro.add(Box.createVerticalStrut(20));
        panelCentro.add(btnCalcular);
        panelCentro.add(Box.createVerticalStrut(15));
        panelCentro.add(jpResultado);

        add(panelCentro, BorderLayout.CENTER);
    }

    // Calcula las estadisticas con datos reales de la BD
    private void calcular() {
        double ingresos = controlador.getTotalIngresos();
        double gastos = controlador.getTotalGastos();
        double balance = controlador.balance(ingresos, gastos);
        double porcentaje = controlador.porcentaje(gastos, ingresos);
        int totalMovimientos = controlador.contarMovimientos();
        double promedioMovimientos = controlador.promedio(ingresos + gastos, totalMovimientos);

        // Muestra los resultados formateados
        txtResultados.setText("");
        txtResultados.append("=== DATOS DESDE LA BASE DE DATOS ===\n\n");
        txtResultados.append("Total Ingresos:       $" + String.format("%,.2f", ingresos) + "\n");
        txtResultados.append("Total Gastos:         $" + String.format("%,.2f", gastos) + "\n");
        txtResultados.append("Balance:              $" + String.format("%,.2f", balance) + "\n");
        txtResultados.append("% Gastos/Ingresos:    " + String.format("%.2f", porcentaje) + " %\n");
        txtResultados.append("Total Movimientos:    " + totalMovimientos + "\n");
        txtResultados.append("Promedio por Movim.:  $" + String.format("%,.2f", promedioMovimientos) + "\n");

        if (balance >= 0) {
            txtResultados.append("\nEstado: POSITIVO - Estas ahorrando");
        } else {
            txtResultados.append("\nEstado: NEGATIVO - Gastas mas de lo que ganas");
        }
    }

    // SETTER: permite asignar un controlador desde afuera
    public void setControlador(EstadisticaController controlador) {
        this.controlador = controlador;
    }
}