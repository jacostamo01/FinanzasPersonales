package vista;

import controlador.MovimientoController;
import modelo.Movimiento;
import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;

public class ViewMovimientos extends JFrame {

    private JTextField txtMonto;
    private JTextField txtDescripcion;
    private JTextArea txtAreaLista;
    private JButton btnIngreso;
    private JButton btnGasto;
    private JButton btnVer;
    private JButton btnVolver;

    private MovimientoController controller;
    private ViewMenuPrincipal menuPrincipal; // referencia al menu original

    public ViewMovimientos(String usuario, ViewMenuPrincipal menu, MovimientoController controller) {
        this.menuPrincipal = menu; // guarda el menu que ya existia
        this.controller = controller; // usa el controller que ya existia, no crea uno nuevo
        configurarVentana();
        crearComponentes();
        agregarEventos();
    }

    private void configurarVentana() {
        setTitle("Ingresos y Gastos");
        setSize(450, 500);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(null);
        setLocationRelativeTo(null);
    }

    private void crearComponentes() {

        JLabel lblTitulo = new JLabel("Registrar Movimiento", SwingConstants.CENTER);
        lblTitulo.setFont(new Font("Arial", Font.BOLD, 16));
        lblTitulo.setBounds(0, 10, 450, 30);
        add(lblTitulo);

        JLabel lblMonto = new JLabel("Monto:");
        lblMonto.setBounds(40, 60, 80, 25);
        add(lblMonto);

        txtMonto = new JTextField();
        txtMonto.setBounds(130, 60, 150, 25);
        add(txtMonto);

        // pequeña ayuda visual para el formato
        JLabel lblFormato = new JLabel("Ej: 1.500.000");
        lblFormato.setFont(new Font("Arial", Font.ITALIC, 10));
        lblFormato.setForeground(Color.GRAY);
        lblFormato.setBounds(290, 62, 100, 20);
        add(lblFormato);

        JLabel lblDescripcion = new JLabel("Descripción:");
        lblDescripcion.setBounds(40, 100, 90, 25);
        add(lblDescripcion);

        txtDescripcion = new JTextField();
        txtDescripcion.setBounds(130, 100, 150, 25);
        add(txtDescripcion);

        btnIngreso = new JButton("Agregar Ingreso");
        btnIngreso.setBounds(40, 150, 150, 35);
        btnIngreso.setBackground(new Color(50, 180, 50));
        btnIngreso.setForeground(Color.WHITE);
        add(btnIngreso);

        btnGasto = new JButton("Agregar Gasto");
        btnGasto.setBounds(220, 150, 150, 35);
        btnGasto.setBackground(new Color(220, 60, 60));
        btnGasto.setForeground(Color.WHITE);
        add(btnGasto);

        btnVer = new JButton("Ver Movimientos");
        btnVer.setBounds(130, 205, 170, 30);
        add(btnVer);

        txtAreaLista = new JTextArea();
        txtAreaLista.setEditable(false);
        JScrollPane scroll = new JScrollPane(txtAreaLista);
        scroll.setBounds(30, 250, 390, 150);
        add(scroll);

        // boton para volver al menu principal
        btnVolver = new JButton("Volver al Menú");
        btnVolver.setBounds(130, 415, 170, 30);
        btnVolver.setBackground(new Color(100, 100, 100));
        btnVolver.setForeground(Color.WHITE);
        add(btnVolver);
    }

    private void agregarEventos() {

        btnIngreso.addActionListener(e -> {
            double monto = parsearMonto(txtMonto.getText().trim());
            String descripcion = txtDescripcion.getText().trim();
            if (!validar(txtMonto.getText().trim(), descripcion, monto)) return;
            controller.agregarIngreso(monto, descripcion);
            JOptionPane.showMessageDialog(this, "Ingreso agregado correctamente.");
            limpiarCampos();
        });

        btnGasto.addActionListener(e -> {
            double monto = parsearMonto(txtMonto.getText().trim());
            String descripcion = txtDescripcion.getText().trim();
            if (!validar(txtMonto.getText().trim(), descripcion, monto)) return;
            controller.agregarGasto(monto, descripcion);
            JOptionPane.showMessageDialog(this, "Gasto agregado correctamente.");
            limpiarCampos();
        });

        btnVer.addActionListener(e -> {
            ArrayList<Movimiento> lista = controller.listarMovimientos();
            if (lista.isEmpty()) {
                txtAreaLista.setText("No hay movimientos registrados.");
                return;
            }
            String texto = "";
            for (Movimiento m : lista) {
                texto += m.getTipo() + " | $" + formatearNumero(m.getMonto())
                        + " | " + m.getDescripcion() + "\n";
            }
            txtAreaLista.setText(texto);
        });

        // cierra esta ventana y vuelve a mostrar el menu original
        btnVolver.addActionListener(e -> {
            dispose(); // cierra ViewMovimientos
            menuPrincipal.setVisible(true); // vuelve a mostrar el menu original
        });
    }

    // quita los puntos y convierte a double
    // acepta: 1.500.000 o 1500000 o 1500.50
    private double parsearMonto(String texto) {
        try {
            long cantidadPuntos = texto.chars().filter(c -> c == '.').count();
            String limpio;
            if (cantidadPuntos > 1) {
                // ejemplo: 1.500.000 -> 1500000
                limpio = texto.replace(".", "");
            } else {
                // ejemplo: 1500.50 -> decimal normal
                limpio = texto;
            }
            return Double.parseDouble(limpio);
        } catch (NumberFormatException ex) {
            return -1;
        }
    }

    // formatea el numero de vuelta con puntos al mostrarlo
    private String formatearNumero(double numero) {
        long entero = (long) numero;
        String texto = String.valueOf(entero);
        String resultado = "";
        int contador = 0;
        for (int i = texto.length() - 1; i >= 0; i--) {
            if (contador > 0 && contador % 3 == 0) {
                resultado = "." + resultado;
            }
            resultado = texto.charAt(i) + resultado;
            contador++;
        }
        return resultado;
    }

    // validaciones centralizadas para no repetir codigo
    private boolean validar(String montoTexto, String descripcion, double monto) {
        if (montoTexto.isEmpty() || descripcion.isEmpty()) {
            JOptionPane.showMessageDialog(this, "Por favor complete todos los campos.");
            return false;
        }
        if (monto == -1) {
            JOptionPane.showMessageDialog(this, "El monto debe ser un número válido.\nEjemplo: 1.500.000");
            return false;
        }
        if (monto <= 0) {
            JOptionPane.showMessageDialog(this, "El monto debe ser mayor a 0.");
            return false;
        }
        return true;
    }

    private void limpiarCampos() {
        txtMonto.setText("");
        txtDescripcion.setText("");
    }
}
