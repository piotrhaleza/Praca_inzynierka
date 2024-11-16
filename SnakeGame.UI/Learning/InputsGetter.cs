
using SnakeGame.UI;
using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace snakeGame.UI.Learning
{


    public static class InputsGetter
    {
        public static double MinValue = 0;

        public static Dictionary<SnakeInputs, double> GetInputs(Snake snake, Apple apple)
        {
            var result = new Dictionary<SnakeInputs, double>();


            ColisionSnakeDistans(snake, result);
            ColisionToAppleDistans(snake, apple, result);
            ColisionToWallDistans(snake, result);
            SpecialColsions(result);
            ConvertNumber(result);
            return result;
        }

        #region Main methods
        public static void ConvertNumber(Dictionary<SnakeInputs, double> result)
        {
            result[SnakeInputs.LeftBackToApple] = result[SnakeInputs.LeftBackToApple] > 0 ? 1 : MinValue;
            result[SnakeInputs.LeftForwardToApple] = result[SnakeInputs.LeftForwardToApple] > 0 ? 1 : MinValue;
            result[SnakeInputs.LeftToApple] = result[SnakeInputs.LeftToApple] > 0 ? 1 : MinValue;
            result[SnakeInputs.ForwardToApple] = result[SnakeInputs.ForwardToApple] > 0 ? 1 : MinValue;
            result[SnakeInputs.RightToApple] = result[SnakeInputs.RightToApple] > 0 ? 1 : MinValue;
            result[SnakeInputs.RightBackToApple] = result[SnakeInputs.RightBackToApple] > 0 ? 1 : MinValue;
            result[SnakeInputs.RightForwardToApple] = result[SnakeInputs.RightForwardToApple] > 0 ? 1 : MinValue;

            result[SnakeInputs.LeftBackToSnake] = GetCroosSnake(result[SnakeInputs.LeftBackToSnake]);
            result[SnakeInputs.LeftForwardToSnake] = GetCroosSnake(result[SnakeInputs.LeftForwardToSnake]);
            result[SnakeInputs.LeftToSnake] = GetSimpleSnake(result[SnakeInputs.LeftToSnake]);
            result[SnakeInputs.ForwardToSnake] = GetSimpleSnake(result[SnakeInputs.ForwardToSnake]);
            result[SnakeInputs.RightBackToSnake] = GetCroosSnake(result[SnakeInputs.RightBackToSnake]);
            result[SnakeInputs.RightToSnake] = GetSimpleSnake(result[SnakeInputs.RightToSnake]);
            result[SnakeInputs.RightForwardToSnake] = GetCroosSnake(result[SnakeInputs.RightForwardToSnake]);

            result[SnakeInputs.LeftToWall] = result[SnakeInputs.LeftToApple] != 0 ? 0 : result[SnakeInputs.LeftToSnake] != 0 ? 0 : result[SnakeInputs.LeftToWall] == 1 ? 0.4 : result[SnakeInputs.LeftToWall] == 0 ? 1 : MinValue;
            result[SnakeInputs.ForwardToWall] = result[SnakeInputs.ForwardToApple] != 0 ? 0 : result[SnakeInputs.ForwardToSnake] != 0 ? 0 :  result[SnakeInputs.ForwardToWall] == 1 ? 0.4 : result[SnakeInputs.ForwardToWall] == 0 ? 1 : MinValue;
            result[SnakeInputs.RightToWall] = result[SnakeInputs.RightToApple] != 0 ? 0 : result[SnakeInputs.RightToSnake] != 0 ? 0 : result[SnakeInputs.RightToWall] == 1 ? 0.4 : result[SnakeInputs.RightToWall] == 0 ? 1 : MinValue;
        }
        public static double GetSimpleSnake(double result)
        {
            if (result == -1)
                return MinValue;
            else if (result == 1)
                return 1;
            else if (result == 2)
                return 0.4;

            return 0.1;
        }

        public static double GetCroosSnake(double result)
        {
            double sqrt2 = Math.Sqrt(2);
            double twoSqrt2 = 2 * sqrt2;

            if (result == -1)
                return MinValue;

            if (result <= sqrt2)
                return 1;


            return 0.1;
        }

        public static void SpecialColsions(Dictionary<SnakeInputs, double> result)
        {
            AppleIsVisible(result);
            SnakeForward(result);
        }

        public static void ColisionSnakeDistans(Snake snake, Dictionary<SnakeInputs, double> result)
        {
            switch (snake.MovementDirection)
            {
                case MovementDirection.Up:
                    result.Add(SnakeInputs.LeftBackToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.RightForward));
                    break;
                case MovementDirection.Down:
                    result.Add(SnakeInputs.LeftBackToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.RightForward));
                    break;
                case MovementDirection.Right:
                    result.Add(SnakeInputs.LeftBackToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.RightForward));
                    break;
                case MovementDirection.Left:
                    result.Add(SnakeInputs.LeftBackToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.RightForward));
                    break;
            }
        }
        public static void ColisionToAppleDistans(Snake snake, Apple apple, Dictionary<SnakeInputs, double> result)
        {
            if (snake.Elements.Any(element => element.X == apple.X && element.Y == apple.Y))
            {
                result.Add(SnakeInputs.LeftBackToApple, 0);
                result.Add(SnakeInputs.LeftToApple, 0);
                result.Add(SnakeInputs.LeftForwardToApple, 0);
                result.Add(SnakeInputs.ForwardToApple, 0);
                result.Add(SnakeInputs.RightBackToApple, 0);
                result.Add(SnakeInputs.RightToApple, 0);
                result.Add(SnakeInputs.RightForwardToApple, 0);
                return;
            }
            switch (snake.MovementDirection)
            {
                case MovementDirection.Up:
                    result.Add(SnakeInputs.LeftBackToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.RightForward));
                    break;
                case MovementDirection.Down:
                    result.Add(SnakeInputs.LeftBackToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.RightForward));
                    break;
                case MovementDirection.Right:
                    result.Add(SnakeInputs.LeftBackToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.RightForward));
                    break;

                case MovementDirection.Left:
                    result.Add(SnakeInputs.LeftBackToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.LeftBack));
                    result.Add(SnakeInputs.LeftToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.LeftForwardToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.LeftForward));
                    result.Add(SnakeInputs.ForwardToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightBackToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.RightBack));
                    result.Add(SnakeInputs.RightToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.Right));
                    result.Add(SnakeInputs.RightForwardToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.RightForward));
                    break;
            }
        }
        public static void ColisionToWallDistans(Snake snake, Dictionary<SnakeInputs, double> result)
        {
            switch (snake.MovementDirection)
            {
                case MovementDirection.Up:
                    result.Add(SnakeInputs.LeftToWall, DistansToUpWall(snake, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToWall, DistansToUpWall(snake, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToWall, DistansToUpWall(snake, MovementDirection.Right));
                    break;
                case MovementDirection.Down:
                    result.Add(SnakeInputs.LeftToWall, DistansToDownWall(snake, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToWall, DistansToDownWall(snake, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToWall, DistansToDownWall(snake, MovementDirection.Right));
                    break;
                case MovementDirection.Right:
                    result.Add(SnakeInputs.LeftToWall, DistansToRightWall(snake, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToWall, DistansToRightWall(snake, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToWall, DistansToRightWall(snake, MovementDirection.Right));
                    break;
                case MovementDirection.Left:
                    result.Add(SnakeInputs.LeftToWall, DistansToLeftWall(snake, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToWall, DistansToLeftWall(snake, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToWall, DistansToLeftWall(snake, MovementDirection.Right));
                    break;
            }
        }
        #endregion

        #region Colision method

        private static void SnakeForward(Dictionary<SnakeInputs, double> result)
        {
        }

        private static void AppleIsVisible(Dictionary<SnakeInputs, double> result)
        {
            if (result[SnakeInputs.ForwardToSnake] > 0 && result[SnakeInputs.ForwardToApple] > result[SnakeInputs.ForwardToSnake])
                result[SnakeInputs.ForwardToApple] = 0;

            if (result[SnakeInputs.LeftToSnake] > 0 && result[SnakeInputs.LeftToApple] > result[SnakeInputs.LeftToSnake])
                result[SnakeInputs.LeftToApple] = 0;

            if (result[SnakeInputs.RightToSnake] > 0 && result[SnakeInputs.RightToApple] > result[SnakeInputs.RightToSnake])
                result[SnakeInputs.RightToApple] = 0;

        }

        private static double DistansToUpWall(Snake snake, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    return snake.Head.X - 1;
                case MovementDirection.Forward:
                    return snake.Head.Y - 1;
                case MovementDirection.Right:
                    return 10 - snake.Head.X;
            }
            return 0;
        }
        private static double DistansToDownWall(Snake snake, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    return 10 - snake.Head.X;
                case MovementDirection.Forward:
                    return 10 - snake.Head.Y;
                case MovementDirection.Right:
                    return snake.Head.X - 1;
            }
            return 0;
        }
        private static double DistansToRightWall(Snake snake, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    return snake.Head.Y - 1;
                case MovementDirection.Forward:
                    return 10 - snake.Head.X;
                case MovementDirection.Right:
                    return 10 - snake.Head.Y;
            }
            return 0;
        }
        private static double DistansToLeftWall(Snake snake, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    return 10 - snake.Head.Y;
                case MovementDirection.Forward:
                    return snake.Head.X - 1;
                case MovementDirection.Right:
                    return snake.Head.Y - 1;
            }
            return 0;
        }
        public static double ColisionLeftDistans(GameEntity head, GameEntity element, MovementDirection direct)
        {
            return ColisionLeftDistans(head, new List<GameEntity>() { element }, direct);
        }
        public static double ColisionRightDistans(GameEntity head, GameEntity element, MovementDirection direct)
        {
            return ColisionRightDistans(head, new List<GameEntity>() { element }, direct);
        }
        public static double ColisionDownDistans(GameEntity head, GameEntity element, MovementDirection direct)
        {
            return ColisionDownDistans(head, new List<GameEntity>() { element }, direct);
        }
        public static double ColisionUpDistans(GameEntity head, GameEntity element, MovementDirection direct)
        {
            return ColisionUpDistans(head, new List<GameEntity>() { element }, direct);
        }

        public static double ColisionLeftDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            IEnumerable<GameEntity> entities;

            switch (direct)
            {
                case MovementDirection.LeftBack:
                    entities = elements.Where(x => IsRightDownCorner(head, x)); break;
                case MovementDirection.LeftForward:
                    entities = elements.Where(x => IsLeftDownCorner(head, x)); break;
                case MovementDirection.RightBack:
                    entities = elements.Where(x => IsRightUpCorner(head, x)); break;
                case MovementDirection.RightForward:
                    entities = elements.Where(x => IsLeftUpCorner(head, x)); break;
                case MovementDirection.Left:
                    entities = elements.Where(x => IsDown(head, x)); break;
                case MovementDirection.Forward:
                    entities = elements.Where(x => IsLeft(head, x)); break;
                case MovementDirection.Right:
                    entities = elements.Where(x => IsUp(head, x)); break;
                default:
                    throw new Exception();
            }

            if (entities.Any())
                return entities.Min(x => Distans(x, head));
            else
                return -1;
        }
        public static double ColisionRightDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            IEnumerable<GameEntity> entities;


            switch (direct)
            {
                case MovementDirection.LeftBack:
                    entities = elements.Where(x => IsLeftUpCorner(head, x)); break;
                case MovementDirection.LeftForward:
                    entities = elements.Where(x => IsRightUpCorner(head, x)); break;
                case MovementDirection.RightBack:
                    entities = elements.Where(x => IsLeftDownCorner(head, x)); break;
                case MovementDirection.RightForward:
                    entities = elements.Where(x => IsRightDownCorner(head, x)); break;
                case MovementDirection.Left:
                    entities = elements.Where(x => IsUp(head, x)); break;
                case MovementDirection.Forward:
                    entities = elements.Where(x => IsRight(head, x)); break;
                case MovementDirection.Right:
                    entities = elements.Where(x => IsDown(head, x)); break;
                default:
                    throw new Exception();
            }

            if (entities.Any())
                return entities.Min(x => Distans(x, head));
            else
                return -1;
        }
        public static double ColisionDownDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            IEnumerable<GameEntity> entities;

            switch (direct)
            {
                case MovementDirection.LeftBack:
                    entities = elements.Where(x => IsRightUpCorner(head, x)); break;
                case MovementDirection.LeftForward:
                    entities = elements.Where(x => IsRightDownCorner(head, x)); break;
                case MovementDirection.RightBack:
                    entities = elements.Where(x => IsLeftUpCorner(head, x)); break;
                case MovementDirection.RightForward:
                    entities = elements.Where(x => IsLeftDownCorner(head, x)); break;
                case MovementDirection.Left:
                    entities = elements.Where(x => IsRight(head, x)); break;
                case MovementDirection.Forward:
                    entities = elements.Where(x => IsDown(head, x)); break;
                case MovementDirection.Right:
                    entities = elements.Where(x => IsLeft(head, x)); break;
                default:
                    throw new Exception();
            }

            if (entities.Any())
                return entities.Min(x => Distans(x, head));
            else
                return -1;
        }
        public static double ColisionUpDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            IEnumerable<GameEntity> entities;

            switch (direct)
            {
                case MovementDirection.LeftBack:
                    entities = elements.Where(x => IsLeftDownCorner(head, x)); break;
                case MovementDirection.LeftForward:
                    entities = elements.Where(x => IsLeftUpCorner(head, x)); break;
                case MovementDirection.RightBack:
                    entities = elements.Where(x => IsRightDownCorner(head, x)); break;
                case MovementDirection.RightForward:
                    entities = elements.Where(x => IsRightUpCorner(head, x)); break;
                case MovementDirection.Left:
                    entities = elements.Where(x => IsLeft(head, x)); break;
                case MovementDirection.Forward:
                    entities = elements.Where(x => IsUp(head, x)); break;
                case MovementDirection.Right:
                    entities = elements.Where(x => IsRight(head, x)); break;
                default:
                    throw new Exception();
            }
            if (entities.Any())
                return entities.Min(x => Distans(x, head));
            else
                return -1;
        }
        #endregion

        #region Helper methods
        public static double Distans(GameEntity head, GameEntity element)
        {
            return Math.Sqrt(Math.Pow(head.X - element.X, 2) + Math.Pow(head.Y - element.Y, 2));
        }
        public static bool IsDown(GameEntity head, GameEntity element)
        {
            return element.Y > head.Y && element.X == head.X;
        }
        public static bool IsUp(GameEntity head, GameEntity element)
        {
            return element.Y < head.Y && element.X == head.X;
        }
        public static bool IsRight(GameEntity head, GameEntity element)
        {
            return element.Y == head.Y && element.X > head.X;
        }
        public static bool IsLeft(GameEntity head, GameEntity element)
        {
            return element.Y == head.Y && element.X < head.X;
        }

        public static bool IsRightDownCorner(GameEntity head, GameEntity element)
        {
            if (element.Y > head.Y && element.X > head.X)
            {
                var a = Math.Abs((element.Y - head.Y) / (element.X - head.X));

                return a == 1;
            }
            else
            {
                return false;
            }
        }
        public static bool IsLeftDownCorner(GameEntity head, GameEntity element)
        {
            if (element.Y > head.Y && element.X < head.X)
            {
                var a = Math.Abs((element.Y - head.Y) / (element.X - head.X));

                return a == 1;
            }
            else
            {
                return false;
            }
        }
        public static bool IsLeftUpCorner(GameEntity head, GameEntity element)
        {
            if (element.Y < head.Y && element.X < head.X)
            {
                var a = Math.Abs((element.Y - head.Y) / (element.X - head.X));

                return a == 1;
            }
            else
            {
                return false;
            }
        }
        public static bool IsRightUpCorner(GameEntity head, GameEntity element)
        {
            if (element.Y < head.Y && element.X > head.X)
            {
                var a = Math.Abs((element.Y - head.Y) / (element.X - head.X));

                return a == 1;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

}
