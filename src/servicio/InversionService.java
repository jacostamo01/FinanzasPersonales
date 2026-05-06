package servicio;

import modelo.Inversion;

/**
 * Capa de SERVICIO para el modulo de inversiones.
 * Delega los calculos al modelo Inversion.
 *
 * Conceptos de POO usados:
 * - DELEGACION: no calcula directamente, le pide al objeto Inversion que lo haga
 * - SEPARACION DE CAPAS: la vista llama al controlador, el controlador al servicio,
 *   y el servicio al modelo
 */
public class InversionService {

    // Calcula la ganancia con interes simple
    public double calcularGanancia(Inversion inversion) {
        return inversion.calcularGananciaSimple();
    }

    // Calcula el monto total (capital + ganancia)
    public double calcularMontoTotal(Inversion inversion) {
        return inversion.calcularMontoTotal();
    }
}
