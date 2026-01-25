using System.Windows;

namespace SportShopApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelper db = new DatabaseHelper();
                User user = db.Login(UserBox.Text, PassBox.Password);

            if (user != null)
            {
                if (user.Username == "admin")
                {
                    AdminWindow adminWin = new AdminWindow();
                    adminWin.Show();
                }
                else
                {
                    MainWindow mainWin = new MainWindow(user);
                    mainWin.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Niepoprawny login lub hasło!");
            }
        }
    }
}