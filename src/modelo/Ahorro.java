package modelo;

/**
 * Representa el ahorro del usuario.
 *
 * Conceptos de POO usados:
 * - ENCAPSULAMIENTO: el saldo es privado y solo se modifica con depositar/retirar
 * - VALIDACION: los metodos verifican que los montos sean validos antes de modificar el saldo
 */
public class Ahorro {

    // Atributos privados (ENCAPSULAMIENTO)
    private double saldo;

    // Constructor - inicia con saldo en 0
    public Ahorro() {
        this.saldo = 0.0;
    }

    // Agrega dinero al ahorro (solo si el monto es positivo)
    public void depositar(double monto) {
        if (monto > 0) {
            saldo += monto;
        }
    }

    // Retira dinero del ahorro (solo si hay suficiente saldo)
    public boolean retirar(double monto) {
        if (monto > 0 && saldo >= monto) {
            saldo -= monto;
            return true;
        }
        return false;
    }

    // Getter
    public double getSaldo() {
        return saldo;
    }
}
