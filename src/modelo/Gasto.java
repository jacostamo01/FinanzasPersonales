package modelo;

/**
 * HERENCIA: Gasto extiende de Movimiento.
 * Representa un gasto de dinero (almuerzo, arriendo, etc).
 *
 * Conceptos de POO usados:
 * - extends: hereda atributos y metodos de Movimiento
 * - super(): llama al constructor de la clase padre
 * - @Override: sobreescribe el metodo abstracto getTipo() (POLIMORFISMO)
 */
public class Gasto extends Movimiento {

    // Constructor - usa super() para inicializar los atributos del padre
    public Gasto(double monto, String descripcion) {
        super(monto, descripcion);
    }

    // Implementa el metodo abstracto de Movimiento
    @Override
    public String getTipo() {
        return "Gasto";
    }
}