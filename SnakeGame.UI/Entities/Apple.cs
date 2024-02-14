using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.UI.Entities
{
    public class Apple : GameEntity
    {
        public Apple(int size)
        {
            Rectangle rect = new Rectangle
            {
                Width = size - 4,
                Height = size - 4,
                Fill = Brushes.Red ,
                RadiusX = 10,
                RadiusY = 10
            };
            Size = size;
            UIElement = rect;
        }

        public override bool Equals(object obj)
        {
            if (obj is Apple apple)
                return RealX == apple.RealX && RealY == apple.RealY;
            else
                return false;
        }

        public override int GetHashCode() => base.GetHashCode();
        public override string ToString() => base.ToString();
    }
}
