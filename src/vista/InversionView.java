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
 */
public class InversionView extends JFrame {

    // Atributos privados (ENCAPSULAMIENTO)
    private InversionController controller;
    private Ahorro ahorro;

    // Componentes de la interfaz
    private JLabel lblCapitalDisp;
    private JTextField txtCapital;
    private JTextField txtTasa;
    private JTextField txtTiempo;
    private JTextArea txtResultados;
    private JButton btnCalcular;

    // Constructor - recibe el ahorro compartido para saber cuanto hay disponible
    public InversionView(Ahorro ahorro) {
        this.controller = new InversionController();
        this.ahorro = ahorro;
        configurarVentana();
        crearComponentes();
    }

    private void configurarVentana() {
        setTitle("Simulador de Inversiones");
        setSize(430, 420);
        setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
        setLocationRelativeTo(null);
        setLayout(null);
    }

    private void crearComponentes() {
        // Titulo
        JLabel lblTitulo = new JLabel("Simulador de Inversiones", SwingConstants.CENTER);
        lblTitulo.setFont(new Font("Arial", Font.BOLD, 16));
        lblTitulo.setBounds(0, 10, 430, 25);
        add(lblTitulo);

        // Muestra cuanto tiene ahorrado (capital disponible para invertir)
        JLabel lblDispTitulo = new JLabel("Capital disponible (ahorrado):");
        lblDispTitulo.setBounds(30, 50, 200, 20);
        add(lblDispTitulo);

        lblCapitalDisp = new JLabel("$" + String.format("%,.2f", ahorro.getSaldo()));
        lblCapitalDisp.setFont(new Font("Arial", Font.BOLD, 14));
        lblCapitalDisp.setForeground(new Color(52, 152, 219));
        lblCapitalDisp.setBounds(240, 45, 160, 25);
        add(lblCapitalDisp);

        JSeparator sep = new JSeparator();
        sep.setBounds(30, 80, 360, 2);
        add(sep);

        // Campo: Capital a invertir
        JLabel lbl1 = new JLabel("Capital a invertir:");
        lbl1.setBounds(30, 95, 130, 25);
        add(lbl1);

        txtCapital = new JTextField();
        txtCapital.setBounds(180, 95, 140, 25);
        add(txtCapital);

        // Campo: Tasa de interes
        JLabel lbl2 = new JLabel("Tasa interes (ej: 0.05):");
        lbl2.setBounds(30, 130, 150, 25);
        add(lbl2);

        txtTasa = new JTextField();
        txtTasa.setBounds(180, 130, 140, 25);
        add(txtTasa);

        // Campo: Tiempo en meses
        JLabel lbl3 = new JLabel("Tiempo (meses):");
        lbl3.setBounds(30, 165, 130, 25);
        add(lbl3);

        txtTiempo = new JTextField();
        txtTiempo.setBounds(180, 165, 140, 25);
        add(txtTiempo);

        // Boton para calcular la inversion
        btnCalcular = new JButton("Calcular Inversion");
        btnCalcular.setBounds(120, 210, 180, 35);
        btnCalcular.setBackground(new Color(26, 188, 156));
        btnCalcular.setForeground(Color.WHITE);
        btnCalcular.addActionListener(e -> calcular());
        add(btnCalcular);

        // Area de texto para mostrar los resultados
        txtResultados = new JTextArea();
        txtResultados.setEditable(false);
        txtResultados.setFont(new Font("Monospaced", Font.PLAIN, 12));
        JScrollPane scroll = new JScrollPane(txtResultados);
        scroll.setBounds(30, 260, 360, 110);
        add(scroll);
    }

    // Logica para calcular la inversion
    private void calcular() {
        double capital;
        double tasa;
        int tiempo;

        // Validar que los valores sean numericos
        try {
            capital = Double.parseDouble(txtCapital.getText().trim());
            tasa = Double.parseDouble(txtTasa.getText().trim());
            tiempo = Integer.parseInt(txtTiempo.getText().trim());
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "Ingrese valores numericos validos.");
            return;
        }

        // Validar que todos sean positivos
        if (capital <= 0 || tasa <= 0 || tiempo <= 0) {
            JOptionPane.showMessageDialog(this, "Todos los valores deben ser mayores a 0.");
            return;
        }

        // Validar que no invierta mas de lo que tiene ahorrado
        if (capital > ahorro.getSaldo()) {
            JOptionPane.showMessageDialog(this,
                "No puedes invertir mas de lo que tienes ahorrado ($"
                    + String.format("%,.2f", ahorro.getSaldo()) + ").",
                "Capital insuficiente", JOptionPane.ERROR_MESSAGE);
            return;
        }

        // Crear objeto Inversion y calcular (CREACION DE OBJETOS)
        Inversion inversion = new Inversion(capital, tasa, tiempo);

        double gananciaSimple = controller.calcularGanancia(inversion);
        double totalSimple = controller.calcularMontoTotal(inversion);

        // Mostrar resultados
        txtResultados.setText("");
        txtResultados.append("=== RESULTADO DE LA INVERSION ===\n\n");
        txtResultados.append("Capital invertido:   $" + String.format("%,.2f", capital) + "\n");
        txtResultados.append("Tasa de interes:     " + (tasa * 100) + " %\n");
        txtResultados.append("Tiempo:              " + tiempo + " meses\n");
        txtResultados.append("Ganancia (simple):   $" + String.format("%,.2f", gananciaSimple) + "\n");
        txtResultados.append("Total (simple):      $" + String.format("%,.2f", totalSimple) + "\n");
    }
}
