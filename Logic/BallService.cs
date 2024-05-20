using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Data;
using Microsoft.VisualBasic;

namespace Logic
{
    public class BallService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly BallData _ball;


        public BallService(BallData ball)
        {
            _ball = ball;
        }


        public float X => _ball.Position.X;
        public float Y => _ball.Position.Y;
        public float Radius => _ball.Radius;
        public float Mass => _ball.Mass;
        public float Diameter => _ball.Radius * 2;

        public Vector2 Velocity
        {
            get => _ball.Velocity;
            set
            {
                _ball.Velocity = value;
                RaisePropertyChanged(nameof(Velocity));
            }
        }


        public bool CollidesWith(BallService other)
        {
            Vector2 distance = new Vector2(X + Radius, Y + Radius) - new Vector2(other.X + other.Radius, other.Y + other.Radius);
            float sumRadii = Radius + other.Radius;
            return distance.LengthSquared() <= sumRadii * sumRadii;
        }


        public void HandleCollision(BallService other)
        {
            Vector2 collisionNormal = Vector2.Normalize(new Vector2(other.X, other.Y) - new Vector2(X, Y));
            Vector2 relativeVelocity = other.Velocity - Velocity;
            float velocityAlongNormal = Vector2.Dot(relativeVelocity, collisionNormal);

            if (velocityAlongNormal > 0) return; // Kulki oddalają się od siebie, brak kolizji

            float e = 1.0f; // Współczynnik sprężystości (1 = doskonale sprężysty)

            float j = -(1 + e) * velocityAlongNormal;
            j /= (1 / Mass) + (1 / other.Mass);

            Vector2 impulse = j * collisionNormal;

            Velocity -= (1 / Mass) * impulse;
            other.Velocity += (1 / other.Mass) * impulse;
        }

        public void UpdatePosition()
        {
            _ball.UpdatePosition();
            RaisePropertyChanged(nameof(X), nameof(Y));
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