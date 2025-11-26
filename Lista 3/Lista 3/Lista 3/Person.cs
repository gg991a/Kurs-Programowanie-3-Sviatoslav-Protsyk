using System;

namespace Lista_3
{
    [Serializable]
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pesel { get; set; }

        public Person() { }

        public Person(string firstName, string lastName, string pesel)
        {
            FirstName = firstName;
            LastName = lastName;
            Pesel = pesel;
        }
    }
}