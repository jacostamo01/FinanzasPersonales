package modelo;

/**
 * Representa una simulacion de inversion financiera.
 *
 * Conceptos de POO usados:
 * - ENCAPSULAMIENTO: atributos privados con getters
 * - RESPONSABILIDAD: la clase contiene la logica de calculo de interes
 */
public class Inversion {

    // Atributos privados (ENCAPSULAMIENTO)
    private double capital;       // Dinero invertido
    private double tasaInteres;   // Ej: 0.05 = 5%
    private int tiempo;           // En meses

    // Constructor - recibe los datos de la inversion
    public Inversion(double capital, double tasaInteres, int tiempo) {
        this.capital = capital;
        this.tasaInteres = tasaInteres;
        this.tiempo = tiempo;
    }

    // Formula: ganancia = capital * tasa * tiempo
    public double calcularGananciaSimple() {
        return capital * tasaInteres * tiempo;
    }

    // Capital + ganancia simple
    public double calcularMontoTotal() {
        return capital + calcularGananciaSimple();
    }
}
