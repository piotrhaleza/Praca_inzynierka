
using SnakeGame.UI;
using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snakeGame.UI.Learning
{
    public static class InputsGetter
    {
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
            result[SnakeInputs.LeftToApple] = result[SnakeInputs.LeftToApple] > 0 ? 1 : 0;
            result[SnakeInputs.ForwardToApple] = result[SnakeInputs.ForwardToApple] > 0 ? 1 : 0;
            result[SnakeInputs.RightToApple] = result[SnakeInputs.RightToApple] > 0 ? 1 : 0;

            result[SnakeInputs.LeftToSnake] = result[SnakeInputs.LeftToSnake] >0 && result[SnakeInputs.LeftToSnake] <= 1 ? 1 : 0;
            result[SnakeInputs.ForwardToSnake] = result[SnakeInputs.ForwardToSnake] > 0 && result[SnakeInputs.ForwardToSnake] <= 2 ? 1 : 0;
            result[SnakeInputs.RightToSnake] = result[SnakeInputs.RightToSnake] > 0 && result[SnakeInputs.RightToSnake] <= 1 ? 1 : 0;

            result[SnakeInputs.LeftToWall] = result[SnakeInputs.LeftToWall] == 0 ? 1 : result[SnakeInputs.LeftToWall];
            result[SnakeInputs.ForwardToWall] = result[SnakeInputs.ForwardToWall] == 0 ? 1 : result[SnakeInputs.ForwardToWall];
            result[SnakeInputs.RightToWall] = result[SnakeInputs.RightToWall] == 0 ? 1 : result[SnakeInputs.RightToWall];

            result[SnakeInputs.LeftToWall] = result[SnakeInputs.LeftToWall] == 1 ? 0.5 : 0;
            result[SnakeInputs.ForwardToWall] = result[SnakeInputs.ForwardToWall] == 1 ? 0.5 : 0;
            result[SnakeInputs.RightToWall] = result[SnakeInputs.RightToWall] == 1 ? 0.5 : 0;
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
                    result.Add(SnakeInputs.LeftToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToSnake, ColisionUpDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    break;
                case MovementDirection.Down:
                    result.Add(SnakeInputs.LeftToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToSnake, ColisionDownDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    break;
                case MovementDirection.Right:
                    result.Add(SnakeInputs.LeftToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToSnake, ColisionRightDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    break;
                case MovementDirection.Left:
                    result.Add(SnakeInputs.LeftToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToSnake, ColisionLeftDistans(snake.Head, snake.GameEntities, MovementDirection.Right));
                    break;
            }
        }
        public static void ColisionToAppleDistans(Snake snake, Apple apple, Dictionary<SnakeInputs, double> result)
        {
            switch (snake.MovementDirection)
            {
                case MovementDirection.Up:
                    result.Add(SnakeInputs.LeftToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToApple, ColisionUpDistans(snake.Head, apple, MovementDirection.Right));
                    break;
                case MovementDirection.Down:
                    result.Add(SnakeInputs.LeftToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToApple, ColisionDownDistans(snake.Head, apple, MovementDirection.Right));
                    break;
                case MovementDirection.Right:
                    result.Add(SnakeInputs.LeftToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToApple, ColisionRightDistans(snake.Head, apple, MovementDirection.Right));
                    break;

                case MovementDirection.Left:
                    result.Add(SnakeInputs.LeftToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.Left));
                    result.Add(SnakeInputs.ForwardToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.Forward));
                    result.Add(SnakeInputs.RightToApple, ColisionLeftDistans(snake.Head, apple, MovementDirection.Right));
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
            if (result[SnakeInputs.ForwardToSnake] != 1 &&
                ((result[SnakeInputs.LeftToWall] == 0 || (result[SnakeInputs.LeftToSnake] > 0 && result[SnakeInputs.LeftToSnake] <= 1)) || 
                (result[SnakeInputs.RightToWall] == 0 || (result[SnakeInputs.RightToSnake] > 0 && result[SnakeInputs.RightToSnake] <= 1))))
                result[SnakeInputs.ForwardToSnake] = 10;


        }

        private static void AppleIsVisible(Dictionary<SnakeInputs, double> result)
        {
            if (result[SnakeInputs.ForwardToSnake] >0 && result[SnakeInputs.ForwardToApple] > result[SnakeInputs.ForwardToSnake])
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
            switch (direct)
            {
                case MovementDirection.Left:
                    var left = elements.Where(x => IsDown(head, x));
                    if (left.Any())
                        return left.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Forward:
                    var forward = elements.Where(x => IsLeft(head, x));
                    if (forward.Any())
                        return forward.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Right:
                    var right = elements.Where(x => IsUp(head, x));
                    if (right.Any())
                        return right.Min(x => Distans(x, head));
                    else
                        return -1;
                default:
                    throw new Exception();
            }
        }
        public static double ColisionRightDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    var left = elements.Where(x => IsUp(head, x));
                    if (left.Any())
                        return left.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Forward:
                    var forward = elements.Where(x => IsRight(head, x));
                    if (forward.Any())
                        return forward.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Right:
                    var right = elements.Where(x => IsDown(head, x));
                    if (right.Any())
                        return right.Min(x => Distans(x, head));
                    else
                        return -1;
                default:
                    throw new Exception();
            }
        }
        public static double ColisionDownDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    var left = elements.Where(x => IsRight(head, x));
                    if (left.Any())
                        return left.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Forward:
                    var forward = elements.Where(x => IsDown(head, x));
                    if (forward.Any())
                        return forward.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Right:
                    var right = elements.Where(x => IsLeft(head, x));
                    if (right.Any())
                        return right.Min(x => Distans(x, head));
                    else
                        return -1;
                default:
                    throw new Exception();
            }
        }
        public static double ColisionUpDistans(GameEntity head, List<GameEntity> elements, MovementDirection direct)
        {
            switch (direct)
            {
                case MovementDirection.Left:
                    var left = elements.Where(x => IsLeft(head, x));
                    if (left.Any())
                        return left.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Forward:
                    var forward = elements.Where(x => IsUp(head, x));
                    if (forward.Any())
                        return forward.Min(x => Distans(x, head));
                    else
                        return -1;
                case MovementDirection.Right:
                    var right = elements.Where(x => IsRight(head, x));
                    if (right.Any())
                        return right.Min(x => Distans(x, head));
                    else
                        return -1;
                default:
                    throw new Exception();
            }
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
        #endregion
    }

}
