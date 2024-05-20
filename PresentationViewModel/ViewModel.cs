using Data;
using Logic;
using PresentationModel;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace PresentationViewModel
{
    public class ViewModel : ViewModelBase
    {

        private readonly ModelAbstractAPI modelAPI;
        private int _ballsAmount;
        private IList _balls;
        private readonly int _width;
        private readonly int _height;


        public int BallsAmount
        {
            get => _ballsAmount;
            set
            {
                _ballsAmount = value;
                RaisePropertyChanged("BallsAmount");
            }
        }

        public IList BallsList
        {
            get => _balls;
            set
            {
                _balls = value;
                RaisePropertyChanged("BallsList");
            }
        }

        public ViewModel() : this(ModelAbstractAPI.CreateModelAPI()) { }
        public ViewModel(ModelAbstractAPI modelAbstractAPI)
        {
            modelAPI = ModelAbstractAPI.CreateModelAPI();
            _height = modelAPI.Height;
            _width = modelAPI.Width;
            ClickButton = new RelayCommand(OnClickButton);
            ExitClick = new RelayCommand(OnExitClick);

        }

        public ICommand ClickButton { get; set; }
        public ICommand ExitClick { get; set; }

        public int Width => _width;

        public int Height => _height;

        private void OnClickButton()
        {
            BallsList = modelAPI.CreateBalls(_ballsAmount);
            modelAPI.CallSimulation();
        }

        private void OnExitClick()
        {
            modelAPI.StopSimulation();
        }

    }
}