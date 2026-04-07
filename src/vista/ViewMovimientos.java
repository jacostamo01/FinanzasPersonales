package vista;

import controlador.MovimientoController;
import modelo.Movimiento;
import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;

/**
 * Ventana para registrar ingresos y gastos.
 * Permite agregar, ver y borrar movimientos de la BD.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: recibe el controlador como parametro (no crea uno nuevo)
 * - POLIMORFISMO: muestra Ingresos y Gastos como Movimiento usando getTipo()
 */
public class ViewMovimientos extends JFrame {

    // Componentes de la interfaz (ENCAPSULAMIENTO)
    private JTextField txtMonto;
    private JTextField txtDescripcion;
    private JTextArea txtAreaLista;
    private JButton btnIngreso;
    private JButton btnGasto;
    private JButton btnVer;
    private JButton btnBorrar;
    private JButton btnVolver;

    // Referencias a otros objetos (COMPOSICION)
    private MovimientoController controller;
    private ViewMenuPrincipal menuPrincipal;

    // Constructor - recibe el controlador ya existente (no crea uno nuevo)
    public ViewMovimientos(String usuario, ViewMenuPrincipal menu, MovimientoController controller) {
        this.menuPrincipal = menu;
        this.controller = controller;
        configurarVentana();
        crearComponentes();
        agregarEventos();
    }

    private void configurarVentana() {
        setTitle("Ingresos y Gastos");
        setSize(450, 550);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(null);
        setLocationRelativeTo(null);
    }

    private void crearComponentes() {
        // Titulo
        JLabel lblTitulo = new JLabel("Registrar Movimiento", SwingConstants.CENTER);
        lblTitulo.setFont(new Font("Arial", Font.BOLD, 16));
        lblTitulo.setBounds(0, 10, 450, 30);
        add(lblTitulo);

        // Campo: Monto
        JLabel lblMonto = new JLabel("Monto:");
        lblMonto.setBounds(40, 60, 80, 25);
        add(lblMonto);

        txtMonto = new JTextField();
        txtMonto.setBounds(130, 60, 150, 25);
        add(txtMonto);

        JLabel lblFormato = new JLabel("Ej: 1.500.000");
        lblFormato.setFont(new Font("Arial", Font.ITALIC, 10));
        lblFormato.setForeground(Color.GRAY);
        lblFormato.setBounds(290, 62, 100, 20);
        add(lblFormato);

        // Campo: Descripcion
        JLabel lblDescripcion = new JLabel("Descripcion:");
        lblDescripcion.setBounds(40, 100, 90, 25);
        add(lblDescripcion);

        txtDescripcion = new JTextField();
        txtDescripcion.setBounds(130, 100, 150, 25);
        add(txtDescripcion);

        // Botones de accion
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

        // Area de texto para mostrar la lista de movimientos
        txtAreaLista = new JTextArea();
        txtAreaLista.setEditable(false);
        JScrollPane scroll = new JScrollPane(txtAreaLista);
        scroll.setBounds(30, 250, 390, 150);
        add(scroll);

        // Boton para borrar todos los registros
        btnBorrar = new JButton("Borrar Registros");
        btnBorrar.setBounds(130, 415, 170, 30);
        btnBorrar.setBackground(new Color(200, 50, 50));
        btnBorrar.setForeground(Color.WHITE);
        add(btnBorrar);

        // Boton para volver al menu principal
        btnVolver = new JButton("Volver al Menu");
        btnVolver.setBounds(130, 460, 170, 30);
        btnVolver.setBackground(new Color(100, 100, 100));
        btnVolver.setForeground(Color.WHITE);
        add(btnVolver);
    }

    // Asigna las acciones a cada boton
    private void agregarEventos() {

        // Boton Agregar Ingreso
        btnIngreso.addActionListener(e -> {
            double monto = parsearMonto(txtMonto.getText().trim());
            String descripcion = txtDescripcion.getText().trim();
            if (!validar(txtMonto.getText().trim(), descripcion, monto)) return;

            if (controller.agregarIngreso(monto, descripcion)) {
                JOptionPane.showMessageDialog(this, "Ingreso agregado correctamente.");
                limpiarCampos();
            } else {
                JOptionPane.showMessageDialog(this,
                    "Error: No se pudo guardar. Verifica que XAMPP este encendido.",
                    "Error de conexion", JOptionPane.ERROR_MESSAGE);
            }
        });

        // Boton Agregar Gasto
        btnGasto.addActionListener(e -> {
            double monto = parsearMonto(txtMonto.getText().trim());
            String descripcion = txtDescripcion.getText().trim();
            if (!validar(txtMonto.getText().trim(), descripcion, monto)) return;

            if (controller.agregarGasto(monto, descripcion)) {
                JOptionPane.showMessageDialog(this, "Gasto agregado correctamente.");
                limpiarCampos();
            } else {
                JOptionPane.showMessageDialog(this,
                    "Error: No se pudo guardar. Verifica que XAMPP este encendido.",
                    "Error de conexion", JOptionPane.ERROR_MESSAGE);
            }
        });

        // Boton Ver Movimientos - POLIMORFISMO: llama getTipo() que puede ser Ingreso o Gasto
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

        // Boton Borrar Registros - pide confirmacion antes de eliminar
        btnBorrar.addActionListener(e -> {
            int opcion = JOptionPane.showConfirmDialog(this,
                "Estas seguro? Se eliminaran TODOS los movimientos.",
                "Confirmar borrado", JOptionPane.YES_NO_OPTION, JOptionPane.WARNING_MESSAGE);

            if (opcion == JOptionPane.YES_OPTION) {
                if (controller.eliminarTodos()) {
                    txtAreaLista.setText("");
                    JOptionPane.showMessageDialog(this, "Todos los registros fueron eliminados.");
                } else {
                    JOptionPane.showMessageDialog(this,
                        "Error al eliminar. Verifica que XAMPP este encendido.",
                        "Error", JOptionPane.ERROR_MESSAGE);
                }
            }
        });

        // Boton Volver - cierra esta ventana y muestra el menu
        btnVolver.addActionListener(e -> {
            dispose();
            menuPrincipal.setVisible(true);
        });
    }

    /**
     * Convierte el texto del monto a un numero double.
     * Acepta formatos como: 1.500.000 o 1500000 o 1500.50
     */
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

    // Formatea un numero con puntos de miles para mostrarlo bonito
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

    // Limpia los campos de texto despues de agregar un movimiento
    private void limpiarCampos() {
        txtMonto.setText("");
        txtDescripcion.setText("");
    }
}
