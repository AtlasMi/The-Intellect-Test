using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД
        int check_access = 0; //для проверки прав пользователя

        public StatisticsWindow()
        {
            InitializeComponent();

            CheckAccess();

            try
            {
                #region Загрузка информации из БД
                if (check_access == 0)
                {
                    foreach (var login in itestentities.Accounts)
                    {
                        if (login.Id == SetClass.Id)
                        {
                            textBlock_nick.Text += login.Login;
                            textBlock_level.Text += login.Level;
                            textBlock_score.Text += login.Score;
                            image_ava.Source = new BitmapImage(new Uri(ImagesClass.NumberImage(login.Id_ava).ToString(), UriKind.Relative));
                            break;
                        }
                    }
                }
                else
                {
                    Title = "The Intellect Test | Подробные данные";
                    textBlock_stu.Text = "Данные";

                    foreach (var login in itestentities.Accounts)
                    {
                        if (login.Id == SetClass.Id)
                        {
                            textBlock_nick.Text += login.Login;
                            textBlock_level.Opacity = 0;
                            textBlock_score.Opacity = 0;
                            textBlock_stuthemes.Opacity = 0;
                            Bordinf.Opacity = 0;
                            image_ava.Source = new BitmapImage(new Uri(ImagesClass.NumberImage(login.Id_ava).ToString(), UriKind.Relative));
                            break;
                        }
                    }
                }
                #endregion

                #region Обработка статистики
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

                int[] mySection = new int[3]; int j = -1; //создаём массив и переменную, массив для записи кол-ва слов в каждом разделе, переменную для условия в цикле

                if (check_access == 0) //пользователь
                {
                    int[] mywordsTheme = new int[Themes.Count]; //создаём массив и записываем кол-во слов каждой темы из словаря
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
                    }

                    while (j < mySection.Length) //вычисляем кол-во изученных слов в каждом разделе
                    {
                        foreach (var section in itestentities.Section)
                        {
                            j++;
                            if (j < 3)
                            {
                                for (int i = 0; i < Themes.Count; i++)
                                {
                                    foreach (var themes in itestentities.Themes)
                                    {
                                        if (Themes[i] == themes.Id && themes.Id_section == section.Id)
                                        { mySection[j] += mywordsTheme[i]; break; }
                                    }
                                }
                            }
                            else break;
                        }
                    }

                    int check_stu = 0;
                    for (int i = 0; i < Themes.Count; i++) //при всех правильных ответах одной темы меняется значение
                    {
                        if (maxwordsTheme[i] == mywordsTheme[i] && maxwordsTheme[i] != 0)
                            check_stu++;
                    }
                    textBlock_stuthemes.Text += check_stu;

                    foreach (var themes in itestentities.Themes) //добавление изученных слов по тематикам
                    {
                        for (int i = 0; i < Themes.Count; i++)
                        {
                            if (themes.Id == Themes[i] && themes.Name == textBlock_phil.Text)
                                textBlock_phil.Text += string.Format(": {0}", mywordsTheme[i]);
                            else if (themes.Id == Themes[i] && themes.Name == textBlock_rus.Text)
                                textBlock_rus.Text += string.Format(": {0}", mywordsTheme[i]);
                            else if (themes.Id == Themes[i] && themes.Name == textBlock_bio.Text)
                                textBlock_bio.Text += string.Format(": {0}", mywordsTheme[i]);
                            else if (themes.Id == Themes[i] && themes.Name == textBlock_it.Text)
                                textBlock_it.Text += string.Format(": {0}", mywordsTheme[i]);
                        }
                    }
                }
                else //администратор
                {
                    while (j < mySection.Length) //вычисляем кол-во слов в каждом разделе
                    {
                        foreach (var section in itestentities.Section)
                        {
                            j++;
                            if (j < 3)
                            {
                                for (int i = 0; i < Themes.Count; i++)
                                {
                                    foreach (var themes in itestentities.Themes)
                                    {
                                        if (Themes[i] == themes.Id && themes.Id_section == section.Id)
                                        { mySection[j] += maxwordsTheme[i]; break; }
                                    }
                                }
                            }
                            else break;
                        }
                    }

                    foreach (var themes in itestentities.Themes) //отображение изученных слов по тематикам
                    {
                        for (int i = 0; i < Themes.Count; i++)
                        {
                            if (themes.Id == Themes[i] && themes.Name == textBlock_phil.Text)
                                textBlock_phil.Text += string.Format(": {0}", maxwordsTheme[i]);
                            else if (themes.Id == Themes[i] && themes.Name == textBlock_rus.Text)
                                textBlock_rus.Text += string.Format(": {0}", maxwordsTheme[i]);
                            else if (themes.Id == Themes[i] && themes.Name == textBlock_bio.Text)
                                textBlock_bio.Text += string.Format(": {0}", maxwordsTheme[i]);
                            else if (themes.Id == Themes[i] && themes.Name == textBlock_it.Text)
                                textBlock_it.Text += string.Format(": {0}", maxwordsTheme[i]);
                        }
                    }
                }

                foreach (var section in itestentities.Section) //отображение кол-во слов в разделах
                {
                    for (int i = 0; i < mySection.Length; i++)
                    {
                        if (i == 0 && section.Name == textBlock_gen.Text)
                            textBlock_gen.Text += string.Format(": {0}", mySection[i]);
                        else if (i == 1 && section.Name == textBlock_spec.Text)
                            textBlock_spec.Text += string.Format(": {0}", mySection[i]);
                        else if (i == 2 && section.Name == textBlock_user.Text)
                            textBlock_user.Text += string.Format(": {0}", mySection[i]);
                    }
                }
                #endregion
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
