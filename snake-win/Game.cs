using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2 {
    public class Game {
        public int score;
        public Grid grid = null;
        public Snake snake = null;

        public Random rnd = new Random();
        public int[] fruit = new int[] {};
        public bool gameStoped = true;
        public Control renderSurface;

        Thread gameThread;
        public Graphics g;

        public Color BackColor { get; private set; }
        public Font Font { get; private set; }

        public Game(Control renderSurface) {
            this.renderSurface = renderSurface;
            this.g = Graphics.FromHwnd(renderSurface.Handle);
            this.Font = renderSurface.Font;
            this.BackColor = renderSurface.BackColor;

            grid = new Grid(this) {
                cellW = 20, cellH = 20,
                gridPosition = new Point(150, 150),
                pen = Pens.Black, fruitPen = Pens.Blue,
                cells = 15
            };
            snake = new Snake(this) {
                stepV = 0, stepH = 0, size = 1,
                tail = new List<int[]>(new[] { new int[] { 2, 14 } })
            };


            rnd = new Random();
            fruit = new int[] { rnd.Next(0, grid.cells), rnd.Next(0, grid.cells) };
            gameStoped = true;


        }

        public void play() {
            gameOver();
            if (gameThread != null) gameThread.Abort();
            gameThread = new Thread(new ThreadStart(startGame));
            gameThread.Start();
        }

        private void startGame() {
            gameStoped = false;
            snake.tail = new List<int[]>(new[] { new int[] { rnd.Next(0, grid.cells), rnd.Next(0, grid.cells) } });

            //CurrentPaint = paintGame;
            while (!gameStoped) {
                var t = new Task(() => {
                    snake.move();
                    paintGame();
                });
                t.Start();
                Thread.Sleep(100);
            }
        }
    
        public void gameOver() {
            gameStoped = true;

            //CurrentPaint = startScreen;
        }
        public void paintGame() {

            g.Clear(BackColor);

            for (int i = 0; i < grid.cells; i++) {
                // Vertical
                g.DrawLine(grid.pen, i * grid.cellW + grid.gridPosition.X, grid.gridPosition.Y, i * grid.cellH + grid.gridPosition.X, grid.cells * grid.cellH + grid.gridPosition.Y);
                // Horizontal
                g.DrawLine(grid.pen, grid.gridPosition.X, i * grid.cellW + grid.gridPosition.Y, grid.cells * grid.cellW + grid.gridPosition.X, i * grid.cellW + grid.gridPosition.Y);
            }
            g.FillRectangle(grid.fruitPen.Brush, new Rectangle(fruit[0] * grid.cellW + grid.gridPosition.X, fruit[1] * grid.cellH + grid.gridPosition.Y, grid.cellW, grid.cellH));

            foreach (var tail in snake.tail) {
                g.FillRectangle(grid.pen.Brush, new Rectangle(tail[0] * grid.cellW + grid.gridPosition.X, tail[1] * grid.cellH + grid.gridPosition.Y, grid.cellW, grid.cellH));
            }

            g.DrawString(score.ToString(), Font, grid.pen.Brush, new PointF((float)(grid.cells * grid.cellH + 50), 50F));

            g.Flush();

            //renderSurface.Refresh();
        }


    }
}