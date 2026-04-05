package Controller;

import Services.EstadisticaService;

public class EstadisticaController {
    //Declaracion y Creacion de Objeto del Servicio
    public EstadisticaService servicio;
    
    //inicializacion del servicio en el contructor 
    public EstadisticaController(){
        servicio = new EstadisticaService ();
    }
    
    //Metodos (Views)
    public double balance (double ingresos, double gastos){
        //Recibe los datos ingresado en la vista, llama al objeto servicio y entrega el resultado 
        //la vista envia Datos -> Se los pasa al servicio -> Entrega un Resultado
        return servicio.balance(ingresos, gastos);
    }
    
    public double porcentaje (double gastos, double ingresos){
        return servicio.porcentaje(gastos, ingresos);
    }
    
    public double promedio (double total, int cantida){
        return servicio.promedio(total, cantida);
    }
    
    
}
