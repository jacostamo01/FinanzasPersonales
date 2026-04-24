package vista;

import controlador.InversionController;
import modelo.Inversion;
import modelo.Ahorro;
import javax.swing.*;
import java.awt.*;

/**
 * Ventana del simulador de inversiones.
 * Calcula ganancia simple usando el capital ahorrado.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende de JFrame
 * - COMPOSICION: recibe el mismo objeto Ahorro que AhorroView
 * - CREACION DE OBJETOS: crea un objeto Inversion con los datos ingresados
 * - REUTILIZACION: usa EstiloApp para estilos unificados
 */
public class InversionView extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private InversionController controller;
    private Ahorro ahorro;
    private JLabel lblCapitalDisp;
    private JTextField txtCapital;
    private JTextField txtTasa;
    private JTextField txtTiempo;
    private JTextArea txtResultados;

    // Constructor - recibe el ahorro compartido para saber cuanto hay disponible
    public InversionView(Ahorro ahorro) {
        this.controller = new InversionController();
        this.ahorro = ahorro;

        EstiloApp.configurarVentana(this, "Simulador de Inversiones", 460, 480);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLayout(new BorderLayout());

        add(EstiloApp.crearHeader("SIMULADOR DE INVERSIONES"), BorderLayout.NORTH);
        add(crearContenido(), BorderLayout.CENTER);
    }

    private JPanel crearContenido() {
        JPanel panel = new JPanel();
        panel.setLayout(new BoxLayout(panel, BoxLayout.Y_AXIS));
        panel.setBackground(EstiloApp.FONDO);
        panel.setBorder(BorderFactory.createEmptyBorder(15, 20, 15, 20));

        // Capital disponible
        JPanel panelCapital = new JPanel(new FlowLayout(FlowLayout.CENTER, 10, 5));
        panelCapital.setBackground(EstiloApp.TARJETA);
        panelCapital.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(EstiloApp.BORDE),
            BorderFactory.createEmptyBorder(8, 15, 8, 15)));

        JLabel lblTitCap = EstiloApp.crearLabel("Capital disponible (ahorrado):");
        panelCapital.add(lblTitCap);

        lblCapitalDisp = new JLabel("$" + String.format("%,.2f", ahorro.getSaldo()));
        lblCapitalDisp.setFont(EstiloApp.FUENTE_SUBTITULO);
        lblCapitalDisp.setForeground(EstiloApp.PRIMARIO);
        panelCapital.add(lblCapitalDisp);

        panel.add(panelCapital);
        panel.add(Box.createVerticalStrut(12));

        // Formulario
        JPanel formulario = new JPanel(new GridBagLayout());
        formulario.setBackground(EstiloApp.FONDO);
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.insets = new Insets(5, 5, 5, 5);
        gbc.fill = GridBagConstraints.HORIZONTAL;

        txtCapital = new JTextField(12);
        txtTasa = new JTextField(12);
        txtTiempo = new JTextField(12);

        agregarCampo(formulario, gbc, 0, "Capital a invertir:", txtCapital);
        agregarCampo(formulario, gbc, 1, "Tasa interes (ej: 0.05):", txtTasa);
        agregarCampo(formulario, gbc, 2, "Tiempo (meses):", txtTiempo);

        panel.add(formulario);
        panel.add(Box.createVerticalStrut(10));

        // Boton calcular
        JPanel panelBtn = new JPanel(new FlowLayout(FlowLayout.CENTER));
        panelBtn.setBackground(EstiloApp.FONDO);
        JButton btnCalcular = EstiloApp.crearBoton("Calcular Inversion", EstiloApp.TURQUESA);
        btnCalcular.addActionListener(e -> calcular());
        panelBtn.add(btnCalcular);
        panel.add(panelBtn);
        panel.add(Box.createVerticalStrut(10));

        // Resultados
        txtResultados = new JTextArea(6, 30);
        txtResultados.setEditable(false);
        EstiloApp.aplicarEstiloArea(txtResultados);
        JScrollPane scroll = new JScrollPane(txtResultados);
        scroll.setBorder(BorderFactory.createLineBorder(EstiloApp.BORDE));
        panel.add(scroll);

        return panel;
    }

    // Metodo auxiliar para agregar un campo al formulario con GridBagLayout
    private void agregarCampo(JPanel panel, GridBagConstraints gbc, int fila,
                              String etiqueta, JTextField campo) {
        JLabel lbl = EstiloApp.crearLabel(etiqueta);
        EstiloApp.aplicarEstiloCampo(campo);
        gbc.gridx = 0; gbc.gridy = fila;
        panel.add(lbl, gbc);
        gbc.gridx = 1;
        panel.add(campo, gbc);
    }

    // Logica para calcular la inversion
    private void calcular() {
        double capital;
        double tasa;
        int tiempo;

        try {
            capital = Double.parseDouble(txtCapital.getText().trim());
            tasa = Double.parseDouble(txtTasa.getText().trim());
            tiempo = Integer.parseInt(txtTiempo.getText().trim());
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "Ingrese valores numericos validos.");
            return;
        }

        if (capital <= 0 || tasa <= 0 || tiempo <= 0) {
            JOptionPane.showMessageDialog(this, "Todos los valores deben ser mayores a 0.");
            return;
        }

        if (capital > ahorro.getSaldo()) {
            JOptionPane.showMessageDialog(this,
                "No puedes invertir mas de lo que tienes ahorrado ($"
                    + String.format("%,.2f", ahorro.getSaldo()) + ").",
                "Capital insuficiente", JOptionPane.ERROR_MESSAGE);
            return;
        }

        // CREACION DE OBJETOS: crea un objeto Inversion con los datos ingresados
        Inversion inversion = new Inversion(capital, tasa, tiempo);

        double gananciaSimple = controller.calcularGanancia(inversion);
        double totalSimple = controller.calcularMontoTotal(inversion);

        txtResultados.setText("");
        txtResultados.append("=== RESULTADO DE LA INVERSION ===\n\n");
        txtResultados.append("Capital invertido:   $" + String.format("%,.2f", capital) + "\n");
        txtResultados.append("Tasa de interes:     " + (tasa * 100) + " %\n");
        txtResultados.append("Tiempo:              " + tiempo + " meses\n");
        txtResultados.append("Ganancia (simple):   $" + String.format("%,.2f", gananciaSimple) + "\n");
        txtResultados.append("Total (simple):      $" + String.format("%,.2f", totalSimple) + "\n");
    }
}
