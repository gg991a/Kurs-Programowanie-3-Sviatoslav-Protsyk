using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportShopApp
{
    public partial class MainWindow : Window
    {
        DatabaseHelper db = new DatabaseHelper();
        User currentUser;

        List<Product> allProducts = new List<Product>();
        List<CartItem> cartItems = new List<CartItem>();

        public MainWindow(User user = null)
        {
            InitializeComponent();
            currentUser = user ?? new User { Username = "Gość" };
            WelcomeText.Text = $"Witaj, {currentUser.Username}!";
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                allProducts = db.GetAllProducts();
                UpdateList();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void UpdateList()
        {
            if (allProducts == null) return;
            if (ProductsList == null) return;

            var filtered = allProducts.AsEnumerable();

            if (SearchBox != null && !string.IsNullOrEmpty(SearchBox.Text))
            {
                filtered = filtered.Where(p => p.Name != null && p.Name.ToLower().Contains(SearchBox.Text.ToLower()));
            }

            if (CategoryCombo != null && CategoryCombo.SelectedItem is ComboBoxItem selectedItem)
            {
                string category = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(category) && category != "Wszystkie")
                {
                    filtered = filtered.Where(p => p.Category == category);
                }
            }

            ProductsList.ItemsSource = filtered.ToList();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var product = (Product)btn.Tag;

            var existing = cartItems.FirstOrDefault(x => x.Product.Id == product.Id);

            if (existing != null)
            {
                if (existing.Quantity < product.Quantity) existing.Quantity++;
                else { MessageBox.Show("Brak więcej towaru na magazynie!"); return; }
            }
            else
            {
                cartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }
            RefreshCart();
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var item = (CartItem)btn.Tag;
            cartItems.Remove(item);
            RefreshCart();
        }

        private void RefreshCart()
        {
            CartList.ItemsSource = null;
            CartList.ItemsSource = cartItems;
            TotalTxt.Text = $"{cartItems.Sum(x => x.TotalPrice)} PLN";
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (cartItems.Count == 0) { MessageBox.Show("Twój koszyk jest pusty!"); return; }

            try
            {
                foreach (var item in cartItems)
                {
                    db.BuyProduct(currentUser, item.Product, item.Quantity);
                }
                MessageBox.Show("Dziękujemy za zakupy! Zapraszamy ponownie.");
                cartItems.Clear();
                RefreshCart();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zakupu: " + ex.Message);
                LoadData();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}