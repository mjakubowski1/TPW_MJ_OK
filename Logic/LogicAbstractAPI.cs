using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Threading;
using Data;
using Microsoft.VisualBasic;

namespace Logic
{
    public abstract class LogicAbstractAPI

    {
        public static LogicAbstractAPI CreateLogicAPI(int w, int h, DataAbstractAPI? data = null)
        {
            return new LogicAPI(w, h, data);
        }


        public abstract void CreateBalls(int count);
        public abstract void DeleteBalls();
        public abstract int GetBallsAmount();
        public abstract BallInterface GetBall(int id);
        public abstract int GetBallRadiusByID(int id);
        public abstract Vector2 GetBallPositionByID(int id);



        public abstract int BoardWidth { get; set; }
        public abstract int BoardHeight { get; set; }


        public abstract event EventHandler<(int Id, float X, float Y, int Diameter)>? LogicEvent;
    }
    internal class LogicAPI : LogicAbstractAPI
    {
        private readonly object _lock = new object();
        public override event EventHandler<(int Id, float X, float Y, int Diameter)>? LogicEvent;
        private ConcurrentDictionary<(int, int), bool> _collisionFlags = new ConcurrentDictionary<(int, int), bool>();
        DataAbstractAPI _dataAPI;
        private Logger logger;
        public LogicAPI(int w, int h, DataAbstractAPI? data)
        {
            BoardWidth = w;
            BoardHeight = h;
            _dataAPI = data != null ? data : DataAbstractAPI.CreateDataAPI(w, h);
            logger = new Logger();
        }

        public override int BoardWidth { get; set; }
        public override int BoardHeight { get; set; }

        
        public override int GetBallsAmount()
        {
            return _dataAPI.GetBallAmount();
        }

        public override int GetBallRadiusByID(int id)
        {
            return _dataAPI.GetBallByID(id).Radius;
        }

        public override BallInterface GetBall(int id)
        {
            return _dataAPI.GetBallByID((int)id);
        }

        public override Vector2 GetBallPositionByID(int id)
        {
            BallInterface ball = _dataAPI.GetBallByID(id);
            return ball.Position;
        }


        public void BallPositionChanged(object? sender, EventArgs e)
        {
            if (sender == null) return;
            BallInterface ball = (BallInterface)sender;
            lock (_lock)
            {
                CheckBallCollision(ball);
            }
            DetectWallCollision(ball);
            LogicEvent?.Invoke(this, (ball.Id, ball.Position.X, ball.Position.Y, ball.Radius));
        }
        

        private void DetectWallCollision(BallInterface ball)
        {

            Vector2 newVel = new Vector2(ball.Velocity.X, ball.Velocity.Y);
            int Radius = ball.Radius / 2;
            if (ball.Position.X - Radius <= 0)
            {
                newVel.X = Math.Abs(ball.Velocity.X);
            }
            else if (ball.Position.X + Radius >= BoardWidth)
            {
                newVel.X = -Math.Abs(ball.Velocity.X);
            }

            if (ball.Position.Y - Radius <= 0)
            {
                newVel.Y = Math.Abs(ball.Velocity.Y);
            }
            else if (ball.Position.Y + Radius >= BoardHeight)
            {
                newVel.Y = -Math.Abs(ball.Velocity.Y);

            }

            ball.Velocity = newVel;
        }


        private void CheckBallCollision(BallInterface firstBall)
        {
            for (int i = 0; i < _dataAPI.GetBallAmount(); i++)
            {
                BallInterface secondBall = _dataAPI.GetBallByID(i);
                if (firstBall == secondBall)
                {
                    continue;
                }

                if (!HasCollisionBeenChecked(secondBall, firstBall) && IsCollision(firstBall, secondBall))
                {
                    MarkCollisionAsChecked(firstBall, secondBall);

                    Vector2 newFirstBallVel = NewVelocity(firstBall, secondBall);
                    Vector2 newSecondBallVel = NewVelocity(secondBall, firstBall);
                    if (Vector2.Distance(firstBall.Position, secondBall.Position) > Vector2.Distance(
                        firstBall.Position + newFirstBallVel, secondBall.Position + newSecondBallVel))
                    {
                        return;
                    }
                    firstBall.Velocity = newFirstBallVel;
                    secondBall.Velocity = newSecondBallVel;

                    CollisionDataToSerialize collisionData = new CollisionDataToSerialize(firstBall.Id, secondBall.Id);
                    logger.AddLogDataToQueue(collisionData);
                }
                else
                {
                    RemoveCollisionFromChecked(secondBall, firstBall);
                }
            }
        }


        private Vector2 NewVelocity(BallInterface firstBall, BallInterface secondBall)
        {
            var ball1Vel = firstBall.Velocity;
            var ball2Vel = secondBall.Velocity;
            var distance = firstBall.Position - secondBall.Position;
            return firstBall.Velocity -
                   2.0f * secondBall.Mass / (firstBall.Mass + secondBall.Mass)
                   * (Vector2.Dot(ball1Vel - ball2Vel, distance) * distance) /
                   (float)Math.Pow(distance.Length(), 2);
        }

        private bool HasCollisionBeenChecked(BallInterface firstBall, BallInterface secondBall)
        {
            int id1 = firstBall.Id;
            int id2 = secondBall.Id;
            var key = (id1, id2);
            return _collisionFlags.ContainsKey(key);
        }

        private bool IsCollision(BallInterface firstBall, BallInterface secondBall)
        {
            if (firstBall == null || secondBall == null)
            {
                return false;
            }
            float distance = Vector2.Distance(firstBall.Position, secondBall.Position);
            return distance <= (firstBall.Radius + secondBall.Radius) / 2;
        }


        private void MarkCollisionAsChecked(BallInterface firstBall, BallInterface secondBall)
        {
            int id1 = firstBall.Id;
            int id2 = secondBall.Id;
            var key = (id1, id2);
            _collisionFlags.TryAdd(key, true);
        }

        private void RemoveCollisionFromChecked(BallInterface firstBall, BallInterface secondBall)
        {
            int id1 = firstBall.Id;
            int id2 = secondBall.Id;
            var key = (id1, id2);
            _collisionFlags.Remove(key, out _);
        }


        public override void CreateBalls(int count)
        {
            _dataAPI.CreateBalls(count);
            for (int i = 0; i < count; i++)
            {
                _dataAPI.GetBallByID(i).BallChanged += BallPositionChanged;

            }
        }

        public override void DeleteBalls()
        {
            for (int i = 0; i < _dataAPI.GetBallAmount(); i++)
            {
                _dataAPI.GetBallByID(i).BallChanged -= BallPositionChanged;

            }
            _dataAPI.RemoveBalls();
        }

      

    }
}