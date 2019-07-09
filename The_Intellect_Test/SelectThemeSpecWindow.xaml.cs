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
    /// Логика взаимодействия для SelectThemeSpecWindow.xaml
    /// </summary>
    public partial class SelectThemeSpecWindow : Window
    {
        public SelectThemeSpecWindow()
        {
            InitializeComponent();
        }

        void TransButton(Button but) //метод для начала теста
        {
            SetClass.Themes = but.Content.ToString();
            TestWindow tw = new TestWindow();
            tw.ShowDialog();
            Close();
        }

        private void button_theme1_Click(object sender, RoutedEventArgs e)
        {
            TransButton(button_theme1);
        }
    }
}
