package modelo;

/**
 * CLASE ABSTRACTA - No se puede instanciar directamente.
 * Representa un movimiento financiero (ingreso o gasto).
 *
 * Conceptos de POO usados:
 * - ABSTRACCION: define la estructura comun sin implementar getTipo()
 * - ENCAPSULAMIENTO: atributos privados con getters/setters
 * - HERENCIA: Ingreso y Gasto heredan de esta clase
 */
public abstract class Movimiento {

    // Atributos privados (ENCAPSULAMIENTO)
    private int id;
    private double monto;
    private String descripcion;

    // Constructor - inicializa los atributos al crear el objeto
    public Movimiento(double monto, String descripcion) {
        this.monto = monto;
        this.descripcion = descripcion;
    }

    // Getters y Setters - acceso controlado a los atributos
    public void setId(int id) {
        this.id = id;
    }

    public double getMonto() {
        return monto;
    }

    public String getDescripcion() {
        return descripcion;
    }

    // Metodo abstracto - cada subclase debe implementarlo (POLIMORFISMO)
    public abstract String getTipo();
}
