using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Helpers
{
    public static class AppleHelper
    {
        public static Apple CreateApple(Snake Snake, int elementSize, int rowCount, int columnCount, GameDifficulty difficulty)
        {
            if (difficulty == GameDifficulty.Hard)
                return CreateHardApple(Snake, elementSize, rowCount, columnCount);
            else if(difficulty == GameDifficulty.Medium)
                return CreateMediumApple(Snake, elementSize, rowCount, columnCount);
            else
                return CreateEasyApple(Snake, elementSize, rowCount, columnCount);
        }

        public static Apple CreateHardApple(Snake Snake, int elementSize, int rowCount, int columnCount)
        {
            Random random = new Random();

            var x = random.Next(1, rowCount - 1);
            var y = random.Next(1, columnCount - 1);

            while (Snake.Elements.Any(z => z.X - 1 == x && z.Y - 1 == y))
            {
                x++;
                if (x == 9)
                {
                    y++;
                    x = 1;
                }
                if (y == 9)
                {
                    y = 1;
                }

            }
            return new Apple(elementSize)
            {
                RealX = x * elementSize,
                RealY = y * elementSize
            };
        }
        public static Apple CreateEasyApple(Snake Snake, int elementSize, int rowCount, int columnCount)
        {
            Random random = new Random();

            List<(int x, int y)> possibleValues = new List<(int, int)>()
            {
                (1,5),
                (5,1),
                (5,8),
                (8,5),
            };

            var index = random.Next(possibleValues.Count());

            var x = possibleValues[index].x;
            var y = possibleValues[index].y;

            return new Apple(elementSize)
            {
                RealX = x * elementSize,
                RealY = y * elementSize
            };
        }
        public static Apple CreateMediumApple(Snake Snake, int elementSize, int rowCount, int columnCount)
        {
            Random random = new Random();

            List<(int x, int y)> possibleValues = new List<(int, int)>()
            {
                (1,5),
                (2,2),
                (8,8),
                (2,8),
                (4,6),
                (5,1),
                (5,9),
                (9,5),
            };

            var index = random.Next(possibleValues.Count());

            var x = possibleValues[index].x;
            var y = possibleValues[index].y;

            return new Apple(elementSize)
            {
                RealX = x * elementSize,
                RealY = y * elementSize
            };
        }

    }
}
