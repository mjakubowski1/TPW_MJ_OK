using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Data
{
    internal class BallData : BallInterface
    {
        private Task task;
        private bool move = true;
        private int _radius;
        private Stopwatch stopWatch;
        private int _mass;

        public BallData(float x, float y, int mass, Vector2 velocity, int radious, int id)
        {
            stopWatch = new Stopwatch();
            Id = id;
            _position = new Vector2(x, y);
            this._velocity = velocity;
            _radius = radious;
            this._mass = mass;
            task = Task.Run(Move);
        }

        public event EventHandler? BallChanged;


        private Vector2 _position;

        public Vector2 Position
        {
            get => _position;

            set
            {
                _position = value;
            }
        }


        private Vector2 _velocity;

        public Vector2 Velocity
        {
            get => _velocity;
            set
            {

                _velocity = value;

            }
        }


        public int Radius
        {
            get => _radius;
        }


        public int Mass
        {
            get => _mass;
            private set { _mass = value; }
        }


        public int Id { get; }


        private async void Move()
        {
            float time;

            while (move)
            {
                stopWatch.Restart();
                stopWatch.Start();
                time = (2 / _velocity.Length());
                UpdatePosition(time);

                stopWatch.Stop();
                await Task.Delay(time - stopWatch.ElapsedMilliseconds < 0 ? 0 : (int)(time - stopWatch.ElapsedMilliseconds));
            }

        }


        private void UpdatePosition(float time)
        {
            Position += _velocity * time;
            BallChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            move = false;
            task.Wait();
            task.Dispose();
        }

    }

}