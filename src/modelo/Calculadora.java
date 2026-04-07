package modelo;

/**
 * Clase utilitaria con operaciones matematicas reutilizables.
 *
 * Conceptos de POO usados:
 * - REUTILIZACION: se usa desde EstadisticaService para no repetir logica
 * - METODOS: cada operacion esta separada en su propio metodo
 */
public class Calculadora {

    // Suma dos valores
    public double sumar(double a, double b) {
        return a + b;
    }

    // Resta: a - b
    public double restar(double a, double b) {
        return a - b;
    }

    // Calcula que porcentaje representa una parte del total
    // Ejemplo: porcentaje(gastos, ingresos) = que % de los ingresos se gasta
    public double porcentaje(double parte, double total) {
        if (total == 0) return 0;
        return (parte / total) * 100;
    }

    // Promedio: total dividido entre cantidad
    public double promedio(double total, int cantidad) {
        if (cantidad == 0) return 0;
        return total / cantidad;
    }
}
