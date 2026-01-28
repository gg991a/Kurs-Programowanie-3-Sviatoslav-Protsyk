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
            if (AdminProductsList != null)
                AdminProductsList.ItemsSource = db.GetAllProducts();

            if (OrdersList != null)
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
            Product p = (Product)btn.Tag;

            AddProductWindow editWin = new AddProductWindow(p);
            editWin.ShowDialog();
            LoadAllData();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            Product p = (Product)btn.Tag;

            var res = MessageBox.Show($"Usunąć produkt: {p.Name}?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (res == MessageBoxResult.Yes)
            {
                try
                {
                    db.DeleteProduct(p.Id);
                    LoadAllData();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Nie można usunąć! Produkt jest w historii zamówień.\n" + ex.Message);
                }
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }
    }
}