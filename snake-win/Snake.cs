using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApplication2 {
    public class Snake {
        public Game game;

        public Snake(Game game) {
            this.game = game;
        }


        public Point headPosition;
        public int size;
        public int stepH;
        public int stepV;
        public List<int[]> tail;

        public void move() {
            var snake = this;

            
            if (game.fruit[0] == snake.tail[0][0] + snake.stepH && game.fruit[1] == snake.tail[0][1] + snake.stepV) {
                snake.tail.Add(new int[] { game.fruit[0], game.fruit[1] });
                game.score += 1;
                if (snake.tail.Count >= game.grid.cells * game.grid.cells) {game.gameOver();}

                while (true) {
                    game.fruit = new int[] { game.rnd.Next(0, game.grid.cells), game.rnd.Next(0, game.grid.cells) };
                    bool isFreeCell = true;

                    foreach (var trail in snake.tail) {
                        if (game.fruit[0] == trail[0] && game.fruit[1] == trail[1]) {
                            isFreeCell = false;
                            break;
                        }
                    }
                    if (isFreeCell) break;
                }
            }

            snake.tail[snake.tail.Count - 1][0] = snake.tail[0][0] + snake.stepH;
            snake.tail[snake.tail.Count - 1][1] = snake.tail[0][1] + snake.stepV;

            //snake.tail[0][0] = snake.tail[0][0] + snake.stepH;
            //snake.tail[0][1] = snake.tail[0][1] + snake.stepV;

            var last = snake.tail[snake.tail.Count - 1];

            snake.tail.RemoveAt(snake.tail.Count - 1);
            snake.tail.Insert(0, last);
            
        }

        public void turnDown() { this.stepV = 1; this.stepH = 0; }
        public void turnUp() { this.stepV = -1; this.stepH = 0; }
        public void turnRight() { this.stepV = 0; this.stepH = 1; }
        public void turnLeft() { this.stepV = 0; this.stepH = -1; }



}
}