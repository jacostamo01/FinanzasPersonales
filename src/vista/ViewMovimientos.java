package vista;

import controlador.MovimientoController;
import modelo.Movimiento;
import javax.swing.*;
import java.awt.*;
import java.time.LocalDate;
import java.util.ArrayList;

/**
 * Ventana para registrar ingresos y gastos.
 * Permite agregar y ver movimientos de la BD.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: recibe el controlador como parametro (no crea uno nuevo)
 * - POLIMORFISMO: muestra Ingresos y Gastos como Movimiento usando getTipo()
 * - REUTILIZACION: usa EstiloApp para estilos unificados
 */
public class ViewMovimientos extends JFrame {

    // Componentes de la interfaz (ENCAPSULAMIENTO)
    private JTextField txtMonto;
    private JTextField txtDescripcion;
    private JTextField txtFecha;
    private JTextArea txtAreaLista;

    // Referencias a otros objetos (COMPOSICION)
    private MovimientoController controller;
    private ViewMenuPrincipal menuPrincipal;

    // Constructor - recibe el controlador ya existente (no crea uno nuevo)
    public ViewMovimientos(String usuario, ViewMenuPrincipal menu, MovimientoController controller) {
        this.menuPrincipal = menu;
        this.controller = controller;

        EstiloApp.configurarVentana(this, "Ingresos y Gastos", 480, 520);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(new BorderLayout());

        add(EstiloApp.crearHeader("INGRESOS Y GASTOS"), BorderLayout.NORTH);
        add(crearContenido(), BorderLayout.CENTER);
        add(crearPanelInferior(), BorderLayout.SOUTH);
    }

    private JPanel crearContenido() {
        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(EstiloApp.FONDO);
        panel.setBorder(BorderFactory.createEmptyBorder(15, 20, 10, 20));

        // Formulario de campos
        JPanel formulario = new JPanel(new GridBagLayout());
        formulario.setBackground(EstiloApp.FONDO);
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(5, 5, 5, 5);
        gbc.fill = GridBagConstraints.HORIZONTAL;

        JLabel lblMonto = EstiloApp.crearLabel("Monto:");
        gbc.gridx = 0; gbc.gridy = 0;
        formulario.add(lblMonto, gbc);

        txtMonto = new JTextField(15);
        EstiloApp.aplicarEstiloCampo(txtMonto);
        gbc.gridx = 1;
        formulario.add(txtMonto, gbc);

        JLabel lblFormato = new JLabel("Ej: 1.500.000");
        lblFormato.setFont(new Font("Segoe UI", Font.ITALIC, 11));
        lblFormato.setForeground(EstiloApp.TEXTO_SEC);
        gbc.gridx = 2;
        formulario.add(lblFormato, gbc);

        JLabel lblDesc = EstiloApp.crearLabel("Descripcion:");
        gbc.gridx = 0; gbc.gridy = 1;
        formulario.add(lblDesc, gbc);

        txtDescripcion = new JTextField(15);
        EstiloApp.aplicarEstiloCampo(txtDescripcion);
        gbc.gridx = 1;
        formulario.add(txtDescripcion, gbc);

        JLabel lblFecha = EstiloApp.crearLabel("Fecha:");
        gbc.gridx = 0; gbc.gridy = 2;
        formulario.add(lblFecha, gbc);

        txtFecha = new JTextField(15);
        EstiloApp.aplicarEstiloCampo(txtFecha);
        txtFecha.setText(LocalDate.now().toString());
        gbc.gridx = 1;
        formulario.add(txtFecha, gbc);

        JLabel lblFormatoFecha = new JLabel("AAAA-MM-DD");
        lblFormatoFecha.setFont(new Font("Segoe UI", Font.ITALIC, 11));
        lblFormatoFecha.setForeground(EstiloApp.TEXTO_SEC);
        gbc.gridx = 2;
        formulario.add(lblFormatoFecha, gbc);

        panel.add(formulario);
        panel.add(Box.createVerticalStrut(10));

        // Botones agregar
        JPanel panelBotones = new JPanel(new FlowLayout(FlowLayout.CENTER, 15, 5));
        panelBotones.setBackground(EstiloApp.FONDO);

        JButton btnIngreso = EstiloApp.crearBoton("Agregar Ingreso", EstiloApp.EXITO);
        btnIngreso.addActionListener(e -> agregarMovimiento("Ingreso"));

        JButton btnGasto = EstiloApp.crearBoton("Agregar Gasto", EstiloApp.PELIGRO);
        btnGasto.addActionListener(e -> agregarMovimiento("Gasto"));

        panelBotones.add(btnIngreso);
        panelBotones.add(btnGasto);
        panel.add(panelBotones);
        panel.add(Box.createVerticalStrut(10));

        // Boton Ver Movimientos
        JPanel panelVer = new JPanel(new FlowLayout(FlowLayout.CENTER));
        panelVer.setBackground(EstiloApp.FONDO);
        JButton btnVer = EstiloApp.crearBoton("Ver Movimientos", EstiloApp.PRIMARIO);
        btnVer.addActionListener(e -> verMovimientos());
        panelVer.add(btnVer);
        panel.add(panelVer);
        panel.add(Box.createVerticalStrut(10));

        // Area de texto para lista de movimientos
        txtAreaLista = new JTextArea(8, 30);
        txtAreaLista.setEditable(false);
        EstiloApp.aplicarEstiloArea(txtAreaLista);
        JScrollPane scroll = new JScrollPane(txtAreaLista);
        scroll.setBorder(BorderFactory.createLineBorder(EstiloApp.BORDE));
        panel.add(scroll);

        return panel;
    }

