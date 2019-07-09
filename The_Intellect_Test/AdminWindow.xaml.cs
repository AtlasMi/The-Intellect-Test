using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        public AdminWindow()
        {
            InitializeComponent();

            try
            {
                int check_save = 0;

                #region Загрузка информации из БД
                foreach (var acc in itestentities.Accounts)
                {
                    if (acc.Id == SetClass.Id)
                    {
                        textBlock_nick.Text = acc.Login;
                        image_ava.Source = new BitmapImage(new Uri(ImagesClass.NumberImage(acc.Id_ava).ToString(), UriKind.Relative));
                        break;
                    }
                }
                #endregion

                #region Очистка данных
                check_save = 0;
                foreach (var acc in itestentities.Accounts)
                {
                    if (acc.Id == SetClass.Id)
                    { acc.Level = 0; acc.Score = 0; check_save++; }
                }

                if (check_save == 0)
                    itestentities.SaveChanges();
                #endregion

                #region Кол-во пользоватлей
                int users = 0;
                foreach (var userss in itestentities.Accounts)
                    users++;
                textBlock_users.Text += users;
                #endregion

                #region Кол-во разделов
                int section = 0;
                foreach (var sectionn in itestentities.Section)
                    section++;
                textBlock_section.Text += section;
                #endregion

                #region Кол-во тематик
                int themes = 0;
                foreach (var themess in itestentities.Themes)
                    themes++;
                textBlock_themes.Text += themes;
                #endregion

                #region Кол-во слов
                int words = 0;
                foreach (var wordss in itestentities.Words)
                    words++;
                textBlock_words.Text += words;
                #endregion

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectThemeWindow stw = new SelectThemeWindow();
                stw.ShowDialog();

                if (SetClass.CheckWindow != 0)
                {
                    Close();
                    AdminWindow uw = new AdminWindow();
                    uw.ShowDialog();
                    SetClass.CheckWindow = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void button_faq_Click(object sender, RoutedEventArgs e)
        {
            FAQWindow fw = new FAQWindow();
            fw.ShowDialog();
        }

        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditWindow ew = new EditWindow();
                ew.ShowDialog();
                Close();

                AdminWindow uw = new AdminWindow();
                uw.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_detinf_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow sw = new StatisticsWindow();
            sw.ShowDialog();
        }
    }
}
