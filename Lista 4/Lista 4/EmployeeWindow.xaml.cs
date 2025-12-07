using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Lista_4
{
    public partial class EmployeeWindow : Window
    {
        public Employee EmployeeData { get; private set; }
        private string selectedImagePath;

        public EmployeeWindow()
        {
            InitializeComponent();
            EmployeeData = new Employee();
        }

        public EmployeeWindow(Employee employee) : this()
        {
            txtFirstName.Text = employee.FirstName;
            txtLastName.Text = employee.LastName;
            txtPosition.Text = employee.Position;
            selectedImagePath = employee.ImagePath;

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                DisplayImage(selectedImagePath);
            }
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (dlg.ShowDialog() == true)
            {
                selectedImagePath = dlg.FileName;
                DisplayImage(selectedImagePath);
            }
        }

        private void DisplayImage(string path)
        {
            try
            {
                imgPreview.Source = new BitmapImage(new Uri(path));
            }
            catch
            {
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            EmployeeData.FirstName = txtFirstName.Text;
            EmployeeData.LastName = txtLastName.Text;
            EmployeeData.Position = txtPosition.Text;
            EmployeeData.ImagePath = selectedImagePath;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}