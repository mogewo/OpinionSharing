using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpinionSharing.Agt;
using OpinionSharing;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestMethod1()
        {
            var agentIO = new AgentIO();

            IAATBasedAgent algo = new AAT();

            algo.TargetAwarenessRate = 0.9;

            agentIO.Algorithm = (algo as AgentAlgorithm);

        }
    }
}
