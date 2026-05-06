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
 * - REUTILIZACION: usa EstiloApp para estilos unificados
 */
public class AhorroView extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private AhorroController controller;
    private Ahorro ahorro;
    private JTextField txtMonto;
    private JLabel lblDisponible;
    private JLabel lblAhorrado;

    // Constructor - recibe el objeto Ahorro compartido desde el menu
    public AhorroView(Ahorro ahorro) {
        this.controller = new AhorroController();
        this.ahorro = ahorro;

        EstiloApp.configurarVentana(this, "Ahorros", 450, 360);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(new BorderLayout());

        add(EstiloApp.crearHeader("MODULO DE AHORROS"), BorderLayout.NORTH);
        add(crearContenido(), BorderLayout.CENTER);
        actualizarSaldos();
    }

    private JPanel crearContenido() {
        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(EstiloApp.FONDO);
        panel.setBorder(BorderFactory.createEmptyBorder(15, 25, 15, 25));

        // Panel de informacion de saldos
        JPanel panelInfo = new JPanel(new GridLayout(2, 2, 10, 8));
        panelInfo.setBackground(EstiloApp.TARJETA);
        panelInfo.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(EstiloApp.BORDE),
            BorderFactory.createEmptyBorder(12, 15, 12, 15)));

        JLabel lblDispTitulo = EstiloApp.crearLabel("Disponible (Ingresos - Gastos):");
        panelInfo.add(lblDispTitulo);

        lblDisponible = new JLabel("$0");
        lblDisponible.setFont(EstiloApp.FUENTE_SUBTITULO);
        lblDisponible.setForeground(EstiloApp.EXITO);
        panelInfo.add(lblDisponible);

        JLabel lblAhoTitulo = EstiloApp.crearLabel("Total ahorrado:");
        panelInfo.add(lblAhoTitulo);

        lblAhorrado = new JLabel("$0");
        lblAhorrado.setFont(EstiloApp.FUENTE_SUBTITULO);
        lblAhorrado.setForeground(EstiloApp.PRIMARIO);
        panelInfo.add(lblAhorrado);

        panel.add(panelInfo);
        panel.add(Box.createVerticalStrut(15));

        // Campo monto
        JPanel panelMonto = new JPanel(new FlowLayout(FlowLayout.CENTER, 10, 0));
        panelMonto.setBackground(EstiloApp.FONDO);
        JLabel lblMonto = EstiloApp.crearLabel("Monto:");
        txtMonto = new JTextField(12);
        EstiloApp.aplicarEstiloCampo(txtMonto);
        panelMonto.add(lblMonto);
        panelMonto.add(txtMonto);
        panel.add(panelMonto);
        panel.add(Box.createVerticalStrut(15));

        // Botones
        JPanel panelBotones = new JPanel(new FlowLayout(FlowLayout.CENTER, 20, 0));
        panelBotones.setBackground(EstiloApp.FONDO);

        JButton btnDepositar = EstiloApp.crearBoton("Ahorrar", EstiloApp.EXITO);
        btnDepositar.addActionListener(e -> depositar());

        JButton btnRetirar = EstiloApp.crearBoton("Retirar", EstiloApp.PELIGRO);
        btnRetirar.addActionListener(e -> retirar());

        panelBotones.add(btnDepositar);
        panelBotones.add(btnRetirar);
        panel.add(panelBotones);

        return panel;
    }

    // Logica para ahorrar dinero
    private void depositar() {
        double monto = parsearMonto();
        if (monto < 0) return;

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
        double monto = parsearMonto();
        if (monto < 0) return;

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

    /**
     * REUTILIZACION: valida y convierte el texto del campo monto a numero.
     * Retorna -1 si es invalido, evitando duplicar validacion en depositar y retirar.
     */
    private double parsearMonto() {
        try {
            double monto = Double.parseDouble(txtMonto.getText().trim());
            if (monto <= 0) {
                JOptionPane.showMessageDialog(this, "El monto debe ser mayor a 0.");
                return -1;
            }
            return monto;
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "Ingrese un valor numerico valido.");
            return -1;
        }
    }

    // Actualiza los labels con los valores reales desde la BD
    private void actualizarSaldos() {
        double disponible = controller.getDisponible();
        lblDisponible.setText("$" + String.format("%,.2f", disponible));
        lblAhorrado.setText("$" + String.format("%,.2f", ahorro.getSaldo()));

        if (disponible < 0) {
            lblDisponible.setForeground(EstiloApp.PELIGRO);
        } else {
            lblDisponible.setForeground(EstiloApp.EXITO);
        }
    }
}
