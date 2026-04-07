package controlador;

import modelo.Movimiento;
import servicio.MovimientoService;
import java.util.ArrayList;

/**
 * CONTROLADOR del modulo de movimientos (ingresos y gastos).
 * Es el intermediario entre la Vista y el Servicio.
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
    public boolean agregarIngreso(double monto, String descripcion) {
        return servicio.agregarIngreso(monto, descripcion);
    }

    // Agrega un gasto (retorna true si se guardo en BD)
    public boolean agregarGasto(double monto, String descripcion) {
        return servicio.agregarGasto(monto, descripcion);
    }

    // Lista todos los movimientos desde la BD
    public ArrayList<Movimiento> listarMovimientos() {
        return servicio.listarMovimientos();
    }

    // Elimina todos los registros de la BD
    public boolean eliminarTodos() {
        return servicio.eliminarTodos();
    }
}