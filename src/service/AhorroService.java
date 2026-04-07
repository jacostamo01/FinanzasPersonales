package service;

import model.Ahorro;

public class AhorroService {

    public void guardarDinero(Ahorro ahorro, double monto) {
        ahorro.depositar(monto);
    }

    public boolean retirarDinero(Ahorro ahorro, double monto) {
        return ahorro.retirar(monto);
    }

    public double consultarSaldo(Ahorro ahorro) {
        return ahorro.getSaldo();
    }
}