using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для StudyWindow.xaml
    /// </summary>
    public partial class StudyWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД
        int maxwords = 0;
        int curnum = 0;
        int checkcur = 0;

        public StudyWindow()
        {
            InitializeComponent();

            try
            {
                comboBox_themes.Items.Add("Все слова");

                foreach (var section in itestentities.Section)
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        if (themes.Id_section != section.Id) break;
                        else if (themes.Id_section == section.Id && section.Name == "Общие")
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
                MessageBox.Show("Ошибочка вышла! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        void TotalData()
        {
            listBox_words.Items.Clear(); textBlock_text.Text = ""; textBlock_themes.Text = "";
            maxwords = 0;
            foreach (var mywords in itestentities.My_Dictionary)
            {
                if (mywords.Id_account == SetClass.Id)
                    maxwords++;
            }

            if (maxwords == 0)
            {
                foreach (var words in itestentities.Words)
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        if (words.Id_themes == themes.Id)
                        {
                            textBlock_themes.Text = themes.Name;
                            break;
                        }
                    }
                    listBox_words.Items.Add(words);
                    textBlock_text.Text = words.Determination;
                }
            }
            else
            {
                foreach (var words in itestentities.Words)
                {
                    foreach (var mywords in itestentities.My_Dictionary)
                    {
                        if (mywords.Id_account == SetClass.Id)
                        {
                            curnum++;
                            if (words.Id == mywords.Id_word)
                                checkcur++;

                            if (checkcur == 0 && curnum == maxwords)
                            {
                                foreach (var themes in itestentities.Themes)
                                {
                                    if (words.Id_themes == themes.Id)
                                    {
                                        textBlock_themes.Text = themes.Name;
                                        break;
                                    }
                                }
                                listBox_words.Items.Add(words);
                                textBlock_text.Text = words.Determination;
                                break;
                            }
                        }
                    }
                    curnum = 0;
                    checkcur = 0;
                }
            }
        }

        void DataThemes(string selectedTheme)
        {
            listBox_words.Items.Clear(); textBlock_text.Text = ""; textBlock_themes.Text = "";
            maxwords = 0;
            foreach (var mywords in itestentities.My_Dictionary)
            {
                if (mywords.Id_account == SetClass.Id)
                    maxwords++;
            }

            if (maxwords == 0)
            {
                foreach (var words in itestentities.Words)
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        if (words.Id_themes == themes.Id && themes.Name == selectedTheme)
                        {
                            textBlock_themes.Text = themes.Name;
                            listBox_words.Items.Add(words);
                            textBlock_text.Text = words.Determination;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var words in itestentities.Words)
                {
                    foreach (var mywords in itestentities.My_Dictionary)
                    {
                        if (mywords.Id_account == SetClass.Id)
                        {
                            curnum++;
                            if (words.Id == mywords.Id_word)
                                checkcur++;
                            foreach (var themes in itestentities.Themes)
                            {
                                if (checkcur == 0 && curnum == maxwords && words.Id_themes == themes.Id && themes.Name == selectedTheme)
                                {
                                    textBlock_themes.Text = themes.Name;
                                    listBox_words.Items.Add(words);
                                    textBlock_text.Text = words.Determination;
                                    break;
                                }
                            }
                        }
                    }
                    curnum = 0;
                    checkcur = 0;
                }
            }
        }

        void DataSection(string selectedSection)
        {
            listBox_words.Items.Clear(); textBlock_text.Text = ""; textBlock_themes.Text = "";

            maxwords = 0;
            foreach (var mywords in itestentities.My_Dictionary)
            {
                if (mywords.Id_account == SetClass.Id)
                    maxwords++;
            }

            if (maxwords == 0)
            {
                foreach (var words in itestentities.Words)
                {
                    foreach (var section in itestentities.Section)
                    {
                        foreach (var themes in itestentities.Themes)
                        {
                            if (words.Id_themes == themes.Id && themes.Id_section == section.Id && section.Name == selectedSection)
                            {
                                textBlock_themes.Text = themes.Name;
                                listBox_words.Items.Add(words);
                                textBlock_text.Text = words.Determination;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var words in itestentities.Words)
                {
                    foreach (var mywords in itestentities.My_Dictionary)
                    {
                        if (mywords.Id_account == SetClass.Id)
                        {
                            curnum++;
                            if (words.Id == mywords.Id_word)
                                checkcur++;
                            foreach (var section in itestentities.Section)
                            {
                                foreach (var themes in itestentities.Themes)
                                {
                                    if (checkcur == 0 && curnum == maxwords && words.Id_themes == themes.Id && themes.Id_section == section.Id && section.Name == selectedSection)
                                    {
                                        textBlock_themes.Text = themes.Name;
                                        listBox_words.Items.Add(words);
                                        textBlock_text.Text = words.Determination;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    curnum = 0;
                    checkcur = 0;
                }
            }
        }

        private void listBox_words_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected_words = listBox_words.SelectedItem as Words;

                if (selected_words != null)
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        if (selected_words.Id_themes == themes.Id)
                        {
                            textBlock_themes.Text = themes.Name;
                            break;
                        }
                    }
                    textBlock_text.Text = selected_words.Determination;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибочка вышла! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Ошибочка вышла! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}