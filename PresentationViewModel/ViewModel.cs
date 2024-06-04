using Data;
using Logic;
using PesentationModel;
using PresentationModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PresentationViewModel
{
    public class ViewModel : ViewModelBase
    {

        private readonly ModelAbstractAPI modelAPI;
        private int _ballsAmount;
        private readonly int _width = 750;
        private readonly int _height = 400;
        public ObservableCollection<BallModel> BallsCollection { get; }


        public int BallsAmount
        {
            get => _ballsAmount;
            set
            {
                _ballsAmount = value;
                RaisePropertyChanged("AmountOfBalls");
            }
        }


        public ViewModel()
        {
            modelAPI = ModelAbstractAPI.CreateModelAPI(_width, _height);
            ClickButton = new RelayCommand(OnClickButton);
            ExitClick = new RelayCommand(OnExitClick);
            BallsCollection = modelAPI.Balls;
        }

        public ICommand ClickButton { get; set; }
        public ICommand ExitClick { get; set; }

        public int Width => _width;

        public int Height => _height;

        private void OnClickButton()
        {
            modelAPI.AddBalls(BallsAmount);
        }

        private void OnExitClick()
        {
            modelAPI.StopSimulation();
            BallsCollection.Clear();
        }

    }
}