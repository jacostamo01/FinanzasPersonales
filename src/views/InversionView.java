package views;

import controller.InversionController;
import model.Inversion;

public class InversionView extends javax.swing.JFrame {

    public InversionController controller;

    public InversionView() {
        initComponents();
        this.controller = new InversionController();
    }

    @SuppressWarnings("unchecked")
    private void initComponents() {

        txtCapital = new javax.swing.JTextField();
        txtTasa = new javax.swing.JTextField();
        txtTiempo = new javax.swing.JTextField();

        txtGanancia = new javax.swing.JTextField();
        txtTotal = new javax.swing.JTextField();

        btnCalcular = new javax.swing.JButton();

        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();
        jLabel4 = new javax.swing.JLabel();
        jLabel5 = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);

        jLabel1.setText("Capital");
        jLabel2.setText("Tasa");
        jLabel3.setText("Tiempo");
        jLabel4.setText("Ganancia");
        jLabel5.setText("Total");

        txtGanancia.setEditable(false);
        txtTotal.setEditable(false);

        btnCalcular.setText("Calcular");
        btnCalcular.addActionListener(evt -> btnCalcularActionPerformed());

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);

        layout.setHorizontalGroup(
            layout.createSequentialGroup()
                .addGap(30)
                .addGroup(layout.createParallelGroup()
                    .addComponent(jLabel1)
                    .addComponent(jLabel2)
                    .addComponent(jLabel3)
                )
                .addGap(20)
                .addGroup(layout.createParallelGroup()
                    .addComponent(txtCapital, 120, 120, 120)
                    .addComponent(txtTasa, 120, 120, 120)
                    .addComponent(txtTiempo, 120, 120, 120)
                )
                .addGap(40)
                .addGroup(layout.createParallelGroup()
                    .addComponent(jLabel4)
                    .addComponent(jLabel5)
                )
                .addGap(20)
                .addGroup(layout.createParallelGroup()
                    .addComponent(txtGanancia, 120, 120, 120)
                    .addComponent(txtTotal, 120, 120, 120)
                )
        );

        layout.setVerticalGroup(
            layout.createSequentialGroup()
                .addGap(30)
                .addGroup(layout.createParallelGroup()
                    .addComponent(jLabel1)
                    .addComponent(txtCapital)
                    .addComponent(jLabel4)
                    .addComponent(txtGanancia)
                )
                .addGap(10)
                .addGroup(layout.createParallelGroup()
                    .addComponent(jLabel2)
                    .addComponent(txtTasa)
                    .addComponent(jLabel5)
                    .addComponent(txtTotal)
                )
                .addGap(10)
                .addGroup(layout.createParallelGroup()
                    .addComponent(jLabel3)
                    .addComponent(txtTiempo)
                )
                .addGap(30)
                .addComponent(btnCalcular)
                .addGap(30)
        );

        pack();
    }

    private void btnCalcularActionPerformed() {

        double capital = Double.parseDouble(txtCapital.getText());
        double tasa = Double.parseDouble(txtTasa.getText());
        int tiempo = Integer.parseInt(txtTiempo.getText());

        Inversion inversion = new Inversion(1, capital, tasa, tiempo);

        double ganancia = controller.calcularGanancia(inversion);
        double total = controller.calcularMontoTotal(inversion);

        txtGanancia.setText(String.valueOf(ganancia));
        txtTotal.setText(String.valueOf(total));
    }

    public static void main(String args[]) {
        java.awt.EventQueue.invokeLater(() -> new InversionView().setVisible(true));
    }

    // Variables
    private javax.swing.JButton btnCalcular;
    private javax.swing.JLabel jLabel1, jLabel2, jLabel3, jLabel4, jLabel5;

    private javax.swing.JTextField txtCapital, txtTasa, txtTiempo;
    private javax.swing.JTextField txtGanancia, txtTotal;
}