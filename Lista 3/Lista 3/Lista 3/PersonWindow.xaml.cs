using System.Windows;

namespace Lista_3
{
    public partial class PersonWindow : Window
    {
        public Person PersonData { get; private set; }

        public PersonWindow()
        {
            InitializeComponent();
            PersonData = new Person();
        }

        public PersonWindow(Person personToEdit)
        {
            InitializeComponent();

            TxtFirstName.Text = personToEdit.FirstName;
            TxtLastName.Text = personToEdit.LastName;
            TxtPesel.Text = personToEdit.Pesel;

            PersonData = personToEdit;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                string.IsNullOrWhiteSpace(TxtPesel.Text))
            {
                MessageBox.Show("Proszę wypełnić wszystkie pola!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PersonData.FirstName = TxtFirstName.Text;
            PersonData.LastName = TxtLastName.Text;
            PersonData.Pesel = TxtPesel.Text;

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}