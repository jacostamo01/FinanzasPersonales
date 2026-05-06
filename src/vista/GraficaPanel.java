package vista;

import javax.swing.*;
import java.awt.*;

/**
 * Panel que dibuja un grafico de barras agrupadas.
 * Muestra ingresos (verde) y gastos (rojo) por periodo usando Graphics2D.
 *
 * Conceptos de POO usados:
 * - HERENCIA: extiende JPanel y sobreescribe paintComponent
 * - POLIMORFISMO: el metodo paintComponent se ejecuta automaticamente por Swing
 * - ENCAPSULAMIENTO: los datos son privados
 */
public class GraficaPanel extends JPanel {

    // Datos privados (ENCAPSULAMIENTO)
    private String[] etiquetas;
    private double[] ingresos;
    private double[] gastos;

    // Constructor - recibe los datos a graficar
    public GraficaPanel(String[] etiquetas, double[] ingresos, double[] gastos) {
        this.etiquetas = etiquetas;
        this.ingresos = ingresos;
        this.gastos = gastos;
        setBackground(EstiloApp.TARJETA);
        setPreferredSize(new Dimension(500, 250));
    }

    // POLIMORFISMO: sobreescribe el metodo de JPanel para dibujar las barras
    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);
        Graphics2D g2 = (Graphics2D) g;
        g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

        if (etiquetas == null || etiquetas.length == 0) {
            g2.setFont(EstiloApp.FUENTE_CUERPO);
            g2.setColor(EstiloApp.GRIS);
            g2.drawString("No hay datos para graficar", getWidth() / 2 - 90, getHeight() / 2);
            return;
        }

        int margenIzq = 70;
        int margenDer = 20;
        int margenSup = 20;
        int margenInf = 55;
        int anchoGrafica = getWidth() - margenIzq - margenDer;
        int altoGrafica = getHeight() - margenSup - margenInf;

        // Calcular el valor maximo para escalar las barras
        double maxValor = 1;
        for (int i = 0; i < etiquetas.length; i++) {
            if (ingresos[i] > maxValor) maxValor = ingresos[i];
            if (gastos[i] > maxValor) maxValor = gastos[i];
        }

        // Dibujar ejes
        g2.setColor(EstiloApp.TEXTO_SEC);
        g2.setStroke(new BasicStroke(2));
        g2.drawLine(margenIzq, margenSup, margenIzq, margenSup + altoGrafica);
        g2.drawLine(margenIzq, margenSup + altoGrafica,
                margenIzq + anchoGrafica, margenSup + altoGrafica);

        // Dibujar lineas horizontales de referencia
        g2.setStroke(new BasicStroke(1));
        g2.setFont(new Font("Segoe UI", Font.PLAIN, 10));
        for (int i = 0; i <= 4; i++) {
            int y = margenSup + altoGrafica - (altoGrafica * i / 4);
            g2.setColor(EstiloApp.BORDE);
            g2.drawLine(margenIzq + 1, y, margenIzq + anchoGrafica, y);
            g2.setColor(EstiloApp.TEXTO_SEC);
            g2.drawString(formatearCorto(maxValor * i / 4), 5, y + 4);
        }

        // Dibujar barras agrupadas (verde = ingresos, rojo = gastos)
        int numGrupos = etiquetas.length;
        int anchoGrupo = anchoGrafica / numGrupos;
        int anchoBarra = Math.min(anchoGrupo / 3, 40);

        for (int i = 0; i < numGrupos; i++) {
            int xGrupo = margenIzq + i * anchoGrupo + anchoGrupo / 2;

            // Barra de ingresos (verde)
            int altoIng = (int) (ingresos[i] / maxValor * altoGrafica);
            g2.setColor(EstiloApp.EXITO);
            g2.fillRoundRect(xGrupo - anchoBarra - 2,
                    margenSup + altoGrafica - altoIng, anchoBarra, altoIng, 4, 4);

            // Barra de gastos (roja)
            int altoGas = (int) (gastos[i] / maxValor * altoGrafica);
            g2.setColor(EstiloApp.PELIGRO);
            g2.fillRoundRect(xGrupo + 2,
                    margenSup + altoGrafica - altoGas, anchoBarra, altoGas, 4, 4);

            // Etiqueta del periodo
            g2.setColor(EstiloApp.TEXTO);
            g2.setFont(new Font("Segoe UI", Font.BOLD, 11));
            FontMetrics fm = g2.getFontMetrics();
            int anchoTexto = fm.stringWidth(etiquetas[i]);
            g2.drawString(etiquetas[i], xGrupo - anchoTexto / 2,
                    margenSup + altoGrafica + 18);
        }

        // Leyenda
        int leyendaY = margenSup + altoGrafica + 35;
        g2.setFont(new Font("Segoe UI", Font.BOLD, 12));
        int centroX = margenIzq + anchoGrafica / 2;

        g2.setColor(EstiloApp.EXITO);
        g2.fillRect(centroX - 100, leyendaY, 12, 12);
        g2.setColor(EstiloApp.TEXTO);
        g2.drawString("Ingresos", centroX - 84, leyendaY + 11);

        g2.setColor(EstiloApp.PELIGRO);
        g2.fillRect(centroX + 10, leyendaY, 12, 12);
        g2.setColor(EstiloApp.TEXTO);
        g2.drawString("Gastos", centroX + 26, leyendaY + 11);
    }

    // Formatea un numero grande de forma corta (ej: 1.5M, 500K)
    private String formatearCorto(double valor) {
        if (valor >= 1_000_000) return String.format("%.1fM", valor / 1_000_000);
        if (valor >= 1_000) return String.format("%.0fK", valor / 1_000);
        return String.format("%.0f", valor);
    }
}
