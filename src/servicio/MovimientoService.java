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
 * - SEPARACION DE RESPONSABILIDADES: esta clase solo maneja la BD
 * - POLIMORFISMO: usa la clase abstracta Movimiento para manejar Ingreso y Gasto
 * - TRY-WITH-RESOURCES: cierra conexiones automaticamente
 * - REUTILIZACION: crearMovimientoDesdeRS evita duplicar codigo
 */
public class MovimientoService {

    // Guarda un ingreso en la base de datos
    public boolean agregarIngreso(double monto, String descripcion, String fecha) {
        return guardar("Ingreso", monto, descripcion, fecha);
    }

    // Guarda un gasto en la base de datos
    public boolean agregarGasto(double monto, String descripcion, String fecha) {
        return guardar("Gasto", monto, descripcion, fecha);
    }

    /**
     * Metodo privado que inserta un movimiento en la BD.
     * Usa try-with-resources para cerrar conexion y statement automaticamente.
     */
    private boolean guardar(String tipo, double monto, String descripcion, String fecha) {
        String sql = "INSERT INTO movimientos (tipo, monto, descripcion, fecha) VALUES (?, ?, ?, ?)";

        try (Connection con = Conexion.obtenerConexion()) {
            if (con == null) return false;

            try (PreparedStatement ps = con.prepareStatement(sql)) {
                ps.setString(1, tipo);
                ps.setDouble(2, monto);
                ps.setString(3, descripcion);
                ps.setTimestamp(4, Timestamp.valueOf(fecha + " 00:00:00"));
                ps.executeUpdate();
                return true;
            }
        } catch (SQLException e) {
            System.out.println("Error al guardar: " + e.getMessage());
            return false;
        }
    }

    /**
     * Trae todos los movimientos de la base de datos.
     * POLIMORFISMO: crea Ingreso o Gasto segun el tipo.
     */
    public ArrayList<Movimiento> listarMovimientos() {
        ArrayList<Movimiento> lista = new ArrayList<>();

        try (Connection con = Conexion.obtenerConexion()) {
            if (con == null) return lista;

            try (Statement st = con.createStatement();
                 ResultSet rs = st.executeQuery("SELECT * FROM movimientos")) {
                while (rs.next()) {
                    lista.add(crearMovimientoDesdeRS(rs));
                }
            }
        } catch (SQLException e) {
            System.out.println("Error al listar: " + e.getMessage());
        }

        return lista;
    }

    /**
     * Agrupa los movimientos por mes (NO CRUD: solo consulta).
     * Retorna una lista donde cada elemento es {periodo, totalIngresos, totalGastos}.
     */
    public ArrayList<Object[]> listarPorMes() {
        ArrayList<Object[]> lista = new ArrayList<>();
        String sql = "SELECT DATE_FORMAT(fecha, '%Y-%m') as periodo, " +
                "SUM(CASE WHEN tipo='Ingreso' THEN monto ELSE 0 END) as ingresos, " +
                "SUM(CASE WHEN tipo='Gasto' THEN monto ELSE 0 END) as gastos " +
                "FROM movimientos GROUP BY periodo ORDER BY periodo DESC";

        try (Connection con = Conexion.obtenerConexion()) {
            if (con == null) return lista;

            try (Statement st = con.createStatement();
                 ResultSet rs = st.executeQuery(sql)) {
                while (rs.next()) {
                    lista.add(new Object[]{
                        rs.getString("periodo"),
                        rs.getDouble("ingresos"),
                        rs.getDouble("gastos")
                    });
                }
            }
        } catch (SQLException e) {
            System.out.println("Error al listar por mes: " + e.getMessage());
        }

        return lista;
    }

    /**
     * Trae los movimientos de un mes especifico (NO CRUD: solo consulta).
     * Recibe periodo en formato 'YYYY-MM'.
     */
    public ArrayList<Movimiento> listarMovimientosPorMes(String periodo) {
        ArrayList<Movimiento> lista = new ArrayList<>();
        String sql = "SELECT * FROM movimientos WHERE DATE_FORMAT(fecha, '%Y-%m') = ? ORDER BY fecha DESC";

        try (Connection con = Conexion.obtenerConexion()) {
            if (con == null) return lista;

            try (PreparedStatement ps = con.prepareStatement(sql)) {
                ps.setString(1, periodo);
                try (ResultSet rs = ps.executeQuery()) {
                    while (rs.next()) {
                        lista.add(crearMovimientoDesdeRS(rs));
                    }
                }
            }
        } catch (SQLException e) {
            System.out.println("Error al listar por mes detallado: " + e.getMessage());
        }

        return lista;
    }

    /**
     * REUTILIZACION: metodo privado que convierte una fila del ResultSet
     * en un objeto Movimiento (Ingreso o Gasto segun el tipo).
     * Evita duplicar el mismo codigo en listarMovimientos y listarMovimientosPorMes.
     */
    private Movimiento crearMovimientoDesdeRS(ResultSet rs) throws SQLException {
        String tipo = rs.getString("tipo");
        double monto = rs.getDouble("monto");
        String descripcion = rs.getString("descripcion");

        // POLIMORFISMO: se crea Ingreso o Gasto, pero se retorna como Movimiento
        Movimiento m = tipo.equals("Ingreso")
                ? new Ingreso(monto, descripcion)
                : new Gasto(monto, descripcion);
        m.setId(rs.getInt("id"));
        return m;
    }
}