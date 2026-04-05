package model;

public class Ahorro {
    private int id;
    private int usuarioId;
    private double saldo;

    public Ahorro(int usuarioId) {
        this.usuarioId = usuarioId;
        this.saldo = 0.0;
    }

    public void depositar(double monto) {
        if (monto > 0) {
            saldo += monto;
        }
    }

    public boolean retirar(double monto) {
        if (monto > 0 && saldo >= monto) {
            saldo -= monto;
            return true;
        }
        return false;
    }

    public double getSaldo() {
        return saldo;
    }

    public int getUsuarioId() {
        return usuarioId;
    }
}