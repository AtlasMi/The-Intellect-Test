using System;
using System.Windows;
using System.Windows.Controls;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для SelectThemeWindow.xaml
    /// </summary>
    public partial class SelectThemeWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД
        int check_access = 0; //для проверки прав пользователя

        public SelectThemeWindow()
        {
            InitializeComponent();

            try
            {
                CheckAccess();

                if (check_access != 0)
                {
                    button_section1.Content = "Редактор теста";
                    ST.Title = "The Intellect Test | Проверка тестов";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void TransButton(Button but) //метод для начала теста
        {
            SetClass.Themes = but.Content.ToString();
            TestWindow tw = new TestWindow();
            tw.ShowDialog();
            Close();

            SetClass.CheckWindow++;
        }

        private void button_theme1_Click(object sender, RoutedEventArgs e)
        {
            TransButton(button_theme1);
        }

        private void button_theme2_Click(object sender, RoutedEventArgs e)
        {
            TransButton(button_theme2);
        }

        private void button_theme3_Click(object sender, RoutedEventArgs e)
        {
            TransButton(button_theme3);
        }

        private void button_section2_Click(object sender, RoutedEventArgs e)
        {
            SelectThemeSpecWindow stsw = new SelectThemeSpecWindow();
            stsw.ShowDialog();
        }

        private void button_section1_Click(object sender, RoutedEventArgs e)
        {
            SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
            stuw.ShowDialog();
        }

        private void button_theme5_Click(object sender, RoutedEventArgs e)
        {
            TransButton(button_theme5);
        }

        new int CheckAccess() //проверка на права пользователя
        {
            foreach (var acc in itestentities.Accounts)
            {
                if (check_access != 0)
                    break;
                else if (acc.Id == SetClass.Id && acc.Id_access_right == 1)
                { check_access++; break; }
            }

            return check_access;
        }
    }
}
