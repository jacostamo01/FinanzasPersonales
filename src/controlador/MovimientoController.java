package controlador;

import modelo.Movimiento;
import servicio.MovimientoService;
import java.util.ArrayList;

public class MovimientoController {

    private MovimientoService servicio;

    public MovimientoController() {
        servicio = new MovimientoService();
    }

    public void agregarIngreso(double monto, String descripcion) {
        servicio.agregarIngreso(monto, descripcion);
    }

    public void agregarGasto(double monto, String descripcion) {
        servicio.agregarGasto(monto, descripcion);
    }

    public ArrayList<Movimiento> listarMovimientos() {
        return servicio.listarMovimientos();
    }
}