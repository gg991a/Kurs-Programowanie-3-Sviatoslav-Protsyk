using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Lista_3
{
    public static class DataManager
    {
        private static readonly string FileName = "people.xml";

        public static void SavePeople(List<Person> people)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
            using (TextWriter writer = new StreamWriter(FileName))
            {
                serializer.Serialize(writer, people);
            }
        }

        public static List<Person> LoadPeople()
        {
            if (!File.Exists(FileName))
                return new List<Person>();

            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));
            using (FileStream stream = new FileStream(FileName, FileMode.Open))
            {
                return (List<Person>)serializer.Deserialize(stream);
            }
        }
    }
}