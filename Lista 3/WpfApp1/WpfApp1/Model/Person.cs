using System.ComponentModel;
using System.Xml.Serialization;

namespace WpfApp1.Model
{
    public class Person : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _pesel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Person()
        {
            _firstName = string.Empty;
            _lastName = string.Empty;
            _pesel = string.Empty;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string PESEL
        {
            get => _pesel;
            set
            {
                if (_pesel != value)
                {
                    _pesel = value;
                    OnPropertyChanged(nameof(PESEL));
                }
            }
        }
    }
}