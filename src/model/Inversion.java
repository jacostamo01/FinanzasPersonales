package model;

public class Inversion {
    private int id;
    private int usuarioId;
    private double capital;
    private double tasaInteres; // Ej: 0.05 = 5%
    private int tiempo; // en meses o años

    public Inversion(int usuarioId, double capital, double tasaInteres, int tiempo) {
        this.usuarioId = usuarioId;
        this.capital = capital;
        this.tasaInteres = tasaInteres;
        this.tiempo = tiempo;
    }

    public double calcularGananciaSimple() {
        return capital * tasaInteres * tiempo;
    }

    public double calcularMontoTotal() {
        return capital + calcularGananciaSimple();
    }

    public double calcularInteresCompuesto() {
        return capital * Math.pow((1 + tasaInteres), tiempo);
    }

    // getters
}