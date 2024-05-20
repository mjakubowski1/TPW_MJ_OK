using System.Collections.ObjectModel;
using System.Numerics;
using System.Threading;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Tests
{
    [TestClass]
    public class LogicAbstractAPITests
    {
        [TestMethod]
        public void CreateBalls_CreatesSpecifiedNumberOfBalls()
        {
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI();
            int ballCount = 5;

            logicAPI.CreateBalls(ballCount);

            Assert.AreEqual(ballCount, logicAPI.Balls.Count);
        }

        [TestMethod]
        public void DeleteBalls_RemovesAllBalls()
        {
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI();
            int ballCount = 5;
            logicAPI.CreateBalls(ballCount);

            logicAPI.DeleteBalls();

            Assert.AreEqual(0, logicAPI.Balls.Count);
        }

        [TestMethod]
        public void RunSimulation_StartsUpdatingBalls()
        {
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI();
            int ballCount = 5;
            logicAPI.CreateBalls(ballCount);

            logicAPI.RunSimulation();


        }

        [TestMethod]
        public void StopSimulation_StopsUpdatingBalls()
        {
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI();
            int ballCount = 5;
            logicAPI.CreateBalls(ballCount);
            logicAPI.RunSimulation();

            logicAPI.StopSimulation();

        }


        [TestMethod]
        public void CollidesWith_ReturnsTrue_WhenBallsCollide()
        {

            BallData ballData1 = new BallData(new Vector2(0, 0), new Vector2(1, 0), 10, 1);
            BallData ballData2 = new BallData(new Vector2(15, 0), new Vector2(-1, 0), 10, 1);
            BallService ballService1 = new BallService(ballData1);
            BallService ballService2 = new BallService(ballData2);

            bool result = ballService1.CollidesWith(ballService2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CollidesWith_ReturnsFalse_WhenBallsDoNotCollide()
        {

            BallData ballData1 = new BallData(new Vector2(0, 0), new Vector2(1, 0), 10, 1);
            BallData ballData2 = new BallData(new Vector2(30, 0), new Vector2(-1, 0), 10, 1);
            BallService ballService1 = new BallService(ballData1);
            BallService ballService2 = new BallService(ballData2);

            bool result = ballService1.CollidesWith(ballService2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HandleCollision_ChangesVelocities_OnElasticCollision()
        {

            BallData ballData1 = new BallData(new Vector2(0, 0), new Vector2(1, 0), 10, 1);
            BallData ballData2 = new BallData(new Vector2(15, 0), new Vector2(-1, 0), 10, 1);
            BallService ballService1 = new BallService(ballData1);
            BallService ballService2 = new BallService(ballData2);

            ballService1.HandleCollision(ballService2);

            Assert.AreEqual(new Vector2(-1, 0), ballService1.Velocity);
            Assert.AreEqual(new Vector2(1, 0), ballService2.Velocity);
        }

        [TestMethod]
        public void HandleCollision_NoChangeInVelocity_WhenBallsAreMovingApart()
        {

            BallData ballData1 = new BallData(new Vector2(0, 0), new Vector2(-1, 0), 10, 1);
            BallData ballData2 = new BallData(new Vector2(15, 0), new Vector2(1, 0), 10, 1);
            BallService ballService1 = new BallService(ballData1);
            BallService ballService2 = new BallService(ballData2);

            ballService1.HandleCollision(ballService2);

            Assert.AreEqual(new Vector2(-1, 0), ballService1.Velocity);
            Assert.AreEqual(new Vector2(1, 0), ballService2.Velocity);
        }
    }
}
