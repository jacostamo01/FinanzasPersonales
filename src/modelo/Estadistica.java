package modelo;

public class Estadistica {
    //Atributos (Private / Encapsulamiento)
    private double totalIngresos;
    private double totalGastos;
    
    //Contructor con valores iniciales
    public Estadistica (double totalIngresos, double totalGastos){
        //This hace referencia al atributo de la clase
        this.totalIngresos = totalIngresos;
        this.totalGastos = totalGastos;
    }
    
    //Getters Ingreso de los valores
    public double getTotalIngresos (){
        //Retorne el valor ingresado
        return totalIngresos;
    }
    
    public double getTotalGastos (){
        return totalGastos;
    }
}

