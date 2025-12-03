using WpfApp1.Model;
using WpfApp1.Service;
using WpfApp1.Util;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfApp1.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; }

        private Person? _selectedPerson; 
        public Person? SelectedPerson 
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                (EditPersonCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPersonCommand { get; set; }
        public ICommand EditPersonCommand { get; set; }
        public ICommand SaveCommand { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            People = DataService.LoadPeople();
            SaveCommand = new RelayCommand(p => DataService.SavePeople(People));

            AddPersonCommand = new RelayCommand(p => { });
            EditPersonCommand = new RelayCommand(p => { }, p => SelectedPerson != null);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}