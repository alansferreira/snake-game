using System.Drawing;

namespace WindowsFormsApplication2 {
    public class Grid {
        public Pen fruitPen;
        public Point gridPosition;
        public Game game;

        public Grid(Game game) {
            this.game = game;
        }

        public int cellH { get; set; }
        public int cells { get; set; }
        public int cellW { get; set; }
        public Pen pen { get; set; }
        public object[,] slots { get; set; }
    }
}