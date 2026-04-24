package vista;

import javax.swing.*;
import javax.swing.border.EmptyBorder;
import java.awt.*;

/**
 * Clase utilitaria con los estilos visuales compartidos.
 * Centraliza colores, fuentes y metodos de creacion de componentes
 * para que todas las vistas tengan el mismo aspecto (tema oscuro suave).
 *
 * Conceptos de POO usados:
 * - CONSTANTES (static final): los valores no cambian
 * - METODOS ESTATICOS: se usan sin crear objeto
 * - REUTILIZACION: se usa desde todas las vistas
 */
public class EstiloApp {

    // === COLORES DEL TEMA OSCURO SUAVE ===
    public static final Color HEADER = new Color(35, 40, 48);
    public static final Color FONDO = new Color(45, 50, 58);
    public static final Color TARJETA = new Color(55, 62, 72);
    public static final Color CAMPO = new Color(62, 70, 82);
    public static final Color BORDE = new Color(75, 82, 95);
    public static final Color TEXTO = new Color(210, 215, 220);
    public static final Color TEXTO_SEC = new Color(145, 155, 165);
    public static final Color GRIS = new Color(120, 130, 140);

    // Colores de acento (desaturados, profesionales)
    public static final Color PRIMARIO = new Color(70, 130, 180);
    public static final Color EXITO = new Color(75, 145, 95);
    public static final Color PELIGRO = new Color(175, 75, 70);
    public static final Color MORADO = new Color(110, 95, 160);
    public static final Color TURQUESA = new Color(65, 150, 140);
    public static final Color NARANJA = new Color(180, 135, 70);

    // === FUENTES ===
    public static final Font FUENTE_TITULO = new Font("Segoe UI", Font.BOLD, 20);
    public static final Font FUENTE_SUBTITULO = new Font("Segoe UI", Font.BOLD, 16);
    public static final Font FUENTE_CUERPO = new Font("Segoe UI", Font.PLAIN, 14);
    public static final Font FUENTE_BOTON = new Font("Segoe UI", Font.BOLD, 13);
    public static final Font FUENTE_MONO = new Font("Consolas", Font.PLAIN, 13);

    // Crea un panel de encabezado con titulo claro y fondo oscuro
    public static JPanel crearHeader(String titulo) {
        JPanel panel = new JPanel();
        panel.setBackground(HEADER);
        panel.setBorder(BorderFactory.createEmptyBorder(14, 0, 14, 0));
        JLabel lbl = new JLabel(titulo);
        lbl.setForeground(new Color(220, 225, 230));
        lbl.setFont(new Font("Segoe UI Light", Font.PLAIN, 22));
        panel.add(lbl);
        return panel;
    }

    // Crea un boton con estilo unificado oscuro
    public static JButton crearBoton(String texto, Color color) {
        JButton btn = new JButton(texto);
        btn.setFont(FUENTE_BOTON);
        btn.setBackground(color);
        btn.setForeground(Color.WHITE);
        btn.setFocusPainted(false);
        btn.setBorderPainted(false);
        btn.setCursor(new Cursor(Cursor.HAND_CURSOR));
        return btn;
    }

    // Configura una ventana con el tema oscuro
    public static void configurarVentana(JFrame ventana, String titulo, int ancho, int alto) {
        ventana.setTitle(titulo);
        ventana.setSize(ancho, alto);
        ventana.setLocationRelativeTo(null);
        ventana.getContentPane().setBackground(FONDO);
    }

    // Aplica estilo oscuro a un JTextField
    public static void aplicarEstiloCampo(JTextField campo) {
        campo.setFont(FUENTE_CUERPO);
        campo.setBackground(CAMPO);
        campo.setForeground(TEXTO);
        campo.setCaretColor(TEXTO);
        campo.setBorder(BorderFactory.createCompoundBorder(
            BorderFactory.createLineBorder(BORDE, 1),
            new EmptyBorder(4, 8, 4, 8)));
    }

    // Aplica estilo oscuro a un JTextArea
    public static void aplicarEstiloArea(JTextArea area) {
        area.setFont(FUENTE_MONO);
        area.setBackground(CAMPO);
        area.setForeground(TEXTO);
        area.setCaretColor(TEXTO);
        area.setBorder(new EmptyBorder(8, 8, 8, 8));
    }

    // Crea un JLabel con el color de texto del tema
    public static JLabel crearLabel(String texto) {
        JLabel lbl = new JLabel(texto);
        lbl.setFont(FUENTE_CUERPO);
        lbl.setForeground(TEXTO);
        return lbl;
    }

    // REUTILIZACION: aplica estilo oscuro a cualquier JTable
    public static void aplicarEstiloTabla(JTable tabla) {
        tabla.setFont(FUENTE_CUERPO);
        tabla.setRowHeight(28);
        tabla.setBackground(TARJETA);
        tabla.setForeground(TEXTO);
        tabla.setGridColor(BORDE);
        tabla.setSelectionBackground(PRIMARIO);
        tabla.setSelectionForeground(Color.WHITE);
        tabla.getTableHeader().setFont(FUENTE_BOTON);
        tabla.getTableHeader().setBackground(HEADER);
        tabla.getTableHeader().setForeground(new Color(200, 205, 210));
    }
}
