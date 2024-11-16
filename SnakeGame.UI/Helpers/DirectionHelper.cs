using SnakeGame.UI.Entities;
using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Helpers
{
    public static class DirectionHelper
    {
        public static MovementDirection GetDirect(IList<double> output, MovementDirection direct)
        {
            Random random = new Random();
            var result = new List<double>();
            var bests = new List<int>(); ;
            int bestIndex = -1;
            for (int i = 0; i < output.Count; i++)
                if (output.Max() == output[i])
                    bests.Add(i);

            if(bests.Count()>1)
                bestIndex =random.Next(0, bests.Count());
            else
                bestIndex = bests.FirstOrDefault();

            if (output.All(x => x == 0))
                bestIndex = 1;

            switch (direct)
            {
                case MovementDirection.Left:
                    if (0 == bestIndex)
                        return MovementDirection.Down;
                    if (1 == bestIndex)
                        return MovementDirection.Left;
                    if (2 == bestIndex)
                        return MovementDirection.Up;
                    break;
                case MovementDirection.Right:
                    if (0 == bestIndex)
                        return MovementDirection.Up;
                    if (1 == bestIndex)
                        return MovementDirection.Right;
                    if (2 == bestIndex)
                        return MovementDirection.Down;
                    break;
                case MovementDirection.Up:
                    if (0 == bestIndex)
                        return MovementDirection.Left;
                    if (1 == bestIndex)
                        return MovementDirection.Up;
                    if (2 == bestIndex)
                        return MovementDirection.Right;
                    break;
                case MovementDirection.Down:
                    if (0 == bestIndex)
                        return MovementDirection.Right;
                    if (1 == bestIndex)
                        return MovementDirection.Down;
                    if (2 == bestIndex)
                        return MovementDirection.Left;
                    break;

            }
            return MovementDirection.Left;
        }
    }
}
