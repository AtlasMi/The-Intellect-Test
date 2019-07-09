using System;
using System.Windows;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBox_login.Text != "" || passwordBox_password.Password != "")
                {
                    int check = 0; //проверка на правильность данных
                    foreach (var acc in itestentities.Accounts)
                    {
                        if (textBox_login.Text == acc.Login && passwordBox_password.Password == acc.Password)
                        {
                            check++;
                            SetClass.Id = acc.Id;
                            SetClass.Login = textBox_login.Text;
                            SetClass.Password = passwordBox_password.Password;
                            SetClass.IdAva = acc.Id_ava;

                            if (acc.Id_access_right == 1)
                            { AdminWindow aw = new AdminWindow(); aw.ShowDialog(); Close(); }
                            else
                            { UserWindow uw = new UserWindow(); uw.ShowDialog(); Close(); }
                            break;
                        }
                    }

                    if (check == 0)
                        MessageBox.Show("Ошибка! Проверьте правильность данных!", "Аутентификация", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("Ошибка! Поля пусты", "Аутентификация", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_registry_Click(object sender, RoutedEventArgs e)
        {
            RegistryWindow rw = new RegistryWindow();
            rw.ShowDialog();

            if (SetClass.Login != "" && SetClass.Password != "")
            { textBox_login.Text = SetClass.Login; passwordBox_password.Password = SetClass.Password; }
        }
    }
}
