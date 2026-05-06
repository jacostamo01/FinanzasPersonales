package controlador;

import modelo.Ahorro;
import servicio.AhorroService;

/**
 * CONTROLADOR del modulo de ahorros.
 * Conecta la Vista (AhorroView) con el Servicio (AhorroService).
 *
 * Patron MVC:
 * - AhorroView llama a este controlador
 * - Este controlador llama a AhorroService
 * - AhorroService valida y modifica el modelo Ahorro
 */
public class AhorroController {

    private AhorroService servicio;

    public AhorroController() {
        this.servicio = new AhorroService();
    }

    // Intenta depositar en el ahorro (retorna true si se pudo)
    public boolean depositar(Ahorro ahorro, double monto) {
        return servicio.guardarDinero(ahorro, monto);
    }

    // Intenta retirar del ahorro (retorna true si hay suficiente)
    public boolean retirar(Ahorro ahorro, double monto) {
        return servicio.retirarDinero(ahorro, monto);
    }

    // Consulta cuanto dinero disponible hay (ingresos - gastos desde la BD)
    public double getDisponible() {
        return servicio.calcularDisponible();
    }
}
