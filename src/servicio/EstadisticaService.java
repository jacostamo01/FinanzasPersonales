package servicio;

import modelo.Calculadora;
import modelo.Movimiento;
import java.util.ArrayList;

/**
 * Calcula totales, balances y promedios usando datos de la BD.
 *
 * Conceptos de POO usados:
 * - COMPOSICION: usa objetos de Calculadora y MovimientoService
 * - SEPARACION DE RESPONSABILIDADES: solo calcula, no muestra nada en pantalla
 * - REUTILIZACION: calcularTotal() evita duplicar logica para ingresos y gastos
 */
public class EstadisticaService {

    // COMPOSICION: esta clase "tiene" una Calculadora y un MovimientoService
    private Calculadora calculadora;
    private MovimientoService movimientoService;

    // Constructor - crea las dependencias que necesita
    public EstadisticaService() {
        this.calculadora = new Calculadora();
        this.movimientoService = new MovimientoService();
    }

    /**
     * REUTILIZACION: metodo privado que suma montos filtrados por tipo.
     * Evita duplicar la misma logica para ingresos y gastos.
     */
    private double calcularTotal(String tipo, ArrayList<Movimiento> movimientos) {
        double total = 0;
        for (Movimiento m : movimientos) {
            if (m.getTipo().equals(tipo)) {
                total = calculadora.sumar(total, m.getMonto());
            }
        }
        return total;
    }

    // Calcula ingresos y gastos con UNA sola consulta a la BD
    public double calcularTotalIngresos() {
        return calcularTotal("Ingreso", movimientoService.listarMovimientos());
    }

    public double calcularTotalGastos() {
        return calcularTotal("Gasto", movimientoService.listarMovimientos());
    }

    // Balance = ingresos - gastos (usa Calculadora.restar)
    public double balance(double ingresos, double gastos) {
        return calculadora.restar(ingresos, gastos);
    }

    // Que porcentaje de los ingresos se gasta
    public double porcentaje(double gastos, double ingresos) {
        return calculadora.porcentaje(gastos, ingresos);
    }

    // Promedio por movimiento
    public double promedio(double total, int cantidad) {
        return calculadora.promedio(total, cantidad);
    }

    // Cuenta cuantos movimientos hay en total
    public int contarMovimientos() {
        return movimientoService.listarMovimientos().size();
    }
}
