using System;
using System.Collections.ObjectModel;
using Data;
using Logic;
using PesentationModel;

namespace PresentationModel
{
    public abstract class ModelAbstractAPI
    {
        public abstract int Width { get; }
        public abstract int Height { get; }

        public static ModelAbstractAPI CreateModelAPI(int w, int h)
        {
            return new ModelAPI(w, h);
        }

        public abstract void AddBalls(int amount);
        public abstract void StopSimulation();
        public ObservableCollection<BallModel> Balls { get; set; }
        public abstract int GetAmountOfBalls();
    }
    public class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        public override int Width { get; }
        public override int Height { get; }

        public ModelAPI(int w, int h)
        {
            Width = w;
            Height = h;
            logicAPI = LogicAbstractAPI.CreateLogicAPI(w, h);
            Balls = new ObservableCollection<BallModel>();
            logicAPI.LogicEvent += UpdateBall;
        }

        private void UpdateBall(object? sender, (int id, float x, float y, int diameter) args)
        {


            if (args.id >= Balls.Count)
            {
                return;
            }
            Balls[args.id].Move(args.x - args.diameter / 2, args.y - args.diameter / 2);

        }


        public override void StopSimulation()
        {
            logicAPI.DeleteBalls();
            Balls.Clear();
        }

        public override int GetAmountOfBalls()
        {
            return logicAPI.GetBallsAmount();
        }

        public override void AddBalls(int amount)
        {
            logicAPI.CreateBalls(amount);
            for (int i = 0; i < amount; i++)
            {
                BallModel ballModel = new BallModel(logicAPI.GetBallPositionByID(i).X, logicAPI.GetBallPositionByID(i).Y, logicAPI.GetBallRadiusByID(i));
                Balls.Add(ballModel);
            }
        }
    }
}