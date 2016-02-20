package snake;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;

import javax.swing.JFrame;
import javax.swing.JPanel;

public class GameSwt extends JPanel {
	
	private Game game = new Game();
	
	public GameSwt(){
		setFocusable(true);
		setBackground(Color.CYAN);
		setVisible(true);
		setLocation(0, 0);
		setSize(500, 500);
		addMouseListener(new MouseListener() {
			
			@Override
			public void mouseReleased(MouseEvent e) {}
			
			@Override
			public void mousePressed(MouseEvent e) {}
			
			@Override
			public void mouseExited(MouseEvent e) {}
			
			@Override
			public void mouseEntered(MouseEvent e) {}
			
			@Override
			public void mouseClicked(MouseEvent e) {
				game.start();
			}
		});
		
		addKeyListener(new KeyListener() {
			
			@Override
			public void keyReleased(KeyEvent e) {hintKey(e);}
			@Override
			public void keyPressed(KeyEvent e) {hintKey(e);}

			@Override
			public void keyTyped(KeyEvent e) {hintKey(e);}

			void hintKey(KeyEvent e){
				switch (e.getKeyCode()) {
				case KeyEvent.VK_DOWN: 	game.snake.faceToDown();	break;
				case KeyEvent.VK_UP:	game.snake.faceToUp(); 		break;
				case KeyEvent.VK_LEFT:	game.snake.faceToLeft(); 	break;
				case KeyEvent.VK_RIGHT:	game.snake.faceToRight();	break;
				}

			}
		});
		
	}
	public static void main(String[] args) {
		JFrame frame = new JFrame("Snake SWT");
		frame.add(new GameSwt());
		frame.setSize(500, 500);
		frame.setVisible(true);
		
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

	@Override
	public void paint(Graphics g) {
		
		
	}
	
}
