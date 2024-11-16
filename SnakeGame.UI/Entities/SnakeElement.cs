using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.UI.Entities
{
    public class SnakeElement : GameEntity
    {
        public static bool UiIs = true;

        public SnakeElement(int size, bool isHead = false)
        {
            if (UiIs)
                UIElement = new Rectangle
                {
                    Width = size - 4,
                    Height = size - 4,
                    Fill = isHead ? new SolidColorBrush(Color.FromArgb(255, 122, 0, 255)) :  new SolidColorBrush(Color.FromArgb(255, 142, 145, 255))
                };
            Size = size;
        }



        public bool IsHead
        {

            get => isHead;
            set
            {
                isHead = value;
                if (UiIs)
                    (UIElement as Rectangle).Fill = isHead ? new SolidColorBrush(Color.FromArgb(255, 122, 0, 255)) : new SolidColorBrush(Color.FromArgb(255, 142, 145, 255));
            }


        }
        private bool isHead;

    }
}
