using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        public UserWindow()
        {
            InitializeComponent();

            try
            {
                #region Проверка на первый вход
                int save_check = 0; //для проверки на сохранения 
                foreach (var entry in itestentities.Accounts)
                {
                    if (SetClass.Id == entry.Id && entry.Level == 0)
                    {
                        MessageBox.Show("Вы впервые вошли! Совeтую вам заглянуть в Помощь", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    }
                }
                #endregion

                #region Обработка информации о Уровне
                int max = 0; //кол-во слов
                foreach (var words in itestentities.Words)
                    max++;

                int mymax = 0; //кол-во слов в словаре
                foreach (var maxwords in itestentities.My_Dictionary)
                {
                    if (maxwords.Id_account == SetClass.Id)
                        mymax++;
                }

                int countlvl = 5; //~верхний предел уровня
                int arrcountlvl = countlvl + 1; //уровень для дальнейших решений
                int[] lvl = new int[arrcountlvl]; for (int i = 1; i < arrcountlvl; i++) lvl[i] = i; //массив для уровня и цикл для заполнения кол-ва уровней
                int[] lvldiv = new int[arrcountlvl]; //массив для определения уровня (деления)

                int j = 0;
                for (int i = 1; i < arrcountlvl; i++) //цикл для вычисления деления для каждого уровня (Например: кол-во слов - 30, уровень 1 - до 6 слов, уровень 2 - до 12 слов и т. д.)
                {
                    if (i != 1)
                    { j = i - 1; lvldiv[i] = max / countlvl + lvldiv[j]; }
                    else lvldiv[i] = max / countlvl;
                }

                save_check = 0; //для проверки на сохранения 
                foreach (var acc in itestentities.Accounts) //определение уровня с помощью деления
                {
                    if (SetClass.Id == acc.Id)
                    {
                        if (mymax == 0)
                        { save_check++; acc.Level = 1; break; }
                        else
                        {
                            for (int i = 1; i < arrcountlvl; i++)
                            {
                                if (mymax <= lvldiv[i])
                                { save_check++; acc.Level = lvl[i]; break; }
                            }
                            break;
                        }
                    }
                }

                if (save_check != 0)
                    itestentities.SaveChanges();
                #endregion

                #region Обработка информации о Очках
                List<int> Themes = new List<int>(); //создаём List и записываем в него Id каждой темы
                foreach (var themes in itestentities.Themes)
                    Themes.Add(themes.Id);

                int[] maxwordsTheme = new int[Themes.Count]; //создаём массив и записываем в него кол-во слов в каждой теме
                for (int i = 0; i < Themes.Count; i++)
                {
                    foreach (var wtheme in itestentities.Words)
                    {
                        if (Themes[i] == wtheme.Id_themes)
                            maxwordsTheme[i]++;
                    }
                }

                int score = 0; int[] mywordsTheme = new int[Themes.Count]; //создаём переменную и массив, в массив записываем кол-во слов каждой темы из словаря, в переменную записываем очки
                for (int i = 0; i < Themes.Count; i++)
                {
                    foreach (var wtheme in itestentities.Words)
                    {
                        foreach (var mywords in itestentities.My_Dictionary)
                        {
                            if (SetClass.Id == mywords.Id_account && mywords.Id_word == wtheme.Id && Themes[i] == wtheme.Id_themes)
                            { mywordsTheme[i]++; break; }
                        }
                    }
                    score += mywordsTheme[i];
                }

                for (int i = 0; i < Themes.Count; i++) //при всех правильных ответах одной темы прибавляется 10 очков
                {
                    if (maxwordsTheme[i] == mywordsTheme[i] && maxwordsTheme[i] != 0)
                        score += 10;
                }

                save_check = 0;
                foreach (var acc in itestentities.Accounts)
                {
                    if (acc.Id == SetClass.Id && acc.Score != score)
                    { save_check++; acc.Score = score; break; }
                }

                if (save_check != 0)
                    itestentities.SaveChanges();
                #endregion

                #region Загрузка информации из БД
                foreach (var acc in itestentities.Accounts)
                {
                    if (acc.Id == SetClass.Id)
                    {
                        textBlock_nick.Text = acc.Login;
                        textBlock_level.Text = acc.Level.ToString();
                        textBlock_score.Text = acc.Score.ToString();
                        image_ava.Source = new BitmapImage(new Uri(ImagesClass.NumberImage(acc.Id_ava).ToString(), UriKind.Relative));
                        break;
                    }
                }
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
                    UserWindow uw = new UserWindow();
                    uw.ShowDialog();
                    SetClass.CheckWindow = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_exam_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StudyWindow sw = new StudyWindow();
                sw.ShowDialog();
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

        private void button_diс_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int check_entry = 0;
                foreach (var words in itestentities.Accounts)
                {
                    if (SetClass.Id == words.Id && words.Score == 0)
                    {
                        check_entry++;
                        MessageBox.Show("Ошибка! Вы ещё не ответили правильно ни на один вопрос", "Мой словарь", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }

                if (check_entry == 0)
                {
                    MyDictionaryWindow mdw = new MyDictionaryWindow();
                    mdw.ShowDialog();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_stat_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow sw = new StatisticsWindow();
            sw.ShowDialog();
        }

        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditWindow ew = new EditWindow();
                ew.ShowDialog();
                Close();
                UserWindow uw = new UserWindow();
                uw.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}