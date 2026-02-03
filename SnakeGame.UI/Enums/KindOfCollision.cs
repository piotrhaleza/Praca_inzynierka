using GeneticInterfaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Enums
{
    public enum KindOfCollision
    {
        NoCollision = 0,
        AppleCollision = 1,
        WallCollision = 2,
        SnakeCollision = 3,
        TimeCollision = 4,
    }

    public static class KindOfLossConverter
    {
        public static KindOfCollision Convert(this KindOfLose lose)
        {
            switch (lose)
            {
                case KindOfLose.NoCollision:
                    return KindOfCollision.NoCollision;
                case KindOfLose.WallCollision:
                    return KindOfCollision.WallCollision;
                case KindOfLose.SnakeCollision:
                    return KindOfCollision.SnakeCollision;
                case KindOfLose.TimeCollision:
                    return KindOfCollision.TimeCollision;
                default:
                    return KindOfCollision.NoCollision;
            }
        }
        public static KindOfLose Convert(this KindOfCollision lose)
        {
            switch (lose)
            {
                case KindOfCollision.NoCollision:
                    return KindOfLose.NoCollision;
                case KindOfCollision.AppleCollision:
                    return KindOfLose.NoCollision;
                case KindOfCollision.WallCollision:
                    return KindOfLose.WallCollision;
                case KindOfCollision.SnakeCollision:
                    return KindOfLose.SnakeCollision;
                case KindOfCollision.TimeCollision:
                    return KindOfLose.TimeCollision;
                default:
                    return KindOfLose.NoCollision;
            }
        }
    }
}
