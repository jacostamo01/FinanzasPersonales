package servicio;

import modelo.Ahorro;

/**
 * Capa de SERVICIO para el modulo de ahorros.
 * Valida que el usuario no ahorre mas de lo que tiene disponible.
 *
 * Conceptos de POO usados:
 * - COMPOSICION: usa EstadisticaService para obtener datos de la BD
 * - VALIDACION: verifica los montos antes de modificar el ahorro
 */
public class AhorroService {

    // COMPOSICION: depende de EstadisticaService para calcular el disponible
    private EstadisticaService estadisticaService;

    public AhorroService() {
        this.estadisticaService = new EstadisticaService();
    }

    // Calcula cuanto dinero disponible hay (ingresos - gastos desde la BD)
    public double calcularDisponible() {
        double ingresos = estadisticaService.calcularTotalIngresos();
        double gastos = estadisticaService.calcularTotalGastos();
        return ingresos - gastos;
    }

    /**
     * Intenta ahorrar un monto.
     * Valida que no sea mas de lo disponible (descontando lo ya ahorrado).
     * Retorna true si se pudo ahorrar, false si no alcanza.
     */
    public boolean guardarDinero(Ahorro ahorro, double monto) {
        double disponible = calcularDisponible();
        double limiteReal = disponible - ahorro.getSaldo();

        if (monto <= 0 || monto > limiteReal) {
            return false;
        }

        ahorro.depositar(monto);
        return true;
    }

    // Intenta retirar dinero del ahorro (delega la validacion al modelo)
    public boolean retirarDinero(Ahorro ahorro, double monto) {
        return ahorro.retirar(monto);
    }
}
