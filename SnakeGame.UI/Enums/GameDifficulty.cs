using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Enums
{
    public enum GameDifficulty
    {
        [Description("Łatwy")]
        Easy =0,
        [Description("Średni")]
        Medium = 1,
        [Description("Trudny")]
        Hard =2,
    }
}
