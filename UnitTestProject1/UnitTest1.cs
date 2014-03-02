using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpinionSharing.Agt;
using OpinionSharing.Agt.Algorithm;

namespace UnitTestProject1
{
    [TestClass]
    public class AgentStateTest
    {

        [TestMethod]
        public void UpdateCounter_Counterの正常系テスト()
        {
            UpdateCounter counter = new UpdateCounter();

            counter.CountUp();
            counter.CountUp();
            counter.CountUp();

            Assert.AreEqual(counter.UpdateNum, 3);
            Assert.AreEqual(counter.UpdateRight, 3);
            Assert.AreEqual(counter.UpdateLeft, 0);
            
            counter.CountDown();
            counter.CountDown();
            counter.CountDown();

            Assert.AreEqual(counter.UpdateNum, 0);
            Assert.AreEqual(counter.UpdateRight, 3);
            Assert.AreEqual(counter.UpdateLeft, 0);

            counter.CountDown();
            counter.CountDown();
            counter.CountDown();

            Assert.AreEqual(counter.UpdateNum, -3);
            Assert.AreEqual(counter.UpdateRight, 3);
            Assert.AreEqual(counter.UpdateLeft, 3);

            counter.CountUp();
            counter.CountUp();
            counter.CountUp();

            Assert.AreEqual(counter.UpdateNum, 0);
            Assert.AreEqual(counter.UpdateRight, 3);
            Assert.AreEqual(counter.UpdateLeft, 3);
            //Assert.AreEqual(agent.Belief, agent.PriorBelief);

            //Assert.IsTrue(Math.Abs(agent.Belief - agent.PriorBelief) < 0.00001);

        }

        [TestMethod]
        public void NewUpdateCounter_Counterの正常系テスト()
        {
            /*
            UpdateCounter counter = new UpdateCounter();

            counter.CountUp();
            counter.CountUp();
            counter.CountUp();

            Assert.AreEqual(counter.MaxUpdateNum, 3);

            counter.CountDown();
            counter.CountDown();
            counter.CountDown();

            Assert.AreEqual(counter.MaxUpdateNum, 3);

            counter.CountDown();
            counter.CountDown();
            counter.CountDown();

            Assert.AreEqual(counter.MaxUpdateNum, 3);

            counter.CountDown();
            counter.CountDown();
            counter.CountDown();

            Assert.AreEqual(counter.MaxUpdateNum, 6);

            counter.CountDown();

            Assert.AreEqual(counter.MaxUpdateNum, 7);
            */
        }

    }
}
