using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PesentationModel
{
    public interface IBall : INotifyPropertyChanged
    {
        float Left { get; }
        float Top { get; }
        int Diameter { get; }
    }

    public class BallModel : IBall
    {
        private float _left;
        private float _top;
        private int _diameter;

        public BallModel(float x, float y, int diameter)
        {
            Top = y - diameter / 2;
            Left = x - diameter / 2;

            this._diameter = diameter;
        }

        public float Left
        {
            get { return _left; }
            private set
            {
                _left = value;
                OnPropertyChanged();
            }
        }
        public float Top
        {
            get { return _top; }
            private set
            {
                _top = value;
                OnPropertyChanged();
            }
        }
        public int Diameter
        {
            get { return _diameter; }
        }
        public void Move(float x, float y)
        {
            this.Left = x;
            this.Top = y;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
