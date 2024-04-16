using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Ball : INotifyPropertyChanged
    {
        private Vector2 _position;
        private int _radius;
        private const int _speed = 500;


        public Ball() { }

        public Ball(Vector2 ballPosition, int radius)
        {
            _position = ballPosition;
            _radius = radius;
        }
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public Vector2 Velocity { get; set; }


        public float X
        {
            get => _position.X;
        }
        public float Y
        {
            get => _position.Y;
        }

        public int Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void ChangePosition()
        {
            Position += new Vector2(Velocity.X * _speed, Velocity.Y * _speed);
            if (Position.X < 0 || Position.X > Board._boardWidth - 20)
            {
                Velocity *= -Vector2.UnitX;
            }

            if (Position.Y < 0 || Position.Y > Board._boardHeight - 20)
            {
                Velocity *= -Vector2.UnitY;
            }

            RaisePropertyChanged(nameof(X));
            RaisePropertyChanged(nameof(Y));
        }
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
   
}