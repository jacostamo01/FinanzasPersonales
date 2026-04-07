package vista;

import controlador.AhorroController;
import modelo.Ahorro;
import javax.swing.*;
import java.awt.*;

/**
 * Ventana del modulo de Ahorros.
 * Muestra el dinero disponible (ingresos - gastos) y permite ahorrar o retirar.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: recibe un objeto Ahorro compartido con InversionView
 * - ENCAPSULAMIENTO: los componentes son privados
 */
public class AhorroView extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private AhorroController controller;
    private Ahorro ahorro;

    // Componentes de la interfaz
    private JTextField txtMonto;
    private JLabel lblDisponible;
    private JLabel lblAhorrado;
    private JButton btnDepositar;
    private JButton btnRetirar;

    // Constructor - recibe el objeto Ahorro compartido desde el menu
    public AhorroView(Ahorro ahorro) {
        this.controller = new AhorroController();
        this.ahorro = ahorro;
        configurarVentana();
        crearComponentes();
        actualizarSaldos();
    }

    private void configurarVentana() {
        setTitle("Ahorros");
        setSize(420, 320);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(null);
    }

    private void crearComponentes() {
        // Titulo
        JLabel lblTitulo = new JLabel("Modulo de Ahorros", SwingConstants.CENTER);
        lblTitulo.setFont(new Font("Arial", Font.BOLD, 16));
        lblTitulo.setBounds(0, 10, 420, 25);
        add(lblTitulo);

        // Label: Dinero disponible (calculado desde la BD)
        JLabel lblDispTitulo = new JLabel("Dinero disponible (Ingresos - Gastos):");
        lblDispTitulo.setBounds(30, 50, 250, 20);
        add(lblDispTitulo);

        lblDisponible = new JLabel("$0");
        lblDisponible.setFont(new Font("Arial", Font.BOLD, 16));
        lblDisponible.setForeground(new Color(46, 204, 113));
        lblDisponible.setBounds(280, 45, 130, 25);
        add(lblDisponible);

        // Label: Total ahorrado
        JLabel lblAhoTitulo = new JLabel("Total ahorrado:");
        lblAhoTitulo.setBounds(30, 80, 250, 20);
        add(lblAhoTitulo);

        lblAhorrado = new JLabel("$0");
        lblAhorrado.setFont(new Font("Arial", Font.BOLD, 16));
        lblAhorrado.setForeground(new Color(52, 152, 219));
        lblAhorrado.setBounds(280, 75, 130, 25);
        add(lblAhorrado);

        // Separador visual
        JSeparator sep = new JSeparator();
        sep.setBounds(30, 115, 350, 2);
        add(sep);

        // Campo: Monto a ahorrar o retirar
        JLabel lblMonto = new JLabel("Monto:");
        lblMonto.setBounds(30, 135, 80, 25);
        add(lblMonto);

        txtMonto = new JTextField();
        txtMonto.setBounds(110, 135, 160, 25);
        add(txtMonto);

        // Boton Ahorrar
        btnDepositar = new JButton("Ahorrar");
        btnDepositar.setBounds(50, 185, 140, 35);
        btnDepositar.setBackground(new Color(46, 204, 113));
        btnDepositar.setForeground(Color.WHITE);
        btnDepositar.addActionListener(e -> depositar());
        add(btnDepositar);

        // Boton Retirar
        btnRetirar = new JButton("Retirar");
        btnRetirar.setBounds(220, 185, 140, 35);
        btnRetirar.setBackground(new Color(231, 76, 60));
        btnRetirar.setForeground(Color.WHITE);
        btnRetirar.addActionListener(e -> retirar());
        add(btnRetirar);
    }

    // Logica para ahorrar dinero
    private void depositar() {
        double monto;
        try {
            monto = Double.parseDouble(txtMonto.getText().trim());
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "Ingrese un valor numerico valido.");
            return;
        }

        if (monto <= 0) {
            JOptionPane.showMessageDialog(this, "El monto debe ser mayor a 0.");
            return;
        }

        if (controller.depositar(ahorro, monto)) {
            actualizarSaldos();
            txtMonto.setText("");
            JOptionPane.showMessageDialog(this,
                "Ahorro guardado: $" + String.format("%,.2f", monto));
        } else {
            double disponible = controller.getDisponible();
            double libre = disponible - ahorro.getSaldo();
            JOptionPane.showMessageDialog(this,
                "No alcanza. Solo tienes $" + String.format("%,.2f", libre)
                    + " disponible para ahorrar.",
                "Fondos insuficientes", JOptionPane.ERROR_MESSAGE);
        }
    }

    // Logica para retirar dinero del ahorro
    private void retirar() {
        double monto;
        try {
            monto = Double.parseDouble(txtMonto.getText().trim());
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "Ingrese un valor numerico valido.");
            return;
        }

        if (monto <= 0) {
            JOptionPane.showMessageDialog(this, "El monto debe ser mayor a 0.");
            return;
        }

        if (controller.retirar(ahorro, monto)) {
            actualizarSaldos();
            txtMonto.setText("");
            JOptionPane.showMessageDialog(this,
                "Retiro exitoso: $" + String.format("%,.2f", monto));
        } else {
            JOptionPane.showMessageDialog(this,
                "No puedes retirar mas de lo ahorrado ($"
                    + String.format("%,.2f", ahorro.getSaldo()) + ").",
                "Fondos insuficientes", JOptionPane.ERROR_MESSAGE);
        }
    }

    // Actualiza los labels con los valores reales desde la BD
    private void actualizarSaldos() {
        double disponible = controller.getDisponible();
        lblDisponible.setText("$" + String.format("%,.2f", disponible));
        lblAhorrado.setText("$" + String.format("%,.2f", ahorro.getSaldo()));

        // Cambia el color segun si el disponible es positivo o negativo
        if (disponible < 0) {
            lblDisponible.setForeground(new Color(231, 76, 60));
        } else {
            lblDisponible.setForeground(new Color(46, 204, 113));
        }
    }
}
