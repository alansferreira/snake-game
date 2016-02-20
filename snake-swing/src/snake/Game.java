package snake;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;

public class Game {
	public int score = 0;
	public Snake snake = new Snake();
	public Grid grid = new Grid();
	public Fruit fruit = new Fruit();
	public boolean isRunning = false;
	
	
	private Random rnd = new Random();
//	private Thread walker = new Thread(new Runnable() {
//		
//		@Override
//		public void run() {
//			while(isRunning){
//				
//				walk();
//				
//				Thread.yield();
//				try {Thread.sleep(2000);} catch (InterruptedException e) {}
//			}
//			
//		}
//	});
	
	public void walk(){
		SimplePoint snakeHead = snake.trail.get(0);
	    if (fruit.position.X == snakeHead.X + snake.stepH && fruit.position.Y == snakeHead.Y + snake.stepV) {
	        snake.trail.add(new SimplePoint(fruit.position.X + snake.stepH, fruit.position.Y + snake.stepV ));
	        score += 1;
	        if (snake.trail.size() >= grid.cells * grid.cells) {
	            gameOver();
	        }
	        while (true) {
	            fruit.position = new SimplePoint(rnd.nextInt(grid.cells), rnd.nextInt(grid.cells));
	            boolean isFreeCell = true;

	            for (SimplePoint trail : snake.trail) {
	            	if (fruit.position.X != trail.X || fruit.position.Y != trail.Y) continue;
	            	
	            	isFreeCell = false;
	            	break;
					
				}

	            if (isFreeCell) break;
	        }

	        return;
	    } 

	    System.out.println(String.format("X: %1s, Y: %2s ", snake.trail.get(0).X + snake.stepH, snake.trail.get(0).Y + snake.stepV));
	    snake.trail.get(snake.trail.size() - 1).X = snake.trail.get(0).X + snake.stepH;
	    snake.trail.get(snake.trail.size() - 1).Y = snake.trail.get(0).Y + snake.stepV;

	    //snake.tail[0][0] = snake.tail[0][0] + snake.stepH;
	    //snake.tail[0][1] = snake.tail[0][1] + snake.stepV;
	    
	    SimplePoint last = snake.trail.remove(snake.trail.size() - 1);
	    snake.trail.add(0, last);


	}

	public void start(){
		snake.trail = new ArrayList<SimplePoint>(Arrays.asList(new SimplePoint(rnd.nextInt(grid.cells), rnd.nextInt(grid.cells))));
		
		fruit.position = new SimplePoint(rnd.nextInt(grid.cells), rnd.nextInt(grid.cells));
		isRunning = true;
		
		while (isRunning) {

			walk();

			try {Thread.sleep(100);} catch (InterruptedException e) {}
		}
		
	}
	private void gameOver() {
		isRunning = false;
		//walker.interrupt();
	}

}
