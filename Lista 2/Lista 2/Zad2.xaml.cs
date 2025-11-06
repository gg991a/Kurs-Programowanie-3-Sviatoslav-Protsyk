using System.Windows;
using System.Windows.Controls;

namespace Lista_2
{
    public partial class Zad2 : Window
    {
        private bool isPlayerXTurn = true;
        private bool gameEnded = false;
        private int moveCount = 0;

        public Zad2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameEnded)
            {
                return;
            }

            Button clickedButton = sender as Button;

            if (clickedButton.Content != null)
            {
                return;
            }

            string currentPlayerSymbol = isPlayerXTurn ? "X" : "O";
            clickedButton.Content = currentPlayerSymbol;
            moveCount++;

            if (CheckForWinner(currentPlayerSymbol))
            {
                gameEnded = true;
                MessageBox.Show($"Gratulacje! Wygrał gracz {currentPlayerSymbol}!", "Koniec gry");
                DisableBoard();
                return;
            }

            if (moveCount == 9)
            {
                gameEnded = true;
                MessageBox.Show("Remis! Nikt nie wygrał.", "Koniec gry");
                DisableBoard();
                return;
            }

            isPlayerXTurn = !isPlayerXTurn;
        }

        private bool CheckForWinner(string playerSymbol)
        {
            bool CheckLine(Button b1, Button b2, Button b3)
            {
                return b1.Content?.ToString() == playerSymbol &&
                       b2.Content?.ToString() == playerSymbol &&
                       b3.Content?.ToString() == playerSymbol;
            }

            if (CheckLine(Button0_0, Button0_1, Button0_2)) return true;
            if (CheckLine(Button1_0, Button1_1, Button1_2)) return true;
            if (CheckLine(Button2_0, Button2_1, Button2_2)) return true;

            if (CheckLine(Button0_0, Button1_0, Button2_0)) return true;
            if (CheckLine(Button0_1, Button1_1, Button2_1)) return true;
            if (CheckLine(Button0_2, Button1_2, Button2_2)) return true;

            if (CheckLine(Button0_0, Button1_1, Button2_2)) return true;
            if (CheckLine(Button0_2, Button1_1, Button2_0)) return true;

            return false;
        }

        private void DisableBoard()
        {
            foreach (UIElement element in GameBoard.Children)
            {
                if (element is Button button)
                {
                    button.IsEnabled = false;
                }
            }
        }
    }
}