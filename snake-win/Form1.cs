using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2 {
    public partial class Form1 : Form {
        static Graphics g;
        Game game = null;

        public Form1() {
            InitializeComponent();

            game = new Game(this) {
                score = 0
            };
        }
        protected override void OnResize(EventArgs e) {
            if(game!=null)game.g = Graphics.FromHwnd(this.Handle);
            base.OnResize(e);
        }
        protected override void OnClick(EventArgs e) {
            if(game.gameStoped) game.play();
            base.OnClick(e);
        }
        protected override void OnClosed(EventArgs e) {
            game.gameStoped = true;
            base.OnClosed(e);
        }
        protected override void OnKeyUp(KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Down:
                    game.snake.turnDown();
                    break;
                case Keys.Up:
                    game.snake.turnUp();
                    break;
                case Keys.Right:
                    game.snake.turnRight();
                    break;
                case Keys.Left:
                    game.snake.turnLeft();
                    break;
                default:
                    break;
            }
            base.OnKeyUp(e);
        }
    }
}
