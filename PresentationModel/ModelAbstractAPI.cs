using System;
using System.Collections.ObjectModel;
using Data;
using Logic;

namespace PresentationModel
{
    public abstract class ModelAbstractAPI
    {
        public abstract int Width { get; }
        public abstract int Height { get; }

        public static ModelAbstractAPI CreateModelAPI(LogicAbstractAPI logicAPI = default(LogicAbstractAPI))
        {
            return new ModelAPI(logicAPI);
        }
        public abstract void CallSimulation();
        public abstract void StopSimulation();
        public abstract ObservableCollection<BallService> CreateBalls(int ballsNumber);
        public abstract int GetBallsAmount();


    }
    public class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        public override int Width => logicAPI.GetBoardWidth();
        public override int Height => logicAPI.GetBoardHeight();

        public ModelAPI() : this(LogicAbstractAPI.CreateLogicAPI()) { }

        public ModelAPI(LogicAbstractAPI logicApi)
        {
            logicAPI = logicApi ?? LogicAbstractAPI.CreateLogicAPI();
        }


        public override void CallSimulation()
        {
            logicAPI.RunSimulation();
        }

        public override void StopSimulation()
        {
            logicAPI.StopSimulation();
        }

        public override ObservableCollection<BallService> CreateBalls(int ballsNumber)
        {
            logicAPI.CreateBalls(ballsNumber);
            return logicAPI.Balls;
        }

        public override int GetBallsAmount()
        {
            return logicAPI.Balls.Count;
        }
    }
}