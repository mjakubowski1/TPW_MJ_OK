using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Data
{
    public class BallData : INotifyPropertyChanged
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private float _speed = 1500;
        private float _radius;
        private float _mass;
        private bool _move = true;


        public BallData(Vector2 position, Vector2 velocity, float radious, float mass)
        {
            _position = position;
            _radius = radious;
            _mass = mass;
            _velocity = velocity;
        }


        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                RaisePropertyChanged();
            }
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;

        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public float Radius
        {
            get => _radius;
            set => _radius = value;
        }

        public float Mass
        {
            get => _mass;
            set => _mass = value;
        }

        public float X
        {
            get { return _position.X; }
            private set => _position.X = value;

        }
        public float Y
        {
            get { return _position.Y; }
            private set => _position.Y = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly object _lockObj = new object();

        private void ChangePosition()
        {
            Position += new Vector2(_velocity.X * _speed, _velocity.Y * _speed);
            if (_position.X - 5 <= 0 && +_velocity.X < 0)
            {
                _velocity = new Vector2(-Velocity.X, Velocity.Y);
                X += 4 * _velocity.X * _speed;
            }
            if (_position.X >= DataAbstractAPI.BoardWidth - _radius * 2 && _velocity.X > 0)
            {
                _velocity = new Vector2(-Velocity.X, Velocity.Y);
                X += 4 * _velocity.X * _speed;
            }
            if (_position.Y - 5 <= 0 && _velocity.Y < 0)
            {
                _velocity = new Vector2(Velocity.X, -Velocity.Y);
                Y += 4 * _velocity.Y * _speed;
            }
            if (_position.Y >= DataAbstractAPI.BoardHeight - _radius * 2 && _velocity.Y > 0)
            {
                _velocity = new Vector2(Velocity.X, -Velocity.Y);
                Y += 4 * _velocity.Y * _speed;
            }


            RaisePropertyChanged(nameof(Position));

        }

        public void UpdatePosition()
        {
            ChangePosition();
        }

        protected virtual void RaisePropertyChanged(params string[] propertyNames)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}