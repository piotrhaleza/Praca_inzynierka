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
            var result = new List<double>();

            switch (direct)
            {
                case MovementDirection.Left:
                    if (output[0] == output.Max())
                        return MovementDirection.Down;
                    if (output[1] == output.Max())
                        return MovementDirection.Left;
                    if (output[2] == output.Max())
                        return MovementDirection.Up;
                    break;
                case MovementDirection.Right:
                    if (output[0] == output.Max())
                        return MovementDirection.Up;
                    if (output[1] == output.Max())
                        return MovementDirection.Right;
                    if (output[2] == output.Max())
                        return MovementDirection.Down;
                    break;
                case MovementDirection.Up:
                    if (output[0] == output.Max())
                        return MovementDirection.Left;
                    if (output[1] == output.Max())
                        return MovementDirection.Up;
                    if (output[2] == output.Max())
                        return MovementDirection.Right;
                    break;
                case MovementDirection.Down:
                    if (output[0] == output.Max())
                        return MovementDirection.Right;
                    if (output[1] == output.Max())
                        return MovementDirection.Down;
                    if (output[2] == output.Max())
                        return MovementDirection.Left;
                    break;

            }
            return MovementDirection.Left;
        }
    }
}
