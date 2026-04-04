package servicio;

import conexion.Conexion;
import modelo.Gasto;
import modelo.Ingreso;
import modelo.Movimiento;

import java.sql.*;
import java.util.ArrayList;

public class MovimientoService {

    // guarda en la base de datos
    public void agregarIngreso(double monto, String descripcion) {
        guardar("Ingreso", monto, descripcion);
    }

    public void agregarGasto(double monto, String descripcion) {
        guardar("Gasto", monto, descripcion);
    }

    private void guardar(String tipo, double monto, String descripcion) {
        Connection con = Conexion.obtenerConexion();
        if (con == null) return;

        try {
            String sql = "INSERT INTO movimientos (tipo, monto, descripcion) VALUES (?, ?, ?)";
            PreparedStatement ps = con.prepareStatement(sql);
            ps.setString(1, tipo);
            ps.setDouble(2, monto);
            ps.setString(3, descripcion);
            ps.executeUpdate();
            con.close();
        } catch (SQLException e) {
            System.out.println("Error al guardar: " + e.getMessage());
        }
    }

    // trae todos los movimientos de la base de datos
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
}