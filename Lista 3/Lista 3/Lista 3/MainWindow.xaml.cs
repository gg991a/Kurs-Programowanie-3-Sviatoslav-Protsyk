using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Lista_3
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Person> _peopleList;

        public MainWindow()
        {
            InitializeComponent();
            _peopleList = new ObservableCollection<Person>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Person> loadedData = DataManager.LoadPeople();
            foreach (var p in loadedData)
            {
                _peopleList.Add(p);
            }
            DgPeople.ItemsSource = _peopleList;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            PersonWindow addWindow = new PersonWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                _peopleList.Add(addWindow.PersonData);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgPeople.SelectedItem is Person selectedPerson)
            {
                PersonWindow editWindow = new PersonWindow(selectedPerson);
                editWindow.Owner = this;

                if (editWindow.ShowDialog() == true)
                {
                    DgPeople.Items.Refresh(); 
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać osobę do edycji.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DgPeople.SelectedItem is Person selectedPerson)
            {
                var result = MessageBox.Show($"Czy na pewno chcesz usunąć osobę: {selectedPerson.LastName}?",
                                             "Potwierdzenie usunięcia",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _peopleList.Remove(selectedPerson);
                }
            }
            else
            {
                MessageBox.Show("Proszę wybrać osobę do usunięcia.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DataManager.SavePeople(new List<Person>(_peopleList));
        }
    }
}