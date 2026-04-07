package modelo;

public class Ingreso extends Movimiento {

    public Ingreso(double monto, String descripcion) {
        super(monto, descripcion);
    }

    @Override
    public String getTipo() {
        return "Ingreso";
    }
}