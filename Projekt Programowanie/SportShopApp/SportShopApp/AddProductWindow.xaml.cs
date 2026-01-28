using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace SportShopApp
{
    public partial class AddProductWindow : Window
    {
        private string selectedImagePath = "";
        private Product productToEdit = null;
        public AddProductWindow(Product product = null)
        {
            InitializeComponent();

            if (product != null)
            {
                productToEdit = product;
                this.Title = "Edytuj produkt";

                NameTextBox.Text = product.Name;
                CategoryTextBox.Text = product.Category;
                PriceTextBox.Text = product.Price.ToString();
                QuantityTextBox.Text = product.Quantity.ToString();
                selectedImagePath = product.ImagePath;

                if (!string.IsNullOrEmpty(selectedImagePath))
                {
                    ImagePathTextBlock.Text = selectedImagePath;
                    try { ProductImagePreview.Source = new BitmapImage(new Uri(selectedImagePath)); } catch { }
                }
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;
                ImagePathTextBlock.Text = selectedImagePath;
                ProductImagePreview.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text)) return;
            if (!decimal.TryParse(PriceTextBox.Text, out decimal price)) return;
            if (!int.TryParse(QuantityTextBox.Text, out int quantity)) return;

            DatabaseHelper db = new DatabaseHelper();

            if (productToEdit == null)
            {
                Product newProduct = new Product
                {
                    Name = NameTextBox.Text,
                    Category = CategoryTextBox.Text,
                    Price = price,
                    Quantity = quantity,
                    ImagePath = selectedImagePath
                };
                db.AddProduct(newProduct);
                MessageBox.Show("Dodano!");
            }
            else
            {
                productToEdit.Name = NameTextBox.Text;
                productToEdit.Category = CategoryTextBox.Text;
                productToEdit.Price = price;
                productToEdit.Quantity = quantity;
                productToEdit.ImagePath = selectedImagePath;

                db.UpdateProduct(productToEdit);
                MessageBox.Show("Zapisano zmiany!");
            }

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}