using System;
using System.Numerics;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Tests
{
    [TestClass]
    public class LogicAbstractAPITests
    {
        [TestMethod]
        public void CreateLogicAPI_ShouldReturnInstanceOfLogicAPI()
        {

            int width = 800;
            int height = 600;

            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI(width, height);

            Assert.IsNotNull(logicAPI);
            Assert.IsInstanceOfType(logicAPI, typeof(LogicAbstractAPI));
        }

        [TestMethod]
        public void CreateBalls_ShouldCreateSpecifiedNumberOfBalls()
        {

            int width = 800;
            int height = 600;
            int ballCount = 5;
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI(width, height);

            logicAPI.CreateBalls(ballCount);
            int actualBallCount = logicAPI.GetBallsAmount();

            Assert.AreEqual(ballCount, actualBallCount);
        }

        [TestMethod]
        public void DeleteBalls_ShouldRemoveAllBalls()
        {

            int width = 800;
            int height = 600;
            int ballCount = 5;
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI(width, height);
            logicAPI.CreateBalls(ballCount);

            logicAPI.DeleteBalls();
            int actualBallCount = logicAPI.GetBallsAmount();

            Assert.AreEqual(0, actualBallCount);
        }

        [TestMethod]
        public void GetBallsAmount_ShouldReturnCorrectNumberOfBalls()
        {

            int width = 800;
            int height = 600;
            int ballCount = 5;
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI(width, height);
            logicAPI.CreateBalls(ballCount);

            int actualBallCount = logicAPI.GetBallsAmount();

            Assert.AreEqual(ballCount, actualBallCount);
        }

        [TestMethod]
        public void GetBallRadiusByID_ShouldReturnCorrectDiameter()
        {

            int width = 800;
            int height = 600;
            int ballCount = 5;
            LogicAbstractAPI logicAPI = LogicAbstractAPI.CreateLogicAPI(width, height);
            logicAPI.CreateBalls(ballCount);
            int ballId = 2;
            BallInterface ball = logicAPI.GetBall(ballId);
            int expectedRadius = ball.Radius;

            int actualRadius = logicAPI.GetBallRadiusByID(ballId);

            Assert.AreEqual(expectedRadius, actualRadius);
        }

    }
}