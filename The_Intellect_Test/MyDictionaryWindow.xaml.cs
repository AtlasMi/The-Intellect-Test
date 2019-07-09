using System;
using System.Windows;
using System.Windows.Controls;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для MyDictionaryWindow.xaml
    /// </summary>
    public partial class MyDictionaryWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        public MyDictionaryWindow()
        {
            InitializeComponent();

            try
            {
                comboBox_themes.Items.Add("Все слова");
                foreach (var themes in itestentities.Themes)
                {
                    foreach (var section in itestentities.Section)
                    {
                        if (themes.Id_section == section.Id && section.Name == "Общие")
                            comboBox_themes.Items.Add(themes);
                    }
                }
                foreach (var section in itestentities.Section)
                    comboBox_themes.Items.Add(section);

                TotalData();
                comboBox_themes.Text = "Все слова";
                listBox_words.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void TotalData()
        {
            listBox_words.Items.Clear(); textBlock_text.Text = ""; textBlock_themes.Text = "";

            foreach (var words in itestentities.Words)
            {
                foreach (var mywords in itestentities.My_Dictionary)
                {
                    if (mywords.Id_account == SetClass.Id)
                    {
                        foreach (var themes in itestentities.Themes)
                        {
                            if (words.Id_themes == themes.Id && mywords.Id_word == words.Id)
                            {
                                textBlock_themes.Text = themes.Name;
                                listBox_words.Items.Add(words);
                                if (listBox_words.SelectedItem == words) textBlock_text.Text = words.Determination;
                                break;
                            }
                        }
                    }
                }
            }
        }

        void DataThemes(string selectedTheme)
        {
            listBox_words.Items.Clear(); textBlock_text.Text = ""; textBlock_themes.Text = "";

            foreach (var words in itestentities.Words)
            {
                foreach (var mywords in itestentities.My_Dictionary)
                {
                    if (mywords.Id_account == SetClass.Id)
                    {
                        foreach (var themes in itestentities.Themes)
                        {
                            if (words.Id_themes == themes.Id && themes.Name == selectedTheme)
                            {
                                if (mywords.Id_word == words.Id)
                                {
                                    textBlock_themes.Text = themes.Name;
                                    listBox_words.Items.Add(words);
                                    if (listBox_words.SelectedItem == words) textBlock_text.Text = words.Determination;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        void DataSection(string selectedSection)
        {
            listBox_words.Items.Clear(); textBlock_text.Text = ""; textBlock_themes.Text = "";

            foreach (var words in itestentities.Words)
            {
                foreach (var mywords in itestentities.My_Dictionary)
                {
                    if (mywords.Id_account == SetClass.Id)
                    {
                        foreach (var section in itestentities.Section)
                        {
                            foreach (var themes in itestentities.Themes)
                            {
                                if (words.Id_themes == themes.Id && themes.Id_section == section.Id && section.Name == selectedSection)
                                {
                                    if (mywords.Id_word == words.Id)
                                    {
                                        textBlock_themes.Text = themes.Name;
                                        listBox_words.Items.Add(words);
                                        textBlock_text.Text = words.Determination;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void listBox_words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox_words.SelectedItem as Words != null)
            {
                var selected_words = (listBox_words.SelectedItem as Words).Id;

                foreach (var word in itestentities.Words)
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        if (selected_words == word.Id && themes.Id == word.Id_themes)
                        {
                            textBlock_themes.Text = themes.Name;
                            textBlock_text.Text = word.Determination;
                            break;
                        }
                    }
                }
            }
        }

        private void comboBox_themes_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_themes.Text == "Все слова")
                    TotalData();
                else
                {
                    int check_themes = 0;
                    foreach (var themes in itestentities.Themes)
                    {
                        if (themes.Name == comboBox_themes.Text)
                        { DataThemes(comboBox_themes.Text); check_themes++; break; }
                    }

                    if (check_themes == 0)
                    {
                        foreach (var section in itestentities.Section)
                        {
                            if (section.Name == comboBox_themes.Text)
                            { DataSection(comboBox_themes.Text); break; }
                        }
                    }
                }
                listBox_words.SelectedIndex = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
