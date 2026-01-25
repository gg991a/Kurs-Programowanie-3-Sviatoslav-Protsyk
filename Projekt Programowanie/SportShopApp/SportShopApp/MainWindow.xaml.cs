using System;
using System.Windows;
using System.Windows.Controls;

namespace SportShopApp
{
    public partial class MainWindow : Window
    {
        DatabaseHelper db = new DatabaseHelper();
        User currentUser;

        public MainWindow(User user = null)
        {
            InitializeComponent();
            if (user != null)
            {
                currentUser = user;
                WelcomeText.Text = $"Witaj, {currentUser.Username}!";
            }
            LoadData();
        }

        private void LoadData()
        {
            try { ProductsList.ItemsSource = db.GetAllProducts(); }
            catch (Exception ex) { MessageBox.Show("Błąd: " + ex.Message); }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BuyItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Product selectedProduct = (Product)btn.Tag;

            if (selectedProduct == null) return;

            int amountToBuy = selectedProduct.QuantityToBuy;

            if (amountToBuy <= 0)
            {
                MessageBox.Show("Wpisz poprawną ilość (minimum 1)!");
                return;
            }

            try
            {
                if (currentUser != null)
                {
                    db.BuyProduct(currentUser, selectedProduct, amountToBuy);
                    MessageBox.Show($"Kupiłeś {amountToBuy} szt.: {selectedProduct.Name}!");
                    LoadData(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zakupu: " + ex.Message);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}