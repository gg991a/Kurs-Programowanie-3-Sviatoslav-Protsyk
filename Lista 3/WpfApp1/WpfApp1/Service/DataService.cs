using WpfApp1.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace WpfApp1.Service
{
    public static class DataService
    {
        private static readonly string DataFilePath = "people.xml";

        public static ObservableCollection<Person> LoadPeople()
        {
            if (File.Exists(DataFilePath))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
                    using (var reader = new FileStream(DataFilePath, FileMode.Open))
                    {
                        return (ObservableCollection<Person>)(serializer.Deserialize(reader)
                                                              ?? new ObservableCollection<Person>());
                    }
                }
                catch { }
            }
            return new ObservableCollection<Person>();
        }

        public static void SavePeople(ObservableCollection<Person> people)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            using (var writer = new FileStream(DataFilePath, FileMode.Create))
            {
                serializer.Serialize(writer, people);
            }
        }
    }
}