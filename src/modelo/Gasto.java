package modelo;

public class Gasto extends Movimiento {

    public Gasto(double monto, String descripcion) {
        super(monto, descripcion);
    }

    @Override
    public String getTipo() {
        return "Gasto";
    }
}