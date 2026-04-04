package modelo;

public abstract class Movimiento {

    private int id;
    private double monto;
    private String descripcion;

    public Movimiento(double monto, String descripcion) {
        this.monto = monto;
        this.descripcion = descripcion;
    }

    public int getId() { return id; }
    public void setId(int id) { this.id = id; }
    public double getMonto() { return monto; }
    public String getDescripcion() { return descripcion; }

    public abstract String getTipo();
}
