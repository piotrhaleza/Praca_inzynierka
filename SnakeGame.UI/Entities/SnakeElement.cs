using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame.UI.Entities
{
    public class SnakeElement : GameEntity
    {
        public SnakeElement(int size, bool isHead = false)
        {
            UIElement = new Rectangle
            {
                Width = size - 4,
                Height = size - 4,
                Fill = isHead? Brushes.Blue : Brushes.Green
            };
            Size = size;
        }
      

    
        public bool IsHead {
            
            get => isHead; 
            set
            {
                isHead = value;
                (UIElement as Rectangle).Fill = isHead ? Brushes.Blue : Brushes.Green;
            } 
        
        
        }
        private bool isHead;

    }
}
