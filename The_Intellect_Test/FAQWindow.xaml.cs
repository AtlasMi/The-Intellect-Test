using System;
using System.Windows;
using System.Windows.Controls;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для FAQWindow.xaml
    /// </summary>
    public partial class FAQWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        public FAQWindow()
        {
            InitializeComponent();
            try
            {
                foreach (var name in itestentities.FAQ)
                {
                    listBox_faq.Items.Add(name);
                    textBlock_text.Text = name.Text;
                }

                listBox_faq.SelectedIndex = 0;

                if (SetClass.CheckOpen != 0)
                {
                    listBox_faq.SelectedIndex = 4;

                    SetClass.CheckOpen = 0;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listBox_faq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected_faq = listBox_faq.SelectedItem as FAQ;

                textBlock_text.Text = selected_faq.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
