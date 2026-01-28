using WpfApp1.Model;
using System.ComponentModel;

namespace WpfApp1.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private Person _person;
        public Person Person => _person;

        public PersonViewModel(Person person)
        {
            _person = person;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FirstName
        {
            get => _person.FirstName;
            set { _person.FirstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        public string LastName
        {
            get => _person.LastName;
            set { _person.LastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public string PESEL
        {
            get => _person.PESEL;
            set { _person.PESEL = value; OnPropertyChanged(nameof(PESEL)); }
        }
    }
}