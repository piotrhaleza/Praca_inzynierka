using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame.UI.Entities
{
    public class Snake
    {
        private readonly int _elementSize;

        public Snake(int elementSize)
        {
            Elements = new List<SnakeElement>();
            _elementSize = elementSize;
        }
        public Snake(Snake copy)
        {
            Elements = new List<SnakeElement>();
            MovementDirection = copy.MovementDirection;
            foreach (var item in copy.Elements)
                Elements.Add(new SnakeElement(item.Size) { RealX = item.RealX, RealY = item.RealY });
            TailBackup= copy.TailBackup;
            _elementSize = copy._elementSize;
        }
        public  Snake Copy()
        {
            return new Snake(this);
        }

        public SnakeElement TailBackup { get; set; }
        public List<SnakeElement> Elements { get; set; }
        public List<SnakeElement> Body => Elements.Where(x => !x.IsHead).ToList();
        public List<GameEntity> GameEntities => Elements.Where(x => !x.IsHead).OfType<GameEntity>().ToList();
        public MovementDirection MovementDirection { get; set; }

        public SnakeElement Head => Elements.Any() ? Elements[0] : null;
        public int X => Head.X;
        public int Y => Head.Y;

        internal void UpdateMovementDirection(MovementDirection up)
        {
            switch (up)
            {
                case MovementDirection.Up:
                    if (MovementDirection != MovementDirection.Down)
                        MovementDirection = MovementDirection.Up;
                    break;
                case MovementDirection.Left:
                    if (MovementDirection != MovementDirection.Right)
                        MovementDirection = MovementDirection.Left;
                    break;
                case MovementDirection.Down:
                    if (MovementDirection != MovementDirection.Up)
                        MovementDirection = MovementDirection.Down;
                    break;
                case MovementDirection.Right:
                    if (MovementDirection != MovementDirection.Left)
                        MovementDirection = MovementDirection.Right;
                    break;
            }
        }

        internal void Grow()
        {
            if (Elements != null && TailBackup != null)
                Elements.Add(new SnakeElement(_elementSize) { RealX = TailBackup.RealX, RealY = TailBackup.RealY });
        }

        public bool CollisionWithSelf()
        {
            SnakeElement snakeHead = Head;
            if (snakeHead != null)
                foreach (var snakeElement in Elements)
                    if (!snakeElement.IsHead)
                        if (snakeElement.RealX == snakeHead.RealX && snakeElement.RealY == snakeHead.RealY)
                            return true;
            return false;
        }
        

        internal void PositionFirstElement(int cols, int rows, MovementDirection initialDirection)
        {
            Elements.Add(new SnakeElement(_elementSize, true)
            {
                RealX = cols * _elementSize,
                RealY = rows * _elementSize,
                IsHead = true
            });
            MovementDirection = initialDirection;
        }

        internal void MoveSnake()
        {
            SnakeElement head = Elements[0];
            SnakeElement tail = Elements[Elements.Count - 1];

            TailBackup = new SnakeElement(_elementSize)
            {
                RealX = tail.RealX,
                RealY = tail.RealY
            };

            head.IsHead = false;
            tail.IsHead = true;
            tail.RealX = head.RealX;
            tail.RealY = head.RealY;
            switch (MovementDirection)
            {
                case MovementDirection.Right:
                    tail.RealX += _elementSize;
                    break;
                case MovementDirection.Left:
                    tail.RealX -= _elementSize;
                    break;
                case MovementDirection.Up:
                    tail.RealY -= _elementSize;
                    break;
                case MovementDirection.Down:
                    tail.RealY += _elementSize;
                    break;
                default:
                    break;
            }
            Elements.RemoveAt(Elements.Count - 1);
            Elements.Insert(0, tail);
        }
    }


}
