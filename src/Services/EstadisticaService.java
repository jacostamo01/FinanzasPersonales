package Services;

public class EstadisticaService {
    //OPERACIONES
    //Calculo del balance (Patrimonio) entre lo que gana, menos lo que gasta 
    public double balance (double ingresos, double gastos){
        return ingresos - gastos;
    }

    public double porcentaje (double gastos, double ingresos){
        //Condicional, en caso de que ingresos sea igual a cero evite dividir en cero
        if (ingresos == 0) return 0;
        return (gastos / ingresos) * 100;
    }
    
    public double promedio (double total, int cantidad){
        //Condicional
        if (cantidad == 0) return 0;
        return total / cantidad;
    }
    
    
}
