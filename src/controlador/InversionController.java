package controlador;

import modelo.Inversion;
import servicio.InversionService;

/**
 * CONTROLADOR del modulo de inversiones.
 * Conecta la Vista (InversionView) con el Servicio (InversionService).
 *
 * Patron MVC:
 * - InversionView llama a este controlador
 * - Este controlador llama a InversionService
 * - InversionService delega al modelo Inversion
 */
public class InversionController {

    private InversionService servicio;

    public InversionController() {
        this.servicio = new InversionService();
    }

    // Calcula la ganancia con interes simple
    public double calcularGanancia(Inversion inversion) {
        return servicio.calcularGanancia(inversion);
    }

    // Calcula el monto total (capital + ganancia)
    public double calcularMontoTotal(Inversion inversion) {
        return servicio.calcularMontoTotal(inversion);
    }
}
