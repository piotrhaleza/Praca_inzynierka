using System.Windows;

namespace SnakeGame.UI.Entities
{
    public class GameEntity
    {
        public UIElement UIElement { get; set; }
        public int RealX { get; set; }
        public int RealY { get; set; }
        public int Size { get; set; }
        public int X => RealX / Size + 1;
        public int Y => RealY / Size + 1;
    }
}
