package controlador;

import servicio.EstadisticaService;

/**
 * CONTROLADOR del modulo de estadisticas.
 * Conecta la Vista con el Servicio de estadisticas.
 *
 * Patron MVC:
 * - EstadisticaView llama a este controlador
 * - Este controlador llama a EstadisticaService
 * - EstadisticaService calcula usando datos de la BD
 */
public class EstadisticaController {

    private EstadisticaService servicio;

    public EstadisticaController() {
        this.servicio = new EstadisticaService();
    }

    // Obtiene el total de ingresos desde la BD
    public double getTotalIngresos() {
        return servicio.calcularTotalIngresos();
    }

    // Obtiene el total de gastos desde la BD
    public double getTotalGastos() {
        return servicio.calcularTotalGastos();
    }

    // Calcula ingresos - gastos
    public double balance(double ingresos, double gastos) {
        return servicio.balance(ingresos, gastos);
    }

    // Calcula el porcentaje de gastos sobre ingresos
    public double porcentaje(double gastos, double ingresos) {
        return servicio.porcentaje(gastos, ingresos);
    }

    // Calcula el promedio por movimiento
    public double promedio(double total, int cantidad) {
        return servicio.promedio(total, cantidad);
    }

    // Cuenta cuantos movimientos hay en la BD
    public int contarMovimientos() {
        return servicio.contarMovimientos();
    }
}
