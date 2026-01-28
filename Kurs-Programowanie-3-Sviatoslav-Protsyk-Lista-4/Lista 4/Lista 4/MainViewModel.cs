using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Lista_4
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Employee> Employees { get; set; }

        [ObservableProperty]
        // Цей атрибут каже кнопкам "Edytuj" та "Usuń" оновити свій стан, коли ви змінили вибір у списку
        [NotifyCanExecuteChangedFor(nameof(EditEmployeeCommand), nameof(DeleteEmployeeCommand))]
        private Employee? _selectedEmployee;

        public MainViewModel()
        {
            Employees = new ObservableCollection<Employee>
            {
                new Employee("Test", "User", "Manager", "")
            };
        }

        [RelayCommand]
        private void AddEmployee()
        {
            var newEmployee = new Employee();
            var window = new EmployeeWindow(newEmployee);
            if (window.ShowDialog() == true)
            {
                Employees.Add(newEmployee);
            }
        }

        [RelayCommand(CanExecute = nameof(IsEmployeeSelected))]
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

        [RelayCommand(CanExecute = nameof(IsEmployeeSelected))]
        private void DeleteEmployee()
        {
            if (SelectedEmployee != null) Employees.Remove(SelectedEmployee);
        }

        private bool IsEmployeeSelected() => SelectedEmployee != null;

        [RelayCommand]
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

        [RelayCommand]
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
    }
}