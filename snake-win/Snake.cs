using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApplication2 {
    internal class Snake {
        public Snake() {
        }

        public Point headPosition;
        public int size;
        public int stepH;
        public int stepV;
        public List<int[]> tail;
    }
}