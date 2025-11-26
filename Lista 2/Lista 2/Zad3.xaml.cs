using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Lista_2
{
    public partial class Zad3 : Window
    {
        public Zad3()
        {
            InitializeComponent();
            DataContext = new GraStatki(); 
        }
    }


    public class Baza : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnProp([CallerMemberName] string nazwa = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nazwa));
    }

    public class Komenda : ICommand
    {
        private Action<object> _akcja;
        public Komenda(Action<object> akcja) => _akcja = akcja;
        public bool CanExecute(object parametr) => true;
        public void Execute(object parametr) => _akcja(parametr);
        public event EventHandler CanExecuteChanged;
    }


    public enum Stan { Puste, Statek, Trafiony, Pudlo }

    public class Pole : Baza
    {
        private Stan _stan = Stan.Puste;
        private bool _czyWrog;

        public int X { get; set; }
        public int Y { get; set; }
        public Komenda StrzalKomenda { get; set; }

        public Stan AktualnyStan
        {
            get => _stan;
            set
            {
                _stan = value;
                OnProp();
                OnProp(nameof(Kolor));
                OnProp(nameof(Tekst));
            }
        }

        public Brush Kolor
        {
            get
            {
                if (_stan == Stan.Puste) return _czyWrog ? Brushes.AliceBlue : Brushes.White;
                if (_stan == Stan.Statek) return _czyWrog ? Brushes.AliceBlue : Brushes.Gray;
                if (_stan == Stan.Trafiony) return Brushes.Crimson;
                if (_stan == Stan.Pudlo) return Brushes.LightBlue;  
                return Brushes.White;
            }
        }

        public string Tekst
        {
            get
            {
                if (_stan == Stan.Trafiony) return "X";
                if (_stan == Stan.Pudlo) return "•";
                if (_stan == Stan.Statek && !_czyWrog) return "O";
                return "";
            }
        }

        public Pole(int x, int y, bool czyWrog, Action<Pole> poKliknieciu)
        {
            X = x; Y = y; _czyWrog = czyWrog;
            StrzalKomenda = new Komenda(o => poKliknieciu(this));
        }
    }

    public class GraStatki : Baza
    {
        private const int ROZMIAR = 10;
        private Random _los = new Random();

        private List<int> _statki = new List<int>
        {
            4,
            3, 3,
            2, 2, 2,
            1, 1, 1, 1
        };

        public ObservableCollection<Pole> PlanszaGracza { get; set; }
        public ObservableCollection<Pole> PlanszaWroga { get; set; }

        private string _status;
        public string Status { get => _status; set { _status = value; OnProp(); } }

        private bool _turaGracza;
        public bool TuraGracza { get => _turaGracza; set { _turaGracza = value; OnProp(); } }

        public Komenda ResetKomenda { get; set; }

        public GraStatki()
        {
            ResetKomenda = new Komenda(o => StartGry());
            StartGry();
        }

        private void StartGry()
        {
            PlanszaGracza = new ObservableCollection<Pole>();
            PlanszaWroga = new ObservableCollection<Pole>();

            for (int i = 0; i < ROZMIAR * ROZMIAR; i++)
            {
                PlanszaGracza.Add(new Pole(i % ROZMIAR, i / ROZMIAR, false, c => { }));
                PlanszaWroga.Add(new Pole(i % ROZMIAR, i / ROZMIAR, true, StrzalGracza));
            }

            OnProp(nameof(PlanszaGracza));
            OnProp(nameof(PlanszaWroga));

            RozmiescStatki(PlanszaGracza);
            RozmiescStatki(PlanszaWroga);

            TuraGracza = true;
            Status = "Twój ruch! Wybierz cel na planszy wroga.";
        }

        private async void StrzalGracza(Pole pole)
        {
            if (!TuraGracza || pole.AktualnyStan == Stan.Trafiony || pole.AktualnyStan == Stan.Pudlo) return;

            if (pole.AktualnyStan == Stan.Statek)
            {
                pole.AktualnyStan = Stan.Trafiony;
                SprawdzWygrana(PlanszaWroga, "GRACZ");
            }
            else
            {
                pole.AktualnyStan = Stan.Pudlo;
            }

            if (!SprawdzWygrana(PlanszaWroga, "GRACZ"))
            {
                TuraGracza = false;
                Status = "Ruch komputera...";
                await Task.Delay(500);
                RuchKomputera();
            }
        }

        private void RuchKomputera()
        {
            var dostepnePola = PlanszaGracza.Where(c => c.AktualnyStan != Stan.Trafiony && c.AktualnyStan != Stan.Pudlo).ToList();
            if (dostepnePola.Count == 0) return;

            var cel = dostepnePola[_los.Next(dostepnePola.Count)];

            if (cel.AktualnyStan == Stan.Statek)
                cel.AktualnyStan = Stan.Trafiony;
            else
                cel.AktualnyStan = Stan.Pudlo;

            if (!SprawdzWygrana(PlanszaGracza, "KOMPUTER"))
            {
                TuraGracza = true;
                Status = "Twój ruch!";
            }
        }

        private bool SprawdzWygrana(ObservableCollection<Pole> plansza, string kto)
        {
            if (!plansza.Any(c => c.AktualnyStan == Stan.Statek))
            {
                Status = $"KONIEC GRY! Wygrał: {kto}";
                TuraGracza = false;
                return true;
            }
            return false;
        }

        private void RozmiescStatki(ObservableCollection<Pole> plansza)
        {
            foreach (var p in plansza) p.AktualnyStan = Stan.Puste;

            foreach (var dlugosc in _statki)
            {
                bool ustawiono = false;
                while (!ustawiono)
                {
                    bool poziomo = _los.Next(2) == 0;
                    int x = _los.Next(ROZMIAR);
                    int y = _los.Next(ROZMIAR);

                    if (CzyMoznaPostawic(plansza, x, y, dlugosc, poziomo))
                    {
                        for (int i = 0; i < dlugosc; i++)
                        {
                            int idx = poziomo ? y * ROZMIAR + (x + i) : (y + i) * ROZMIAR + x;
                            plansza[idx].AktualnyStan = Stan.Statek;
                        }
                        ustawiono = true;
                    }
                }
            }
        }

        private bool CzyMoznaPostawic(ObservableCollection<Pole> plansza, int x, int y, int dlugosc, bool poziomo)
        {
            if (poziomo)
            {
                if (x + dlugosc > ROZMIAR) return false;
            }
            else
            {
                if (y + dlugosc > ROZMIAR) return false;
            }

            int startX = Math.Max(0, x - 1);
            int startY = Math.Max(0, y - 1);

            int koniecX = poziomo ? Math.Min(ROZMIAR - 1, x + dlugosc) : Math.Min(ROZMIAR - 1, x + 1);
            int koniecY = poziomo ? Math.Min(ROZMIAR - 1, y + 1) : Math.Min(ROZMIAR - 1, y + dlugosc);

            for (int r = startY; r <= koniecY; r++)
            {
                for (int c = startX; c <= koniecX; c++)
                {
                    if (plansza[r * ROZMIAR + c].AktualnyStan == Stan.Statek)
                        return false;
                }
            }

            return true;
        }
    }
}