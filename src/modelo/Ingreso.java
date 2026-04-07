package modelo;

/**
 * HERENCIA: Ingreso extiende de Movimiento.
 * Representa un ingreso de dinero (sueldo, pago, etc).
 *
 * Conceptos de POO usados:
 * - extends: hereda atributos y metodos de Movimiento
 * - super(): llama al constructor de la clase padre
 * - @Override: sobreescribe el metodo abstracto getTipo() (POLIMORFISMO)
 */
public class Ingreso extends Movimiento {

    // Constructor - usa super() para inicializar los atributos del padre
    public Ingreso(double monto, String descripcion) {
        super(monto, descripcion);
    }

    // Implementa el metodo abstracto de Movimiento
    @Override
    public String getTipo() {
        return "Ingreso";
    }
}