using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SportShopApp
{
    public partial class AdminWindow : Window
    {
        DatabaseHelper db = new DatabaseHelper();

        public AdminWindow()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            AdminProductsList.ItemsSource = db.GetAllProducts();
            OrdersList.ItemsSource = db.GetAllOrders();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAllData();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addWin = new AddProductWindow();
            addWin.ShowDialog();
            LoadAllData();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Product productToEdit = (Product)btn.Tag;

            AddProductWindow editWin = new AddProductWindow(productToEdit);
            editWin.ShowDialog();
            LoadAllData(); 
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void Export_Click(object sender, RoutedEventArgs e) { }
    }
}