using System.Collections.ObjectModel;
using System.Numerics;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI CreateApi()
        {
            return new Board();
        }

        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract ObservableCollection<Ball> Balls { get; }
        public abstract void RunSimulation();
        public abstract void StopSimulation();
        public abstract Ball CreateBall(Vector2 pos, int radius);
        public abstract void CreateBalls(int count, int radius);
        public abstract void DeleteBalls();
    }
}
