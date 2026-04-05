package vista;

import javax.swing.*;
import java.awt.*;
import Controller.EstadisticaController;

//Herencia, clase que hereda componentes del Jframe
public class EstadisticaView extends JFrame {
    //Variables
    private EstadisticaController controlador; //Objeto
    private JTextField txtIngresos, txtGastos;
    private JTextArea txtResultados;
    private JButton btnCalcular;

    public EstadisticaView() {
        controlador = new EstadisticaController(); //Inicializacion

        //Titulo
        setTitle("Estadísticas Financieras");
        setSize(420, 500);
        setLocationRelativeTo(null); //Posicion de ventana (centrada)

        JPanel jpEstadistica = new JPanel();
        jpEstadistica.setBackground(new Color(33, 97, 140));
        jpEstadistica.setPreferredSize(new Dimension(100, 50));

        JLabel lbTitulo = new JLabel("ESTADISTICAS FINANCIERAS");
        lbTitulo.setForeground(Color.WHITE);
        lbTitulo.setFont(new Font("Segoe UI", Font.BOLD, 18));
        jpEstadistica.add(lbTitulo);
        add(jpEstadistica, BorderLayout.NORTH);


        //Panel que contiene a Ingresos y Gastos
        JPanel panelCentro = new JPanel();
        panelCentro.setLayout(new BoxLayout(panelCentro, BoxLayout.Y_AXIS));
        panelCentro.setBackground(Color.white);

        //Panel Datos
        JPanel jpDatos = new JPanel(new GridLayout(2, 100, 10, 10));
        jpDatos.setBorder(BorderFactory.createTitledBorder("DATOS"));
        jpDatos.setBackground(new Color(245, 245, 245));

        JLabel lbIngresos = new JLabel("Ingresos:");
        lbIngresos.setFont(new Font("Segoe UI", Font.BOLD, 14));
        JLabel lbGastos = new JLabel("Gastos:");
        lbGastos.setFont(new Font("Segoe UI", Font.BOLD, 14));

        txtIngresos = new JTextField();
        txtIngresos.setFont(new Font("Segoe UI", 0, 14));
        txtGastos = new JTextField();
        txtGastos.setFont(new Font("Segoe UI",0, 14));

        jpDatos.add(lbIngresos);
        jpDatos.add(txtIngresos);
        jpDatos.add(lbGastos);
        jpDatos.add(txtGastos);

        //Boton
        btnCalcular = new JButton("Calcular");
        btnCalcular.setBackground(new Color(52, 152, 219));
        btnCalcular.setForeground(Color.WHITE);
        btnCalcular.setFont(new Font("Segoe UI", Font.BOLD, 14));
        btnCalcular.setFocusPainted(false);
        btnCalcular.setAlignmentX(CENTER_ALIGNMENT);
        btnCalcular.addActionListener(e -> calcular());


        //Panel de resultados
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

        //Panel que contenga a los demas
        panelCentro.add(Box.createVerticalStrut(20));
        panelCentro.add(jpDatos);
        panelCentro.add(Box.createVerticalStrut(20));
        panelCentro.add(btnCalcular);
        panelCentro.add(Box.createVerticalStrut(15));
        panelCentro.add(jpResultado);

        add(panelCentro, BorderLayout.CENTER);
    }

    //metodos
    private void calcular() {
        try {
            //Abstraccion (Solo llama lo necesario(Service))
            double ingresos = Double.parseDouble(txtIngresos.getText());
            double gastos = Double.parseDouble(txtGastos.getText());

            double balance = controlador.balance(ingresos, gastos);
            double porcentaje = controlador.porcentaje(gastos, ingresos);

            //Visualizacion de Resultados
             txtResultados.append("REGISTROS: " + "\n");
            txtResultados.append("Ingresos: " + ingresos + "\n");
            txtResultados.append("Gastos: " + gastos + "\n");
            txtResultados.append("Balance: " + balance + "\n");
            txtResultados.append("Porcentaje de gastos: " + porcentaje + " %\n");

        } catch (Exception e) {
            JOptionPane.showMessageDialog(this, "Verifique que los campos solo contengan numeros");
        }
    }

    // MAIN
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new EstadisticaView().setVisible(true));
    }
}