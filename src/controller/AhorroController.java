package controller;

import model.Ahorro;
import service.AhorroService;

public class AhorroController {

    private AhorroService service;

    public AhorroController() {
        this.service = new AhorroService();
    }

    public void depositar(Ahorro ahorro, double monto) {
        service.guardarDinero(ahorro, monto);
    }

    public void retirar(Ahorro ahorro, double monto) {
        boolean ok = service.retirarDinero(ahorro, monto);

        if (!ok) {
            System.out.println("Fondos insuficientes");
        }
    }

    public void verSaldo(Ahorro ahorro) {
        System.out.println("Saldo actual: " + service.consultarSaldo(ahorro));
    }
}