    // Panel inferior con boton Volver
    private JPanel crearPanelInferior() {
        JPanel panel = new JPanel();
        panel.setBackground(EstiloApp.FONDO);
        panel.setBorder(BorderFactory.createEmptyBorder(5, 0, 10, 0));

        JButton btnVolver = EstiloApp.crearBoton("Volver al Menu", EstiloApp.GRIS);
        btnVolver.addActionListener(e -> {
            dispose();
            menuPrincipal.setVisible(true);
        });
        panel.add(btnVolver);
        return panel;
    }

    // Agrega un ingreso o gasto segun el tipo
    private void agregarMovimiento(String tipo) {
        double monto = parsearMonto(txtMonto.getText().trim());
        String descripcion = txtDescripcion.getText().trim();
        String fecha = txtFecha.getText().trim();
        if (!validar(txtMonto.getText().trim(), descripcion, monto)) return;

        if (!fecha.matches("\\d{4}-\\d{2}-\\d{2}")) {
            JOptionPane.showMessageDialog(this,
                "La fecha debe tener formato AAAA-MM-DD.\nEjemplo: 2026-04-12",
                "Fecha invalida", JOptionPane.WARNING_MESSAGE);
            return;
        }

        boolean exito;
        if (tipo.equals("Ingreso")) {
            exito = controller.agregarIngreso(monto, descripcion, fecha);
        } else {
            exito = controller.agregarGasto(monto, descripcion, fecha);
        }

        if (exito) {
            JOptionPane.showMessageDialog(this, tipo + " agregado correctamente.");
            txtMonto.setText("");
            txtDescripcion.setText("");
            txtFecha.setText(LocalDate.now().toString());
        } else {
            JOptionPane.showMessageDialog(this,
                "Error: No se pudo guardar. Verifica que XAMPP este encendido.",
                "Error de conexion", JOptionPane.ERROR_MESSAGE);
        }
    }

    // POLIMORFISMO: llama getTipo() que puede ser "Ingreso" o "Gasto"
    private void verMovimientos() {
        ArrayList<Movimiento> lista = controller.listarMovimientos();
        if (lista.isEmpty()) {
            txtAreaLista.setText("No hay movimientos registrados.");
            return;
        }

        StringBuilder texto = new StringBuilder();
        for (Movimiento m : lista) {
            texto.append(m.getTipo()).append(" | $")
                 .append(formatearNumero(m.getMonto()))
                 .append(" | ").append(m.getDescripcion()).append("\n");
        }
        txtAreaLista.setText(texto.toString());
    }

    // Convierte texto con formato de miles a numero (ej: 1.500.000 -> 1500000)
    private double parsearMonto(String texto) {
        try {
            long cantidadPuntos = texto.chars().filter(c -> c == '.').count();
            String limpio;
            if (cantidadPuntos > 1) {
                limpio = texto.replace(".", "");
            } else {
                limpio = texto;
            }
            return Double.parseDouble(limpio);
        } catch (NumberFormatException ex) {
            return -1;
        }
    }

    // Formatea un numero con separadores de miles
    private String formatearNumero(double numero) {
        long entero = (long) numero;
        String texto = String.valueOf(entero);
        StringBuilder resultado = new StringBuilder();
        int contador = 0;

        for (int i = texto.length() - 1; i >= 0; i--) {
            if (contador > 0 && contador % 3 == 0) {
                resultado.insert(0, ".");
            }
            resultado.insert(0, texto.charAt(i));
            contador++;
        }
        return resultado.toString();
    }

    // Valida que los campos no esten vacios y que el monto sea valido
    private boolean validar(String montoTexto, String descripcion, double monto) {
        if (montoTexto.isEmpty() || descripcion.isEmpty()) {
            JOptionPane.showMessageDialog(this, "Por favor complete todos los campos.");
            return false;
        }
        if (monto == -1) {
            JOptionPane.showMessageDialog(this, "El monto debe ser un numero valido.\nEjemplo: 1.500.000");
            return false;
        }
        if (monto <= 0) {
            JOptionPane.showMessageDialog(this, "El monto debe ser mayor a 0.");
            return false;
        }
        return true;
    }
}
