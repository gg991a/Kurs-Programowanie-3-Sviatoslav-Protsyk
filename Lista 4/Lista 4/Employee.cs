using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lista_4
{
    [Serializable]
    public class Employee : INotifyPropertyChanged
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _position = string.Empty;
        private string _imagePath = string.Empty;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }

        public string ImagePath
        {
            get => _imagePath;
            set { _imagePath = value; OnPropertyChanged(); }
        }

        public Employee() { }

        public Employee(string fName, string lName, string pos, string imgPath)
        {
            FirstName = fName;
            LastName = lName;
            Position = pos;
            ImagePath = imgPath;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}