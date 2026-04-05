package controller;

import model.Inversion;
import service.InversionService;

public class InversionController {

    private InversionService service;

    public InversionController() {
        this.service = new InversionService();
    }

    public void mostrarResumen(Inversion inversion) {
        System.out.println("Ganancia: " + service.calcularGanancia(inversion));
        System.out.println("Total: " + service.calcularMontoTotal(inversion));
    }

    public double calcularGanancia(Inversion inversion) {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }

    public double calcularMontoTotal(Inversion inversion) {
        throw new UnsupportedOperationException("Not supported yet."); // Generated from nbfs://nbhost/SystemFileSystem/Templates/Classes/Code/GeneratedMethodBody
    }
}