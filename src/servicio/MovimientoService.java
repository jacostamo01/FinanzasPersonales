package servicio;

import conexion.Conexion;
import modelo.Gasto;
import modelo.Ingreso;
import modelo.Movimiento;

import java.sql.*;
import java.util.ArrayList;

/**
 * Capa de SERVICIO para movimientos (ingresos y gastos).
 * Se encarga de la comunicacion con la base de datos.
 *
 * Conceptos de POO usados:
 * - SEPARACION DE RESPONSABILIDADES: esta clase solo maneja la BD,
 *   no la interfaz grafica ni la logica de negocio
 * - POLIMORFISMO: usa la clase abstracta Movimiento para manejar
 *   tanto Ingreso como Gasto en una misma lista
 */
public class MovimientoService {

    // Guarda un ingreso en la base de datos
    public boolean agregarIngreso(double monto, String descripcion) {
        return guardar("Ingreso", monto, descripcion);
    }

    // Guarda un gasto en la base de datos
    public boolean agregarGasto(double monto, String descripcion) {
        return guardar("Gasto", monto, descripcion);
    }

    /**
     * Metodo privado que inserta un movimiento en la BD.
     * Usa PreparedStatement para evitar inyeccion SQL.
     * Retorna true si se guardo correctamente, false si hubo error.
     */
    private boolean guardar(String tipo, double monto, String descripcion) {
        Connection con = Conexion.obtenerConexion();
        if (con == null) return false;

        try {
            String sql = "INSERT INTO movimientos (tipo, monto, descripcion) VALUES (?, ?, ?)";
            PreparedStatement ps = con.prepareStatement(sql);
            ps.setString(1, tipo);
            ps.setDouble(2, monto);
            ps.setString(3, descripcion);
            ps.executeUpdate();
            con.close();
            return true;
        } catch (SQLException e) {
            System.out.println("Error al guardar: " + e.getMessage());
            return false;
        }
    }

    /**
     * Trae todos los movimientos de la base de datos.
     * POLIMORFISMO: crea objetos Ingreso o Gasto segun el tipo,
     * pero los guarda como Movimiento en la misma lista.
     */
    public ArrayList<Movimiento> listarMovimientos() {
        ArrayList<Movimiento> lista = new ArrayList<>();
        Connection con = Conexion.obtenerConexion();
        if (con == null) return lista;

        try {
            String sql = "SELECT * FROM movimientos";
            Statement st = con.createStatement();
            ResultSet rs = st.executeQuery(sql);

            while (rs.next()) {
                String tipo = rs.getString("tipo");
                double monto = rs.getDouble("monto");
                String descripcion = rs.getString("descripcion");

                // POLIMORFISMO: se crea Ingreso o Gasto, pero se guarda como Movimiento
                Movimiento m;
                if (tipo.equals("Ingreso")) {
                    m = new Ingreso(monto, descripcion);
                } else {
                    m = new Gasto(monto, descripcion);
                }
                m.setId(rs.getInt("id"));
                lista.add(m);
            }
            con.close();
        } catch (SQLException e) {
            System.out.println("Error al listar: " + e.getMessage());
        }

        return lista;
    }

    // Elimina todos los movimientos de la base de datos
    public boolean eliminarTodos() {
        Connection con = Conexion.obtenerConexion();
        if (con == null) return false;

        try {
            String sql = "DELETE FROM movimientos";
            Statement st = con.createStatement();
            st.executeUpdate(sql);
            con.close();
            return true;
        } catch (SQLException e) {
            System.out.println("Error al eliminar: " + e.getMessage());
            return false;
        }
    }
}