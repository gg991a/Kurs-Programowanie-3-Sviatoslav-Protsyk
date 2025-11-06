using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Lista_2
{
    public partial class Zad3 : Window
    {
        private const int ROZMIAR_SIATKI = 10;
        private enum StanKomorki { Puste, Statek, Trafiony, Pudlo }

        private StanKomorki[,] planszaGracza = null!;
        private StanKomorki[,] planszaWroga = null!;
        private Button[,] komorkiUiGracza = null!;
        private Random losowy = new Random();

        private readonly List<int> rozmiaryOkretow = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        public Zad3()
        {
            InitializeComponent();
            InicjalizujGre();
        }

        private void InicjalizujGre()
        {
            planszaGracza = new StanKomorki[ROZMIAR_SIATKI, ROZMIAR_SIATKI];
            planszaWroga = new StanKomorki[ROZMIAR_SIATKI, ROZMIAR_SIATKI];
            komorkiUiGracza = new Button[ROZMIAR_SIATKI, ROZMIAR_SIATKI];

            RozmiescOkrety(planszaGracza);
            RozmiescOkrety(planszaWroga);

            RysujPlanszeGracza();
            RysujPlanszeWroga();
            TekstStatusu.Text = "Twoja tura. Wybierz cel.";
            SiatkaWroga.IsEnabled = true;
        }

        private void RysujPlanszeGracza()
        {
            SiatkaGracza.Children.Clear();
            for (int r = 0; r < ROZMIAR_SIATKI; r++)
            {
                for (int c = 0; c < ROZMIAR_SIATKI; c++)
                {
                    var button = new Button
                    {
                        Margin = new Thickness(0.5),
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(0.5),
                        IsEnabled = false,
                        Background = PobierzPedzelGracza(planszaGracza[r, c]),
                        Content = PobierzZnakGracza(planszaGracza[r, c])
                    };

                    komorkiUiGracza[r, c] = button;
                    SiatkaGracza.Children.Add(button);
                }
            }
        }

        private void RysujPlanszeWroga()
        {
            SiatkaWroga.Children.Clear();
            for (int r = 0; r < ROZMIAR_SIATKI; r++)
            {
                for (int c = 0; c < ROZMIAR_SIATKI; c++)
                {
                    var button = new Button
                    {
                        Margin = new Thickness(1),
                        Tag = new Point(c, r),
                        Background = PobierzPedzelWroga(planszaWroga[r, c]),
                        Content = PobierzZnakWroga(planszaWroga[r, c])
                    };

                    button.Click += KomorkaWroga_Click;

                    if (planszaWroga[r, c] == StanKomorki.Trafiony || planszaWroga[r, c] == StanKomorki.Pudlo)
                        button.IsEnabled = false;

                    SiatkaWroga.Children.Add(button);
                }
            }
        }

        private Brush PobierzPedzelGracza(StanKomorki stan)
        {
            switch (stan)
            {
                case StanKomorki.Trafiony:
                    return Brushes.DarkRed;     
                case StanKomorki.Pudlo:
                    return Brushes.LightGray;   
                default:
                    return Brushes.White;       
            }
        }

        private Brush PobierzPedzelWroga(StanKomorki stan)
        {
            switch (stan)
            {
                case StanKomorki.Trafiony:
                    return Brushes.Crimson;
                case StanKomorki.Pudlo:
                    return Brushes.LightSteelBlue;
                default:
                    return Brushes.AliceBlue;
            }
        }

        private string PobierzZnakGracza(StanKomorki stan)
        {
            switch (stan)
            {
                case StanKomorki.Statek: return "O"; 
                case StanKomorki.Trafiony: return "X";
                case StanKomorki.Pudlo: return "•";
                default: return "";
            }
        }

        private string PobierzZnakWroga(StanKomorki stan)
        {
            switch (stan)
            {
                case StanKomorki.Trafiony: return "X";
                case StanKomorki.Pudlo: return "•";
                default: return "";
            }
        }

        private void RozmiescOkrety(StanKomorki[,] plansza)
        {
            foreach (int rozmiar in rozmiaryOkretow)
            {
                bool umieszczono = false;
                while (!umieszczono)
                {
                    bool poziomo = losowy.Next(2) == 0;
                    int r = losowy.Next(ROZMIAR_SIATKI);
                    int c = losowy.Next(ROZMIAR_SIATKI);

                    if (CzyMoznaUmiescic(plansza, r, c, rozmiar, poziomo))
                    {
                        for (int i = 0; i < rozmiar; i++)
                        {
                            if (poziomo) plansza[r, c + i] = StanKomorki.Statek;
                            else plansza[r + i, c] = StanKomorki.Statek;
                        }
                        umieszczono = true;
                    }
                }
            }
        }

        private bool CzyMoznaUmiescic(StanKomorki[,] plansza, int r, int c, int rozmiar, bool poziomo)
        {
            if (poziomo)
            {
                if (c + rozmiar > ROZMIAR_SIATKI) return false;
                for (int i = 0; i < rozmiar; i++)
                    if (plansza[r, c + i] != StanKomorki.Puste) return false;
            }
            else
            {
                if (r + rozmiar > ROZMIAR_SIATKI) return false;
                for (int i = 0; i < rozmiar; i++)
                    if (plansza[r + i, c] != StanKomorki.Puste) return false;
            }
            return true;
        }

        private async void KomorkaWroga_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var coords = (Point)button.Tag;
            int r = (int)coords.Y;
            int c = (int)coords.X;

            PrzetworzStrzal(planszaWroga, r, c);
            button.Background = PobierzPedzelWroga(planszaWroga[r, c]);
            button.Content = PobierzZnakWroga(planszaWroga[r, c]);
            button.IsEnabled = false;

            if (SprawdzZwyciezce(planszaWroga, "Gracz")) return;

            SiatkaWroga.IsEnabled = false;
            TekstStatusu.Text = "Tura Komputera...";
            await Task.Delay(500);
            TuraKomputera();
        }

        private void TuraKomputera()
        {
            int r, c;
            do
            {
                r = losowy.Next(ROZMIAR_SIATKI);
                c = losowy.Next(ROZMIAR_SIATKI);
            } while (planszaGracza[r, c] == StanKomorki.Trafiony || planszaGracza[r, c] == StanKomorki.Pudlo);

            PrzetworzStrzal(planszaGracza, r, c);
            komorkiUiGracza[r, c].Background = PobierzPedzelGracza(planszaGracza[r, c]);
            komorkiUiGracza[r, c].Content = PobierzZnakGracza(planszaGracza[r, c]);

            if (SprawdzZwyciezce(planszaGracza, "Komputer")) return;

            TekstStatusu.Text = "Twoja tura. Wybierz cel.";
            SiatkaWroga.IsEnabled = true;
        }

        private void PrzetworzStrzal(StanKomorki[,] plansza, int r, int c)
        {
            if (plansza[r, c] == StanKomorki.Statek)
                plansza[r, c] = StanKomorki.Trafiony;
            else if (plansza[r, c] == StanKomorki.Puste)
                plansza[r, c] = StanKomorki.Pudlo;
        }

        private bool SprawdzZwyciezce(StanKomorki[,] plansza, string nazwa)
        {
            for (int r = 0; r < ROZMIAR_SIATKI; r++)
                for (int c = 0; c < ROZMIAR_SIATKI; c++)
                    if (plansza[r, c] == StanKomorki.Statek)
                        return false;

            TekstStatusu.Text = $"Koniec gry! {nazwa} wygrywa!";
            SiatkaWroga.IsEnabled = false;
            MessageBox.Show(TekstStatusu.Text, "Koniec Gry");
            return true;
        }

        private void PrzyciskNowaGra_Click(object sender, RoutedEventArgs e)
        {
            InicjalizujGre();
        }
    }
}
