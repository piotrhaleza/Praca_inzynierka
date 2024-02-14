using MachineLearingInterfaces;
using MachineLearning;
using MachineLearning.Biases;
using MachineLearning.Wages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlUnitTest
{
    public class NetworkTests
    {
        IInitWages wages;
        IInitBiases biases;

        [SetUp] 
        public void SetUp() {

            wages = new InitRandomWages();
            biases = new InitZeroBiases();
        
        }

        [Test]
        public void NetowrkTest()
        {
            var network = new Network(wages, biases, 0);

            Assert.That(network, Is.Not.Null, "Ma nie byc nullem");
        }
        [TearDown]
        public void End()
        {
            wages = null;
        }

    }
}
