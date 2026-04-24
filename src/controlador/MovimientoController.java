package controlador;

import modelo.Movimiento;
import servicio.MovimientoService;
import java.util.ArrayList;

/**
 * CONTROLADOR del modulo de movimientos (ingresos y gastos).
 *
 * Patron MVC (Modelo-Vista-Controlador):
 * - La Vista (ViewMovimientos) llama al Controlador
 * - El Controlador llama al Servicio (MovimientoService)
 * - El Servicio se comunica con la Base de Datos
 */
public class MovimientoController {

    // COMPOSICION: el controlador "tiene" un servicio
    private MovimientoService servicio;

    public MovimientoController() {
        this.servicio = new MovimientoService();
    }

    // Agrega un ingreso (retorna true si se guardo en BD)
    public boolean agregarIngreso(double monto, String descripcion, String fecha) {
        return servicio.agregarIngreso(monto, descripcion, fecha);
    }

    // Agrega un gasto (retorna true si se guardo en BD)
    public boolean agregarGasto(double monto, String descripcion, String fecha) {
        return servicio.agregarGasto(monto, descripcion, fecha);
    }

    // Lista todos los movimientos desde la BD
    public ArrayList<Movimiento> listarMovimientos() {
        return servicio.listarMovimientos();
    }

    // Agrupa movimientos por mes (NO CRUD: solo consulta)
    public ArrayList<Object[]> listarPorMes() {
        return servicio.listarPorMes();
    }

    // Lista movimientos detallados de un mes especifico
    public ArrayList<Movimiento> listarMovimientosPorMes(String periodo) {
        return servicio.listarMovimientosPorMes(periodo);
    }
}