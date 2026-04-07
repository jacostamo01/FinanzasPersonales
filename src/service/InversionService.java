package service;

import model.Inversion;

public class InversionService {

    public double calcularGanancia(Inversion inversion) {
        return inversion.calcularGananciaSimple();
    }

    public double calcularMontoTotal(Inversion inversion) {
        return inversion.calcularMontoTotal();
    }

    public double calcularInteresCompuesto(Inversion inversion) {
        return inversion.calcularInteresCompuesto();
    }
}