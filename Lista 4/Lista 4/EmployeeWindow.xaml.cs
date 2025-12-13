using Microsoft.Win32;
using System.Windows;

namespace Lista_4
{
    public partial class EmployeeWindow : Window
    {
        private Employee _employee;

        public EmployeeWindow(Employee employee)
        {
            InitializeComponent();
            _employee = employee;
            DataContext = _employee;
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
            if (dlg.ShowDialog() == true)
            {
                _employee.ImagePath = dlg.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
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