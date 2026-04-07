import javax.swing.SwingUtilities;
import vista.ViewLogin;

/**
 * Clase principal - punto de entrada del programa.
 * Lanza la ventana de Login usando SwingUtilities para que
 * la interfaz grafica se ejecute en el hilo correcto (EDT).
 */
public class Main {
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            new ViewLogin().setVisible(true);
        });
    }
}