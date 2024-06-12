
using Data;
using System;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }

        public static DataAbstractAPI CreateDataAPI(int w, int h)
        {
            return new DataLayer(w, h);
        }

        public abstract int GetBallAmount();
        public abstract BallInterface GetBallByID(int index);
        public abstract void CreateBalls(int count);
        public abstract void RemoveBalls();


        internal class DataLayer : DataAbstractAPI
        {
            private Logger logger;
            private List<BallInterface> _balls;
            private readonly Random _random = new Random();
            public override int BoardWidth { get; }
            public override int BoardHeight { get; }

            public DataLayer(int w, int h)
            {
                _balls = new List<BallInterface>();
                BoardWidth = w;
                BoardHeight = h;
            }

            public override int GetBallAmount()
            {
                return _balls.Count;
            }

            public override BallInterface GetBallByID(int index)
            {
                return _balls[index];
            }


            public override void CreateBalls(int count)
            {
                logger = new Logger();
                for (int i = 0; i < count; i++)
                {
                    float velX = (float)((_random.NextDouble() - 0.5) / 2);
                    float velY = (float)((_random.NextDouble() - 0.5) / 2);
                    while (velX == 0 & velY == 0)
                    {
                        velX = _random.Next(-2, 2);
                        velY = _random.Next(-2, 2);
                    }

                    Vector2 vel = new Vector2(velX, velY);
                    int radius = 40;
                    int mass = radius * 2;
                    float ballX = (float)(_random.Next(15 + radius, BoardWidth - radius - 15) + _random.NextDouble());
                    float ballY = (float)(_random.Next(15 + radius, BoardHeight - radius - 15) + _random.NextDouble());

                    BallData ball = new BallData(ballX, ballY, mass, vel, radius, i);
                    //ball.BallChanged += (object? sender, EventArgs args) => logger.AddBallToQueue((BallInterface)sender);
                    logger.AddLogDataToQueue(new BallDataToSerialize(new Vector2(ballX, ballY), mass, vel, radius, i));

                    ball.BallChanged += (object? sender, EventArgs args) => logger.AddLogDataToQueue(new BallDataToSerialize(
                        ((BallInterface)sender).Position,
                        ((BallInterface)sender).Mass,
                        ((BallInterface)sender).Velocity,
                        ((BallInterface)sender).Radius,
                        ((BallInterface)sender).Id
                    ));
                    _balls.Add(ball);  
                }
            }

            public override void RemoveBalls()
            {
                foreach (BallInterface ball in _balls)
                {
                    ball.Dispose();
                }
                _balls.Clear();
            }
        }
    }
}