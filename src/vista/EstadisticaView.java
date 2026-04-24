package vista;

import javax.swing.*;
import java.awt.*;
import controlador.EstadisticaController;

/**
 * Ventana del modulo de estadisticas financieras.
 * Muestra totales, balance, porcentajes y promedios desde la BD.
 * Incluye una grafica de barras comparando ingresos vs gastos.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: usa EstadisticaController y GraficaPanel
 * - SETTER (setControlador): permite inyectar el controlador desde afuera
 * - REUTILIZACION: usa EstiloApp para estilos y GraficaPanel para graficar
 */
public class EstadisticaView extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private EstadisticaController controlador;
    private JTextArea txtResultados;
    private JPanel panelGrafica;

    // Constructor - configura la ventana y sus componentes
    public EstadisticaView() {
        this.controlador = new EstadisticaController();

        EstiloApp.configurarVentana(this, "Estadisticas Financieras", 500, 560);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(new BorderLayout());

        add(EstiloApp.crearHeader("ESTADISTICAS FINANCIERAS"), BorderLayout.NORTH);
        add(crearContenido(), BorderLayout.CENTER);
    }

    private JPanel crearContenido() {
        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(EstiloApp.FONDO);
        panel.setBorder(BorderFactory.createEmptyBorder(10, 15, 10, 15));

        // Boton calcular
        JPanel panelBtn = new JPanel(new FlowLayout(FlowLayout.CENTER));
        panelBtn.setBackground(EstiloApp.FONDO);
        JButton btnCalcular = EstiloApp.crearBoton("Calcular Estadisticas", EstiloApp.MORADO);
        btnCalcular.addActionListener(e -> calcular());
        panelBtn.add(btnCalcular);
        panel.add(panelBtn);
        panel.add(Box.createVerticalStrut(8));

        // Resultados en texto
        txtResultados = new JTextArea(8, 30);
        txtResultados.setEditable(false);
        EstiloApp.aplicarEstiloArea(txtResultados);
        JScrollPane scroll = new JScrollPane(txtResultados);
        scroll.setBorder(BorderFactory.createLineBorder(EstiloApp.BORDE));
        panel.add(scroll);
        panel.add(Box.createVerticalStrut(8));

        // Panel para la grafica (REUTILIZACION de GraficaPanel)
        panelGrafica = new JPanel(new BorderLayout());
        panelGrafica.setBackground(EstiloApp.TARJETA);
        panelGrafica.setBorder(BorderFactory.createTitledBorder(
            BorderFactory.createLineBorder(EstiloApp.BORDE),
            "Grafica Ingresos vs Gastos",
            0, 0, EstiloApp.FUENTE_BOTON, EstiloApp.TEXTO_SEC));
        panelGrafica.setPreferredSize(new Dimension(450, 200));
        panel.add(panelGrafica);

        return panel;
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

        // Actualizar la grafica con los totales
        panelGrafica.removeAll();
        panelGrafica.add(new GraficaPanel(
            new String[]{"Resumen Total"},
            new double[]{ingresos},
            new double[]{gastos}
        ), BorderLayout.CENTER);
        panelGrafica.revalidate();
        panelGrafica.repaint();
    }
}