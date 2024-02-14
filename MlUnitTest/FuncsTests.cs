using MachineLearingInterfaces.ActivationFunc;
using MachineLearning;
using MachineLearning.Funcs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlUnitTest
{
    [TestFixture]
    public class FuncsTests
    {

        //[TestCaseSource(typeof(TestCasesLineral),"TestCases")]
        //public double LineralFuncActivateTest(double a, double b, double x)
        //{
        //    var func = new LineralActivationFunc(a, b);

        //    var result = func.Activate(x);

        //    return result;
        //}
        //[Test]
        //[TestCase(1, 2, 1, ExpectedResult = 3)]
        //[TestCase(1, 0, 1, ExpectedResult = 1)]
        //[TestCase(0, 5, 1, ExpectedResult = 5)]
        //public double SigmoidFuncActivateTest(double a, double b, double x)
        //{
        //    var func = new LineralActivationFunc();

        //    return 0;
        //}
        //[Test]
        //public double Linera2FuncActivateTest([Values(100, 2, 3, 4)] double a, [Values(0, 6, 2, 4)] double b, [Values(0, 6, 2, 4)] double x)
        //{
        //    var func = new LineralActivationFunc(a, b);

        //    var result = func.Activate(x);

        //    return result;
        //}


        [Test]
        public void Linera2FuncActivateTest()
        {

            string ax = "Hania handluje kwiatami niedaleko szkoły i filharmonii. Uwielbia hiacynty, hortensje i herbaciane róże. Nie jest to błahe zajecie, bo wymaga dużo cierpliwości i talentu. Na horyzoncie pojawił się pomysł dostarczenia hiacyntów tego samego dnia i godzinie do szkoły i do filharmonii. Hania wahała się zatem co wybrać, tym bardziej, że padał deszcz i huczał wiatr. Uwielbia jednak harce dzieci, więc dostarczy kwiaty do szkoły. Tam Honoratka grała na harmonii, zagrała nawet przepięknie hymn narodowy. Dzieci hojnie obdarowały dyrektora i nauczycieli bukietami hiacyntów i wręczyli pamiątkowy herb szkoły. Wszyscy mieli świetny humor, a po uroczystości wypili zieloną herbatkę i zjedli maślane herbatniki.";
            ax = ax.Replace(".", "");
            ax = ax.Replace(",", "");

            var words = ax.Split(' ').Distinct().ToList();
            var texts = String.Join( "\n",words.OrderBy(x => x.Length).ToList());
            var texts2 = String.Join("\n", words.Where(x=> x.ToUpper().StartsWith("H")).OrderBy(x => x.Length).ToList());

        }

    }
    public class TestCasesLineral
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData(1, 2, 1).Returns(3);
                yield return new TestCaseData(0, 5, 1).Returns(5);
                yield return new TestCaseData(0, 5, 2).Returns(6);
            }
        }
    }
}
