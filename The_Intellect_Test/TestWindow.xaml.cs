using System;
using System.Windows;
using System.Windows.Controls;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        int maxwords = 0; //кол-во слов темы
        int curword = 1; //текущее слово
        int idword = 0; //id слова
        int corwords = 0; //количество правильный ответов
        int check_access = 0; //для проверки прав пользователя

        public TestWindow()
        {
            InitializeComponent();

            CheckAccess();

            try
            {
                textBlock_themes.Text += SetClass.Themes; //тематика

                foreach (var themes in itestentities.Themes)
                {
                    if (themes.Name == SetClass.Themes)
                    {
                        foreach (var sec in itestentities.Section) //если тест пользовательский отображение соответсвующего текста
                        {
                            if (themes.Id_section == sec.Id && themes.Name == "Пользовательский")
                                textBlock_authenticity.Text = "(не проверено)";
                        }

                        foreach (var acc in itestentities.Accounts) //отображение авторства
                        {
                            if (themes.Id_user == acc.Id)
                                textBlock_creator.Text = string.Format("{0} ©", acc.Login);
                        }

                        foreach (var words in itestentities.Words) //кол - во слов темы
                        {
                            if (words.Id_themes == themes.Id)
                                maxwords++;
                        }
                    }
                }

                textBlock_num3.Text = maxwords.ToString(); //макс. кол-во слов темы

                LoadingData();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void SaveData(TextBlock but)
        {
            try
            {
                int check_coin = 0;
                foreach (var mydic in itestentities.My_Dictionary) //проверка на совпадение слов в словаре
                {
                    if (mydic.Id_account == SetClass.Id && mydic.Id_word == idword)
                    { check_coin++; break; }
                }

                int save = 0;

                foreach (var words in itestentities.Words) //проверка на правильность ответа
                {
                    if (check_coin == 0 && words.Id == idword && but.Text == words.Answer)
                    {
                        save++;
                        var add_answer = new My_Dictionary();
                        add_answer.Id_account = SetClass.Id;
                        add_answer.Id_word = idword;
                        itestentities.My_Dictionary.Add(add_answer);
                    }
                }

                if (save != 0 && check_access == 0) //проверка на сохранение
                    itestentities.SaveChanges();

                if (check_coin != 0 || save != 0) //проверка на правильность слова для переменной
                    corwords++;

                if (curword > maxwords) //проверка на завершение теста
                { MessageWin(); Close(); }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void LoadingData()
        {
            try
            {
                textBlock_1variant.Text = null;
                textBlock_2variant.Text = null;
                textBlock_3variant.Text = null;
                textBlock_4variant.Text = null;

                int selcurwords = 1; //для выбора текущего слова
                Random r = new Random(); //для случайного расположения варинтов ответа
                int[] variant = { 5, 5, 5, 5 }; //массив для сравнения для случайного располжения

                foreach (var themes in itestentities.Themes)
                {
                    if (themes.Name == SetClass.Themes)
                    {
                        foreach (var words in itestentities.Words)
                        {
                            if (words.Id_themes == themes.Id)
                            {
                                if (selcurwords != curword) //проверка на порядковый номер вопроса
                                    selcurwords++;
                                else
                                {
                                    string[] random_variation = { words.Answer, words.Variation_2, words.Variation_3, words.Variation_4 }; //варианты ответа

                                    textBlock_question.Text = words.Question; //Вопрос
                                    textBlock_num.Text = curword.ToString(); //Порядковый номер вопроса

                                    int i = 0;
                                    while (i < 4) //цикл для отображение каждого варианта ответа
                                    {
                                        int check = 0; int random = r.Next(4);
                                        foreach (var vari in variant) //проверка на совпадение одинаковых вариантов
                                        {
                                            if (random == vari)
                                            { check++; break; }
                                        }

                                        if (check == 0 && i == 0) //отображение вариантов
                                        { textBlock_1variant.Text = random_variation[random]; variant[i] = random; i++; }
                                        else if (check == 0 && i == 1)
                                        { textBlock_2variant.Text = random_variation[random]; variant[i] = random; i++; }
                                        else if (check == 0 && i == 2)
                                        { textBlock_3variant.Text = random_variation[random]; variant[i] = random; i++; }
                                        else if (check == 0 && i == 3)
                                        { textBlock_4variant.Text = random_variation[random]; variant[i] = random; i++; }
                                    }

                                    idword = words.Id; //id слова для условий

                                    curword++; //текущие слово
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void MessageWin()
        {
            try
            {
                int corwordstot = 0;
                foreach (var themes in itestentities.Themes)
                {
                    if (themes.Name == SetClass.Themes)
                    {
                        foreach (var words in itestentities.Words)
                        {
                            if (words.Id_themes == themes.Id)
                            {
                                foreach (var mywords in itestentities.My_Dictionary)
                                {
                                    if (mywords.Id_account == SetClass.Id)
                                        corwordstot++;
                                }
                            }
                        }
                        break;
                    }
                }

                if (corwordstot == maxwords)
                    MessageBox.Show(string.Format("Вы прошли тест раздела \"{0}\"! Вы ответили правильно на все вопросы. Поздравляем! Не останавливайтесь на достигнутом.", SetClass.Themes), "Тест пройден", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show(string.Format("Вы прошли тест тематики \"{0}\"! Вы ответили правильно на {1} из {2} вопросов. ", SetClass.Themes, corwords, maxwords), "Тест пройден", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void button_1variant_Click(object sender, RoutedEventArgs e)
        {
            SaveData(textBlock_1variant); LoadingData();
        }

        private void button_2variant_Click(object sender, RoutedEventArgs e)
        {
            SaveData(textBlock_2variant); LoadingData();
        }

        private void button_3variant_Click(object sender, RoutedEventArgs e)
        {
            SaveData(textBlock_3variant); LoadingData();
        }

        private void button_4variant_Click(object sender, RoutedEventArgs e)
        {
            SaveData(textBlock_4variant); LoadingData();
        }

        private void button_pass_Click(object sender, RoutedEventArgs e)
        {
            LoadingData();

            if (textBlock_1variant.Text == "" && textBlock_2variant.Text == "" && textBlock_3variant.Text == "" && textBlock_4variant.Text == "") //проверка на завершение теста
            { MessageWin(); Close(); }
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
