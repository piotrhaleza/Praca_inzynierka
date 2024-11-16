using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Helpers
{
    public static class Consts
    {
        public static int ElementSize => 50;
        public static int ColumnCount => 10;
        public static int RowCount => 10;
        public static double GameAreaWidth => RowCount * ElementSize;
        public static double GameAreaHeight => ColumnCount * ElementSize;
    }
}
