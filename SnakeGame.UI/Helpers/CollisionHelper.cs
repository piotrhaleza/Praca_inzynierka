using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SnakeGame.UI.Helpers
{

    public static class CollisionHelper
    {
        public static int MaxRepeat = 200;

        public static KindOfCollision SnakeHadCollision(Snake snake, Apple apple, int elementSize, int turn)
        {
            if (snake != null)
            {
                if (turn > MaxRepeat)
                    return KindOfCollision.TimeCollision;
                if (CollisionWithApple(snake, apple))
                    return KindOfCollision.AppleCollision;
                if ( snake.CollisionWithSelf())
                    return KindOfCollision.SnakeCollision;
                if (CollisionWithWorldBoundsGenetics(snake, elementSize))
                    return KindOfCollision.WallCollision;
            }
            return KindOfCollision.NoCollision;
        }

        public static bool CollisionWithWorldBounds(Snake Snake,int elementSize)
        {
            if (Snake == null || Snake.Head == null)
                return false;
            var snakeHead = Snake.Head;
            return (snakeHead.RealX > GameWorld.GameAreaWidth - elementSize ||
                snakeHead.RealY > GameWorld.GameAreaHeight - elementSize ||
                snakeHead.RealX < 0 || snakeHead.RealY < 0);
        }
        public static bool CollisionWithWorldBoundsGenetics(Snake Snake, int elementSize)
        {
            if (Snake == null || Snake.Head == null)
                return false;
            var snakeHead = Snake.Head;
            return (snakeHead.X > 10 ||
                snakeHead.Y > 10 ||
                snakeHead.X < 0 || snakeHead.Y < 0);
        }

        public static bool CollisionWithApple(Snake snake, Apple apple)
        {
            if (apple == null || snake == null || snake.Head == null)
                return false;
            SnakeElement head = snake.Head;
            return (head.RealX == apple.RealX && head.RealY == apple.RealY);
        }
    }
}
