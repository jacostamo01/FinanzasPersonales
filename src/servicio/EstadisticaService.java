package servicio;

import modelo.Calculadora;
import modelo.Movimiento;
import java.util.ArrayList;

/**
 * Capa de SERVICIO para estadisticas financieras.
 * Calcula totales, balances y promedios usando datos de la BD.
 *
 * Conceptos de POO usados:
 * - COMPOSICION: usa objetos de Calculadora y MovimientoService
 *   en lugar de hacer todo aqui (reutiliza codigo)
 * - SEPARACION DE RESPONSABILIDADES: solo calcula, no muestra nada en pantalla
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

    // Trae los movimientos de la BD y suma solo los ingresos
    public double calcularTotalIngresos() {
        ArrayList<Movimiento> movimientos = movimientoService.listarMovimientos();
        double total = 0;
        for (Movimiento m : movimientos) {
            if (m.getTipo().equals("Ingreso")) {
                total = calculadora.sumar(total, m.getMonto());
            }
        }
        return total;
    }

    // Trae los movimientos de la BD y suma solo los gastos
    public double calcularTotalGastos() {
        ArrayList<Movimiento> movimientos = movimientoService.listarMovimientos();
        double total = 0;
        for (Movimiento m : movimientos) {
            if (m.getTipo().equals("Gasto")) {
                total = calculadora.sumar(total, m.getMonto());
            }
        }
        return total;
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
