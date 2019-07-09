using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для TestEditorAddWindow.xaml
    /// </summary>
    public partial class TestEditorAddWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        public TestEditorAddWindow()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Regex regex_check = new Regex("[0-9]");
                if (textBox_name.Text != "" && textBox_name.Text.Length > 2)
                {
                    int check_themes = 0;
                    foreach (var themes in itestentities.Themes)
                    {
                        if (themes.Name == textBox_name.Text)
                            check_themes++;
                    }
                    if (check_themes == 0)
                    {
                        if (int.Parse(textBox_count.Text) != 0)
                        {
                            Match match_checksym = regex_check.Match(textBox_count.Text);

                            if (match_checksym.Success)
                            {
                                if (int.Parse(textBox_count.Text) >= 7)
                                {
                                    if (int.Parse(textBox_count.Text) <= 30)
                                    {
                                        int idthemes = 0;
                                        var add_themes = new Themes();
                                        add_themes.Name = textBox_name.Text;
                                        SetClass.Themes = textBox_name.Text;
                                        add_themes.Id_section = 3;
                                        add_themes.Id_user = SetClass.Id;
                                        itestentities.Themes.Add(add_themes);
                                        itestentities.SaveChanges();

                                        foreach (var themes in itestentities.Themes)
                                        {
                                            if (themes.Name == textBox_name.Text)
                                                idthemes = themes.Id;
                                        }

                                        for (int i = 1; i <= int.Parse(textBox_count.Text); i++)
                                        {
                                            var add_word = new Words();
                                            add_word.Word = i.ToString();
                                            add_word.Id_themes = idthemes;
                                            add_word.Question = "(пусто)";
                                            add_word.Answer = "(пусто)";
                                            add_word.Variation_2 = "(пусто)";
                                            add_word.Variation_3 = "(пусто)";
                                            add_word.Variation_4 = "(пусто)";
                                            add_word.Determination = "(пусто)";
                                            itestentities.Words.Add(add_word);
                                        }
                                        itestentities.SaveChanges();
                                        MessageBox.Show("Тест создан. Можете начать его редактировать.", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Information);

                                        Close();
                                        TestEditorEditWindow teew = new TestEditorEditWindow();
                                        teew.ShowDialog();
                                    }
                                    else MessageBox.Show("Ошибка! Невозможно добавить больше 30 вопросов за раз", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else MessageBox.Show("Ошибка! Кол-во вопросов не может быть меньше 7", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else MessageBox.Show("Ошибка! В поле \"Количество вопросов\" может быть только числовое значение", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else MessageBox.Show("Ошибка! Поле \"Количество вопросов\" не может быть пустым", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else MessageBox.Show("Ошибка! \"Название теста\" не может совпадать с существующими", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("Ошибка! Поле \"Название теста\" не может быть меньше 3 символов", "The Intellect Test | Создание теста", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
