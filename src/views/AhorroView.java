package views;

import controller.AhorroController;
import model.Ahorro;

public class AhorroView extends javax.swing.JFrame {

    public AhorroController controller;
    private Ahorro ahorro;

    public AhorroView() {
        initComponents();
        this.controller = new AhorroController();

        // Simulación usuario logueado (luego lo reemplazan)
        this.ahorro = new Ahorro(1);
    }

    @SuppressWarnings("unchecked")
    private void initComponents() {

        txtMonto = new javax.swing.JTextField();
        txtSaldo = new javax.swing.JTextField();

        btnDepositar = new javax.swing.JButton();
        btnRetirar = new javax.swing.JButton();

        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);

        jLabel1.setText("Monto");
        jLabel2.setText("Saldo");

        btnDepositar.setText("Depositar");
        btnDepositar.addActionListener(evt -> btnDepositarActionPerformed());

        btnRetirar.setText("Retirar");
        btnRetirar.addActionListener(evt -> btnRetirarActionPerformed());

        txtSaldo.setEditable(false);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);

        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
            .addGroup(layout.createSequentialGroup()
                .addGap(30)
                .addComponent(jLabel1)
                .addGap(20)
                .addComponent(txtMonto, javax.swing.GroupLayout.PREFERRED_SIZE, 120, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(30)
                .addComponent(jLabel2)
                .addGap(20)
                .addComponent(txtSaldo, javax.swing.GroupLayout.PREFERRED_SIZE, 120, javax.swing.GroupLayout.PREFERRED_SIZE)
            )
            .addGroup(layout.createSequentialGroup()
                .addGap(60)
                .addComponent(btnDepositar)
                .addGap(40)
                .addComponent(btnRetirar)
            )
        );

        layout.setVerticalGroup(
            layout.createSequentialGroup()
                .addGap(30)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel1)
                    .addComponent(txtMonto)
                    .addComponent(jLabel2)
                    .addComponent(txtSaldo)
                )
                .addGap(30)
                .addGroup(layout.createParallelGroup()
                    .addComponent(btnDepositar)
                    .addComponent(btnRetirar)
                )
                .addGap(30)
        );

        pack();
    }

    private void btnDepositarActionPerformed() {
        double monto = Double.parseDouble(txtMonto.getText());

        controller.depositar(ahorro, monto);

        txtSaldo.setText(String.valueOf(ahorro.getSaldo()));
    }

    private void btnRetirarActionPerformed() {
        double monto = Double.parseDouble(txtMonto.getText());

        controller.retirar(ahorro, monto);

        txtSaldo.setText(String.valueOf(ahorro.getSaldo()));
    }

    public static void main(String args[]) {
        java.awt.EventQueue.invokeLater(() -> new AhorroView().setVisible(true));
    }

    // Variables
    private javax.swing.JButton btnDepositar, btnRetirar;
    private javax.swing.JLabel jLabel1, jLabel2;
    private javax.swing.JTextField txtMonto, txtSaldo;
}