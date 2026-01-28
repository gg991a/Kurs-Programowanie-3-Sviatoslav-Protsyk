using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Lista_2
{
    public partial class Zad2 : Window
    {
        public Zad2()
        {
            InitializeComponent();
            DataContext = new Game();
        }
    }

    public class Game : Base
    {
        public ObservableCollection<Box> List { get; set; } 

        private string _info;
        public string Info
        {
            get => _info;
            set { _info = value; OnProp(); }
        }

        private bool _turnX;   
        private bool _active;  

        public Relay Reset { get; set; }

        public Game()
        {
            Reset = new Relay(o => Start());
            Start();
        }

        private void Start()
        {
            List = new ObservableCollection<Box>();
            for (int i = 0; i < 9; i++)
            {
                var box = new Box();
                box.Click = new Relay(o => Play(box));
                List.Add(box);
            }
            OnProp(nameof(List)); 

            _turnX = true;
            _active = true;
            Info = "Ruch: X";
        }

        private void Play(Box b)
        {
            if (!_active || !string.IsNullOrEmpty(b.Sign)) return;

            b.Sign = _turnX ? "X" : "O";

            if (CheckWin())
            {
                Info = "Wygrał " + (_turnX ? "X" : "O") + "!";
                _active = false;
            }
            else if (List.All(x => !string.IsNullOrEmpty(x.Sign)))
            {
                Info = "Remis!";
                _active = false;
            }
            else
            {
                _turnX = !_turnX;
                Info = "Ruch: " + (_turnX ? "X" : "O");
            }
        }

        private bool CheckWin()
        {
            int[][] lines = {
                new[]{0,1,2}, new[]{3,4,5}, new[]{6,7,8}, 
                new[]{0,3,6}, new[]{1,4,7}, new[]{2,5,8}, 
                new[]{0,4,8}, new[]{2,4,6}                
            };

            foreach (var line in lines)
            {
                var s1 = List[line[0]].Sign;
                var s2 = List[line[1]].Sign;
                var s3 = List[line[2]].Sign;

                if (!string.IsNullOrEmpty(s1) && s1 == s2 && s2 == s3)
                    return true;
            }
            return false;
        }
    }

    public class Box : Base
    {
        private string _sign;
        public string Sign
        {
            get => _sign;
            set { _sign = value; OnProp(); }
        }

        public Relay Click { get; set; }
    }

    public class Base : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnProp([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class Relay : ICommand
    {
        private Action<object> _action;
        public Relay(Action<object> action) => _action = action;
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _action(parameter);
        public event EventHandler CanExecuteChanged;
    }
}