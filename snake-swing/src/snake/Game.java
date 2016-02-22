package snake;

import java.awt.Color;
import java.awt.Graphics;
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
	private Thread walker;
	public Graphics renderSurface;
	
	public Game() {}

	public void walk(){
		SimplePoint snakeHead = snake.trail.get(0);
	    if (fruit.position.X == snakeHead.X + snake.stepH && fruit.position.Y == snakeHead.Y + snake.stepV) {
	        snake.trail.add(new SimplePoint(fruit.position.X, fruit.position.Y));
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
		if(walker!=null)walker.interrupt();
		
		snake.trail = new ArrayList<SimplePoint>(Arrays.asList(new SimplePoint(rnd.nextInt(grid.cells), rnd.nextInt(grid.cells))));
		
		fruit.position = new SimplePoint(rnd.nextInt(grid.cells), rnd.nextInt(grid.cells));
		isRunning = true;

		walker = new Thread(new Runnable() {
			
			@Override
			public void run() {
				while(isRunning){
					
					walk();
					render();
					Thread.yield();
					try {Thread.sleep(100);} catch (InterruptedException e) {}
				}
			}
		});
		
		
		walker.start();
		
	}
	private void render() {
		Graphics g = this.renderSurface;
		g.clearRect(0, 0, 1000, 1000);
		g.setColor(Color.BLACK);
		for (int i = 0; i < grid.cells; i++) {
			// Vertical
			g.drawLine(i * grid.cellW + grid.gridPosition.X, grid.gridPosition.Y, i * grid.cellH + grid.gridPosition.X, grid.cells * grid.cellH + grid.gridPosition.Y);
			// Horizontal
			g.drawLine(grid.gridPosition.X, i * grid.cellW + grid.gridPosition.Y, grid.cells * grid.cellW + grid.gridPosition.X, i * grid.cellW + grid.gridPosition.Y);
		}
		g.setColor(fruit.color);
		g.fillRect(fruit.position.X * grid.cellW + grid.gridPosition.X, fruit.position.Y * grid.cellH + grid.gridPosition.Y, grid.cellW, grid.cellH);

		g.setColor(Color.BLACK);
		for (SimplePoint tail : snake.trail) {
			g.fillRect(tail.X * grid.cellW + grid.gridPosition.X, tail.Y * grid.cellH + grid.gridPosition.Y, grid.cellW, grid.cellH);
		}

		g.drawString(score + "", grid.cells * grid.cellH + 50, 50);
		
	}
	private void gameOver() {
		isRunning = false;
		//walker.interrupt();
	}

}
