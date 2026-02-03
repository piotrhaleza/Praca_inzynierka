using SnakeGame.UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.UI.Learning
{
    public static class OutputGetter
    {
        private static void GetWallExpecttion(List<double> result, Dictionary<SnakeInputs, double> inputs)
        {
            if (inputs[SnakeInputs.LeftToWall] == 1)
            {
                result[0] = 0;
            }
            if (inputs[SnakeInputs.ForwardToWall] == 1)
            {
                result[1] = 0;
            }
            if (inputs[SnakeInputs.RightToWall] == 1)
            {
                result[2] = 0;
            }
        }
        private static void GetAppleExpecttion(List<double> result, Dictionary<SnakeInputs, double> inputs)
        {
            if (inputs[SnakeInputs.LeftToApple] == 1)
            {
                result[0] = 1;
            }
            else if (inputs[SnakeInputs.LeftBackToApple] == 1 && inputs[SnakeInputs.LeftForwardToSnake] == 0)
            {
                result[0] = result[0] != 0 ? 1 : 0; ;
            }
            else if (inputs[SnakeInputs.LeftForwardToApple] == 1 && inputs[SnakeInputs.LeftForwardToSnake] == 0)
            {
                result[1] = result[1] != 0 ? 1 : 0; ;
            }
            else if (inputs[SnakeInputs.ForwardToApple] == 1)
            {
                result[1] = 1;
            }
            else if (inputs[SnakeInputs.RightToApple] == 1)
            {
                result[2] = result[2] != 0 ? 1 : 0; ;
            }
            else if (inputs[SnakeInputs.RightBackToApple] == 1 && inputs[SnakeInputs.RightBackToSnake] == 0)
            {
                result[2] = result[2] != 0 ? 1 : 0; ;
            }
            else if (inputs[SnakeInputs.RightForwardToApple] == 1 && inputs[SnakeInputs.RightForwardToSnake] == 0)
            {
                result[1] = result[1] != 0 ? 1 : 0; ;
            }
        }
        private static void GetSnakeBodyExpecttion(List<double> result, Dictionary<SnakeInputs, double> inputs)
        {
            if (inputs[SnakeInputs.LeftToSnake] == 1)
            {
                result[0] = 0;

                if (result[2] != 0)
                    result[2] = 1;
            }
            if (inputs[SnakeInputs.ForwardToSnake] == 1)
            {
                result[1] = 0;
            }
            if (inputs[SnakeInputs.RightToSnake] == 1)
            {
                result[2] = 0;

                if (result[0] != 0)
                    result[0] = 1;
            }
            if (inputs[SnakeInputs.RightForwardToSnake] == 1 && inputs[SnakeInputs.LeftForwardToSnake] == 1 && (result[0] != 0 || result[2] != 0))
            {
                result[1] = 0;
            }
            if (inputs[SnakeInputs.LeftBackToSnake] == 1 && inputs[SnakeInputs.LeftForwardToSnake] == 1)
            {
                result[0] = 0;
            }
            if (inputs[SnakeInputs.RightBackToSnake] == 1 && inputs[SnakeInputs.RightForwardToSnake] == 1)
            {
                result[2] = 0;
            }
            //if (result[1] != 1 && (result[0] > 0 || result[2] > 0) && inputs[SnakeInputs.ForwardToSnake] > 0)
            //{
            //    result[1] = 0;
            //}
            //if (result[1] != 1 && (result[0] > 0 || result[2] > 0) && inputs[SnakeInputs.ForwardToWall] > 0)
            //{
            //    result[1] = 0;
            //}
        }
        private static void GetCloseToWallExpecttion(List<double> result, Dictionary<SnakeInputs, double> inputs)
        {

            //if (result[0] != 1 && (result[1] > 0 || result[2] > 0) && inputs[SnakeInputs.LeftToWall] > 0)
            //{
            //    result[0] = 0;
            //}
            if (result[1] != 1 && (result[0] > 0 || result[2] > 0) && inputs[SnakeInputs.ForwardToWall] > 0)
            {
                result[1] = 0;
            }
            //if (result[2] != 1 && (result[1] > 0 || result[0] > 0) && inputs[SnakeInputs.RightToWall] > 0)
            //{
            //    result[2] = 0;
            //}
        }
        private static void ChooseTheBest(List<double> result)
        {
            if (!result.Any(x => x == 1))
            {
                if (result[1] == 0)
                {
                    var same = result[0] == result[2];
                    if (same)
                    {
                        Random random = new Random();
                        var exp = random.Next(0, 1);

                        result[0] = 1;

                    }
                    else
                    {
                        if (result[0] > result[2])
                            result[0] = 1;
                        else
                            result[2] = 1;
                    }
                }
                else if (result.Any(x => x == 0.6))
                {

                    if (result[0] > result[2])
                        result[0] = 1;
                    else
                        result[2] = 1;
                }
                else
                {
                    result[1] = 1;
                }

            }


            for (int i = 0; i < result.Count(); i++)
                result[i] = result[i] != 1 ? 0 : result[i];
        }

        public static List<double> GetExpectedOutputs(Dictionary<SnakeInputs, double> inputs)
        {
            List<double> result = new List<double> { 0.5, 0.5, 0.5 };

            GetWallExpecttion(result, inputs);
            GetAppleExpecttion(result, inputs);
            GetSnakeBodyExpecttion(result, inputs);
            GetCloseToWallExpecttion(result, inputs);
            ChooseTheBest(result);

            return result;
        }

    }
}
