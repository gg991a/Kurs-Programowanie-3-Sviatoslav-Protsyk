using WpfApp1.Model;
using WpfApp1.ViewModel;
using WpfApp1.Util;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)this.DataContext;

        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainViewModel();
            this.DataContext = viewModel;

            viewModel.AddPersonCommand = new RelayCommand(p => OpenPersonWindowForAdd(viewModel));
            viewModel.EditPersonCommand = new RelayCommand(p => OpenPersonWindowForEdit(viewModel),
                                                           p => viewModel.SelectedPerson != null);

            this.Closing += (s, args) => viewModel.SaveCommand.Execute(null);
        }

        private void OpenPersonWindowForAdd(MainViewModel mainVM)
        {
            var newPerson = new Person();
            var personVM = new PersonViewModel(newPerson);

            var personWindow = new PersonWindow { DataContext = personVM };

            if (personWindow.ShowDialog() == true)
            {
                mainVM.People.Add(newPerson);
            }
        }

        private void OpenPersonWindowForEdit(MainViewModel mainVM)
        {
            var originalPerson = mainVM.SelectedPerson;
            if (originalPerson == null) return;

            var personVM = new PersonViewModel(originalPerson);
            var personWindow = new PersonWindow { DataContext = personVM };

            personWindow.ShowDialog();
        }
    }
}