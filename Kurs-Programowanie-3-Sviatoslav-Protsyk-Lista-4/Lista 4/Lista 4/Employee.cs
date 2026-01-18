using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Lista_4
{
    [Serializable]
    public partial class Employee : ObservableObject
    {
        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _position = string.Empty;

        [ObservableProperty]
        private string _imagePath = string.Empty;

        public Employee() { }

        public Employee(string fName, string lName, string pos, string imgPath)
        {
            FirstName = fName;
            LastName = lName;
            Position = pos;
            ImagePath = imgPath;
        }
    }
}