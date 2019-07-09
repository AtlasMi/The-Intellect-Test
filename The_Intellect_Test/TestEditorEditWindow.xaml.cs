using System;
using System.Windows;
using System.Windows.Controls;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для TestEditorEditWindow.xaml
    /// </summary>
    public partial class TestEditorEditWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД
        int idthemes = 0; //id темы

        public TestEditorEditWindow()
        {
            InitializeComponent();

            try
            {
                Title = "The Intellect Test | Редактирование теста: " + SetClass.Themes; //заголовок окна

                foreach (var themes in itestentities.Themes)
                {
                    if (themes.Name == SetClass.Themes)
                    { idthemes = themes.Id; }
                }

                foreach (var words in itestentities.Words)
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        if (words.Id_themes == themes.Id && themes.Name == SetClass.Themes)
                        { listBox_words.Items.Add(words); }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var delete_word = listBox_words.SelectedItem as Words;

                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить это слово?", "Удаление!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (delete_word != null)
                        {
                            itestentities.Words.Remove(delete_word);
                            itestentities.SaveChanges();
                            listBox_words.Items.Remove(delete_word);
                            MessageBox.Show("Слово удалено!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                            Clear();
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var words = listBox_words.SelectedItem as Words;

                if (textBox_word.Text != "" && textBox_word.Text.Length > 2)
                {
                    if (textBox_qustion.Text != "" && textBox_qustion.Text.Length > 5)
                    {
                        if (textBox_answer.Text != "" && textBox_answer.Text.Length > 2)
                        {
                            if (textBox_Variation_2.Text != "" && textBox_Variation_2.Text.Length > 2)
                            {
                                if (textBox_Variation_3.Text != "" && textBox_Variation_3.Text.Length > 2)
                                {
                                    if (textBox_Variation_4.Text != "" && textBox_Variation_4.Text.Length > 2)
                                    {
                                        if (textBox_determination.Text != "" && textBox_determination.Text.Length > 10)
                                        {
                                            var edit_word = listBox_words.SelectedItem as Words;

                                            edit_word.Word = textBox_word.Text;
                                            edit_word.Question = textBox_qustion.Text;
                                            edit_word.Answer = textBox_answer.Text;
                                            edit_word.Variation_2 = textBox_Variation_2.Text;
                                            edit_word.Variation_3 = textBox_Variation_3.Text;
                                            edit_word.Variation_4 = textBox_Variation_4.Text;
                                            edit_word.Determination = textBox_determination.Text;

                                            itestentities.SaveChanges();
                                            listBox_words.Items.Refresh();
                                            MessageBox.Show("Слово изменено!", "Изменение", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        else MessageBox.Show("Ошибка! Поле \"Определение\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else MessageBox.Show("Ошибка! Поле \"Вариант 4\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else MessageBox.Show("Ошибка! Поле \"Вариант 3\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else MessageBox.Show("Ошибка! Поле \"Вариант 2\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else MessageBox.Show("Ошибка! Поле \"Ответ\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else MessageBox.Show("Ошибка! Поле \"Вопрос\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("Ошибка! Поле \"Слово\" не может быть пустым", string.Format("The Intellect Test | Редактор тестов: {0}", words.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listBox_words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected_words = listBox_words.SelectedItem as Words;

                if (selected_words != null)
                {
                    textBox_word.Text = selected_words.Word;
                    textBox_qustion.Text = selected_words.Question;
                    textBox_answer.Text = selected_words.Answer;
                    textBox_Variation_2.Text = selected_words.Variation_2;
                    textBox_Variation_3.Text = selected_words.Variation_3;
                    textBox_Variation_4.Text = selected_words.Variation_4;
                    textBox_determination.Text = selected_words.Determination;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_plus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите добавить слово?", "Добавить слово", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        var add_word = new Words();

                        add_word.Word = "(пусто)";
                        add_word.Question = "(пусто)";
                        add_word.Answer = "(пусто)";
                        add_word.Variation_2 = "(пусто)";
                        add_word.Variation_3 = "(пусто)";
                        add_word.Variation_4 = "(пусто)";
                        add_word.Determination = "(пусто)";
                        add_word.Id_themes = idthemes;

                        itestentities.Words.Add(add_word);
                        itestentities.SaveChanges();
                        listBox_words.Items.Add(add_word);

                        MessageBox.Show("Слово добавлено", "Добавление слова", MessageBoxButton.OK, MessageBoxImage.Information);
                        Clear();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_clear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            textBox_word.Text = "";
            textBox_qustion.Text = "";
            textBox_answer.Text = "";
            textBox_Variation_2.Text = "";
            textBox_Variation_3.Text = "";
            textBox_Variation_4.Text = "";
            textBox_determination.Text = "";
            listBox_words.SelectedIndex = -1;
        }
    }
}
