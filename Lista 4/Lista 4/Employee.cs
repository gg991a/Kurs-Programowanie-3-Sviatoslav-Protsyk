using System;

namespace Lista_4
{
    [Serializable] 
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; } 
        public string ImagePath { get; set; } 

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