using Data;
using System.Collections.ObjectModel;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Data.Tests
{
    [TestClass]
    public class DataAbstractAPITests
    {
        [TestMethod]
        public void GetBallData_ReturnsValidBallData()
        {
            DataAbstractAPI dataAPI = DataAbstractAPI.CreateDataAPI();
            Vector2 position = new Vector2(100, 200);
            Vector2 velocity = new Vector2(0.1f, -0.2f);
            float radius = 10f;
            float mass = 5f;

            BallData ballData = dataAPI.GetBallData(position, velocity, radius, mass);

            Assert.IsNotNull(ballData);
            Assert.AreEqual(position, ballData.Position);
            Assert.AreEqual(velocity, ballData.Velocity);
            Assert.AreEqual(radius, ballData.Radius);
            Assert.AreEqual(mass, ballData.Mass);
        }

        [TestMethod]
        public void CreateDataAPI_ReturnsValidDataAPIInstance()
        {
            DataAbstractAPI dataAPI = DataAbstractAPI.CreateDataAPI();

            Assert.IsNotNull(dataAPI);
        }

        [TestMethod]
        public void BoardWidth_ReturnsCorrectValue()
        {
            int boardWidth = DataAbstractAPI.BoardWidth;

            Assert.AreEqual(750, boardWidth);
        }

        [TestMethod]
        public void BoardHeight_ReturnsCorrectValue()
        {
            int boardHeight = DataAbstractAPI.BoardHeight;

            Assert.AreEqual(400, boardHeight);
        }
    }
}
