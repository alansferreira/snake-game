using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2 {
    public partial class Form1 : Form {
        static Graphics g;
        static Form1 _this;
        private PaintEventHandler _currentPaint;

        PaintEventHandler CurrentPaint {
            get {
                return _currentPaint;
            }
            set {
                this.Paint -= _currentPaint;
                _currentPaint = value;
                this.Paint += _currentPaint;
            }
        }
        static Game game = new Game() {
            score = 0
        };
        static Grid grid = new Grid(){
            cellW = 10, cellH = 10,
            gridPosition = new Point(150, 150),
            pen = Pens.Black, fruitPen = Pens.Blue,
            cells = 5, 
            slots = new object[30,30]
        };
        static Snake snake = new Snake() {
            stepV = 0, stepH = 0, size = 1,
            tail = new List<int[]>(new[] { new int[] { 2, 14 } })
        };
        static Random rnd = new Random();
        static int[] fruit = new int[] { rnd.Next(0, grid.cells), rnd.Next(0, grid.cells) };
        static bool gameStoped = false;

        static ThreadStart gameThreadStart = new ThreadStart(() => {
            gameStoped = false;
            snake.tail = new List<int[]>(new[] { new int[] { rnd.Next(0, grid.cells), rnd.Next(0, grid.cells) } });
            _this.CurrentPaint = _this.paintGame;
            while (!gameStoped) {
                var t = new Task(() => { _this.snakeHeadMovement(); });
                t.Start();
                Thread.Sleep(100);
            }
        });
        Thread gameThread;


        public void snakeHeadMovement(){


            if (fruit[0] == snake.tail[0][0] + snake.stepH && fruit[1] == snake.tail[0][1] + snake.stepV) {
                snake.tail.Add(new int[] { fruit[0] + snake.stepH, fruit[1] + snake.stepV });
                game.score += 1;
                if (snake.tail.Count >= grid.cells * grid.cells) {
                    gameOver();
                }
                while (true) {
                    fruit = new int[] { rnd.Next(0, grid.cells), rnd.Next(0, grid.cells) };
                    bool isFreeCell = true;
                    
                    foreach (var trail in snake.tail) {
                        if (fruit[0] == trail[0] && fruit[1] == trail[1]) {
                            isFreeCell = false;
                            break;
                        }
                    }
                    if (isFreeCell) break;
                }

                return;
            } 

            snake.tail[snake.tail.Count - 1][0] = snake.tail[0][0] + snake.stepH;
            snake.tail[snake.tail.Count - 1][1] = snake.tail[0][1] + snake.stepV;

            //snake.tail[0][0] = snake.tail[0][0] + snake.stepH;
            //snake.tail[0][1] = snake.tail[0][1] + snake.stepV;

            var last = snake.tail[snake.tail.Count - 1];
                
            snake.tail.RemoveAt(snake.tail.Count-1);
            snake.tail.Insert(0, last);
            

            this.Invalidate();
        }

        public void paintGame (object o, PaintEventArgs e) {
            var f = (Form)o;
            var g = e.Graphics;
            
            g.Clear(f.BackColor);

            for (int i = 0; i < grid.cells; i++) {
                // Vertical
                g.DrawLine(grid.pen, i * grid.cellW + grid.gridPosition.X, grid.gridPosition.Y, i * grid.cellH + grid.gridPosition.X, grid.cells * grid.cellH + grid.gridPosition.Y);
                // Horizontal
                g.DrawLine(grid.pen, grid.gridPosition.X, i * grid.cellW + grid.gridPosition.Y, grid.cells * grid.cellW + grid.gridPosition.X, i * grid.cellW + grid.gridPosition.Y);
            }
            g.FillRectangle(grid.fruitPen.Brush, new Rectangle(fruit[0] * 10 + grid.gridPosition.X, fruit[1] * 10 + grid.gridPosition.Y, 10, 10));

            foreach (var tail in snake.tail) {
                g.FillRectangle(grid.pen.Brush, new Rectangle(tail[0] * 10 + grid.gridPosition.X, tail[1] * 10 + grid.gridPosition.Y, 10, 10));
            }

            g.DrawString(game.score.ToString(), f.Font, grid.pen.Brush, new PointF((float)(grid.cells * grid.cellH + 50), 50F));

            g.Flush();
        }

        public Form1() {
            InitializeComponent();
            _this = this;
            g = Graphics.FromHwnd(this.Handle);

            CurrentPaint =  startScreen;

        }

        private void startScreen(object o, PaintEventArgs e) {
            var f = (Form)o;
            gameStoped = true;

            e.Graphics.Clear(this.BackColor);
            e.Graphics.DrawString("Clique para começar", this.Font, new SolidBrush(this.ForeColor), new Point(f.Width / 2, f.Height / 2));
            e.Graphics.Flush();
        }

        protected override void OnClick(EventArgs e) {
            play();
            base.OnClick(e);
        }
        public void play() {
            gameOver();
            if(gameThread!=null) gameThread.Abort();
            gameThread = new Thread(gameThreadStart);
            gameThread.Start();
        }
        private void gameOver() {
            gameStoped = true;

            CurrentPaint = startScreen;
        }
        protected override void OnClosed(EventArgs e) {
            gameStoped = true;
            base.OnClosed(e);
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Down:
                    snake.stepV = 1; snake.stepH = 0;
                    break;
                case Keys.Up:
                    snake.stepV = -1; snake.stepH = 0;
                    break;
                case Keys.Right:
                    snake.stepV = 0; snake.stepH = 1;
                    break;
                case Keys.Left:
                    snake.stepV = 0; snake.stepH = -1;
                    break;
                default:
                    break;
            }
            base.OnKeyUp(e);
        }
    }
}
