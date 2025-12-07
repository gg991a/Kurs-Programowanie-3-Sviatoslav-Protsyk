using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Lista_4
{
    public partial class MainWindow : Window
    {
        // Używamy ObservableCollection, aby ListView automatycznie widział zmiany
        public ObservableCollection<Employee> Employees { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Employees = new ObservableCollection<Employee>();

            // Przykładowe dane
            Employees.Add(new Employee("Jan", "Kowalski", "Kierownik", ""));

            lvEmployees.ItemsSource = Employees;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var window = new EmployeeWindow();
            if (window.ShowDialog() == true)
            {
                Employees.Add(window.EmployeeData);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem is Employee selectedEmployee)
            {
                var window = new EmployeeWindow(selectedEmployee);
                if (window.ShowDialog() == true)
                {
                    // Aktualizacja danych na liście (proste nadpisanie właściwości)
                    // W MVVM z INotifyPropertyChanged działoby się to automatycznie,
                    // tutaj dla uproszczenia odświeżamy listę:
                    int index = Employees.IndexOf(selectedEmployee);
                    Employees[index] = window.EmployeeData;
                }
            }
            else
            {
                MessageBox.Show("Wybierz pracownika do edycji.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (lvEmployees.SelectedItem is Employee selectedEmployee)
            {
                Employees.Remove(selectedEmployee);
            }
        }

        // === Serializacja XML ===

        private void SaveXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Employee>));
                using (TextWriter writer = new StreamWriter("pracownicy.xml"))
                {
                    serializer.Serialize(writer, Employees);
                }
                MessageBox.Show("Zapisano dane do pliku pracownicy.xml");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Błąd zapisu: {ex.Message}");
            }
        }

        private void LoadXML_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("pracownicy.xml"))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Employee>));
                    using (FileStream fs = new FileStream("pracownicy.xml", FileMode.Open))
                    {
                        var loadedData = (ObservableCollection<Employee>)serializer.Deserialize(fs);
                        Employees.Clear();
                        foreach (var emp in loadedData)
                        {
                            Employees.Add(emp);
                        }
                    }
                    MessageBox.Show("Wczytano dane.");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Błąd odczytu: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Plik nie istnieje.");
            }
        }
    }
}