using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Lista_4
{
    public class MainViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public ObservableCollection<Employee> Employees { get; set; }

        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveXmlCommand { get; }
        public ICommand LoadXmlCommand { get; }

        public MainViewModel()
        {
            Employees = new ObservableCollection<Employee>
            {
                new Employee("Test", "User", "Manager", "")
            };

            AddCommand = new RelayCommand(_ => AddEmployee());
            EditCommand = new RelayCommand(_ => EditEmployee(), _ => SelectedEmployee != null);
            DeleteCommand = new RelayCommand(_ => DeleteEmployee(), _ => SelectedEmployee != null);
            SaveXmlCommand = new RelayCommand(_ => SaveXml());
            LoadXmlCommand = new RelayCommand(_ => LoadXml());
        }

        private void AddEmployee()
        {
            var newEmployee = new Employee();
            var window = new EmployeeWindow(newEmployee);
            if (window.ShowDialog() == true)
            {
                Employees.Add(newEmployee);
            }
        }

        private void EditEmployee()
        {
            if (SelectedEmployee == null) return;

            var tempEmployee = new Employee(
                SelectedEmployee.FirstName,
                SelectedEmployee.LastName,
                SelectedEmployee.Position,
                SelectedEmployee.ImagePath);

            var window = new EmployeeWindow(tempEmployee);

            if (window.ShowDialog() == true)
            {
                SelectedEmployee.FirstName = tempEmployee.FirstName;
                SelectedEmployee.LastName = tempEmployee.LastName;
                SelectedEmployee.Position = tempEmployee.Position;
                SelectedEmployee.ImagePath = tempEmployee.ImagePath;
            }
        }

        private void DeleteEmployee()
        {
            if (SelectedEmployee != null) Employees.Remove(SelectedEmployee);
        }

        private void SaveXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Employee>));
                using (TextWriter writer = new StreamWriter("pracownicy.xml"))
                {
                    serializer.Serialize(writer, Employees);
                }
                MessageBox.Show("Zapisano pomyślnie!");
            }
            catch (System.Exception ex) { MessageBox.Show($"Błąd: {ex.Message}"); }
        }

        private void LoadXml()
        {
            if (!File.Exists("pracownicy.xml")) return;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Employee>));
                using (FileStream fs = new FileStream("pracownicy.xml", FileMode.Open))
                {
                    var loaded = (ObservableCollection<Employee>)serializer.Deserialize(fs);
                    Employees.Clear();
                    foreach (var emp in loaded) Employees.Add(emp);
                }
                MessageBox.Show("Wczytano dane.");
            }
            catch (System.Exception ex) { MessageBox.Show($"Błąd: {ex.Message}"); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
        }
    }
}