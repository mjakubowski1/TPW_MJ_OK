using Data;
using System.Collections.ObjectModel;
using System.Numerics;

namespace Logic
{
    internal class Board : LogicAbstractAPI
    {
        public static int _boardWidth = 750;
        public static int _boardHeight = 400;
        private CancellationToken _cancelToken;
        private List<Task> _tasks = new List<Task>();
        private List<Thread> _threads = new List<Thread>();
        public ObservableCollection<Ball> _balls = new ObservableCollection<Ball>();
        private readonly DataAbstractAPI dataAPI;

        public Board()
        {
            this.dataAPI = DataAbstractAPI.CreateDataAPI();
        }

        public CancellationToken CancellationToken => _cancelToken;

        public override void RunSimulation()
        {
            _cancelToken = CancellationToken.None;
            foreach (Ball ball in _balls)
            {
                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(5);
                        try
                        {
                            _cancelToken.ThrowIfCancellationRequested();
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }

                        ball.ChangePosition();
                    }
                });
                thread.IsBackground = true;
                thread.Start();
                _threads.Add(thread);
            }
        }

        public override void StopSimulation()
        {
            _cancelToken = new CancellationToken(true);
            _threads.Clear();
            _balls.Clear();
        }

        public override Ball CreateBall(Vector2 pos, int radius)
        {
            return new Ball(pos, radius);
        }

        public override void CreateBalls(int count, int radius)
        {
            if (count < 0)
            {
                count = Math.Abs(count);
            }
            Random random = new Random();
            for (int i = 0; i < count; i++)
            {
                Ball ball = new Ball();
                ball.Velocity = new Vector2((float)0.0045, (float)0.0045);
                ball.Position = new Vector2(random.Next(1, _boardWidth - 20), random.Next(1, _boardHeight - 20));

                _balls.Add(ball);
            }
        }

        public override void DeleteBalls()
        {
            _balls.Clear();
        }

        public int BoardWidth
        {
            get => _boardWidth;
        }
        public int BallsHeight
        {
            get => _boardHeight;
        }

        public override int Width => _boardWidth;

        public override int Height => _boardHeight;

        public override ObservableCollection<Ball> Balls => _balls;
    }
}
