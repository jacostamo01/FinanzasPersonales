package vista;

import controlador.MovimientoController;
import modelo.Movimiento;
import javax.swing.*;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.DefaultTableCellRenderer;
import java.awt.*;
import java.util.ArrayList;

/**
 * Vista NO CRUD - Solo lectura.
 * Muestra los ingresos y gastos agrupados por mes desde la base de datos.
 * Permite seleccionar un mes para ver los movimientos detallados.
 * No permite agregar, editar ni eliminar datos.
 * Incluye una grafica de barras para visualizar la informacion.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende JFrame
 * - COMPOSICION: usa MovimientoController y GraficaPanel
 * - POLIMORFISMO: GraficaPanel sobreescribe paintComponent para dibujar
 */
public class ViewHistorico extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private MovimientoController controller;
    private JTable tablaResumen;
    private JTable tablaDetalle;
    private JPanel panelGrafica;
    private JComboBox<String> comboMes;
    private ArrayList<Object[]> datosResumen;

    // Constructor - recibe el controlador existente (COMPOSICION)
    public ViewHistorico(MovimientoController controller) {
        this.controller = controller;
        EstiloApp.configurarVentana(this, "Historico Mensual", 700, 700);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(new BorderLayout());

        add(EstiloApp.crearHeader("HISTORICO MENSUAL"), BorderLayout.NORTH);
        add(crearContenido(), BorderLayout.CENTER);
        cargarDatos();
    }

    private JPanel crearContenido() {
        JPanel panel = new JPanel(new BorderLayout(0, 10));
        panel.setBackground(EstiloApp.FONDO);
        panel.setBorder(BorderFactory.createEmptyBorder(10, 15, 10, 15));

        // --- Panel superior: tabla resumen + grafica ---
        JPanel panelSuperior = new JPanel(new BorderLayout(0, 10));
        panelSuperior.setBackground(EstiloApp.FONDO);
        panelSuperior.setOpaque(true);

        // Tabla de datos mensuales (NO CRUD: solo lectura)
        String[] columnas = {"Periodo", "Ingresos", "Gastos", "Balance"};
        DefaultTableModel modelo = new DefaultTableModel(columnas, 0) {
            @Override
            public boolean isCellEditable(int row, int column) {
                return false; // NO CRUD: tabla solo lectura
            }
        };

        tablaResumen = new JTable(modelo);
        EstiloApp.aplicarEstiloTabla(tablaResumen);

        DefaultTableCellRenderer centrar = new DefaultTableCellRenderer();
        centrar.setHorizontalAlignment(SwingConstants.CENTER);
        for (int i = 0; i < columnas.length; i++) {
            tablaResumen.getColumnModel().getColumn(i).setCellRenderer(centrar);
        }

        JScrollPane scrollResumen = new JScrollPane(tablaResumen);
        scrollResumen.setPreferredSize(new Dimension(650, 130));

        panelGrafica = new JPanel(new BorderLayout());
        panelGrafica.setBackground(EstiloApp.TARJETA);
        panelGrafica.setPreferredSize(new Dimension(650, 200));
        panelGrafica.setBorder(BorderFactory.createTitledBorder(
                BorderFactory.createLineBorder(EstiloApp.BORDE),
                "Grafica Ingresos vs Gastos",
                0, 0, EstiloApp.FUENTE_BOTON, EstiloApp.TEXTO_SEC));

        panelSuperior.add(scrollResumen, BorderLayout.NORTH);
        panelSuperior.add(panelGrafica, BorderLayout.CENTER);

        // --- Panel inferior: selector de mes + detalle ---
        JPanel panelInferior = new JPanel(new BorderLayout(0, 5));
        panelInferior.setBackground(EstiloApp.FONDO);

        // Selector de mes
        JPanel panelSelector = new JPanel(new FlowLayout(FlowLayout.LEFT, 10, 5));
        panelSelector.setBackground(EstiloApp.FONDO);
        JLabel lblMes = new JLabel("Ver detalle del mes:");
        lblMes.setFont(EstiloApp.FUENTE_BOTON);
        lblMes.setForeground(EstiloApp.TEXTO);
        comboMes = new JComboBox<>();
        comboMes.setFont(EstiloApp.FUENTE_CUERPO);
        comboMes.setBackground(EstiloApp.CAMPO);
        comboMes.setForeground(EstiloApp.TEXTO);
        comboMes.addActionListener(e -> cargarDetalleMes());
        panelSelector.add(lblMes);
        panelSelector.add(comboMes);
        panelInferior.add(panelSelector, BorderLayout.NORTH);

        // Tabla de detalle por mes
        String[] colDetalle = {"Tipo", "Monto", "Descripcion"};
        DefaultTableModel modeloDetalle = new DefaultTableModel(colDetalle, 0) {
            @Override
            public boolean isCellEditable(int row, int column) {
                return false;
            }
        };
        tablaDetalle = new JTable(modeloDetalle);
        EstiloApp.aplicarEstiloTabla(tablaDetalle);
        JScrollPane scrollDetalle = new JScrollPane(tablaDetalle);
        panelInferior.add(scrollDetalle, BorderLayout.CENTER);

        // Splitter vertical
        JSplitPane split = new JSplitPane(JSplitPane.VERTICAL_SPLIT, panelSuperior, panelInferior);
        split.setDividerLocation(350);
        split.setResizeWeight(0.5);
        panel.add(split, BorderLayout.CENTER);

        return panel;
    }

    // Consulta la BD y llena la tabla resumen, la grafica y el combo de meses
    private void cargarDatos() {
        datosResumen = controller.listarPorMes();
        DefaultTableModel modelo = (DefaultTableModel) tablaResumen.getModel();
        modelo.setRowCount(0);

        comboMes.removeAllItems();

        int limite = Math.min(datosResumen.size(), 6);
        String[] etiquetas = new String[limite];
        double[] ingresosArr = new double[limite];
        double[] gastosArr = new double[limite];

        for (Object[] fila : datosResumen) {
            String periodo = (String) fila[0];
            double ingresos = (Double) fila[1];
            double gastos = (Double) fila[2];
            double balance = ingresos - gastos;

            modelo.addRow(new Object[]{
                periodo,
                "$" + String.format("%,.0f", ingresos),
                "$" + String.format("%,.0f", gastos),
                "$" + String.format("%,.0f", balance)
            });

            comboMes.addItem(periodo);
        }

        for (int i = 0; i < limite; i++) {
            Object[] fila = datosResumen.get(limite - 1 - i);
            etiquetas[i] = (String) fila[0];
            ingresosArr[i] = (Double) fila[1];
            gastosArr[i] = (Double) fila[2];
        }

        panelGrafica.removeAll();
        panelGrafica.add(new GraficaPanel(etiquetas, ingresosArr, gastosArr),
                BorderLayout.CENTER);
        panelGrafica.revalidate();
        panelGrafica.repaint();
    }

    // Carga los movimientos detallados del mes seleccionado en el combo
    private void cargarDetalleMes() {
        String mesSeleccionado = (String) comboMes.getSelectedItem();
        if (mesSeleccionado == null) return;

        ArrayList<Movimiento> movimientos = controller.listarMovimientosPorMes(mesSeleccionado);
        DefaultTableModel modelo = (DefaultTableModel) tablaDetalle.getModel();
        modelo.setRowCount(0);

        for (Movimiento m : movimientos) {
            modelo.addRow(new Object[]{
                m.getTipo(),
                "$" + String.format("%,.0f", m.getMonto()),
                m.getDescripcion()
            });
        }
    }
}
