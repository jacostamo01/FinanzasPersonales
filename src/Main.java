import javax.swing.SwingUtilities;
import vista.ViewLogin;

public class Main {
    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            new ViewLogin().setVisible(true);
        });
    }
}