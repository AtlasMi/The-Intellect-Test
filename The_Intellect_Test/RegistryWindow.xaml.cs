using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для RegistryWindow.xaml
    /// </summary>
    public partial class RegistryWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД

        string m_help_login = "макс. 15 сим."; //подсказка для логина
        string m_error_sym = "*недопустимые символы"; //ошибка для пароля и логина
        int numimg = 0; //номер аватара

        Regex regex_check = new Regex("[a-zA-z0-9]"); //условие для логина и пароля

        public RegistryWindow()
        {
            InitializeComponent();

            ColorForeground(textBox_login, Brushes.LightSlateGray); //для фокуса логина, изменение цвета шрифта
            textBox_login.Text = m_help_login; //для фокуса логина
        }

        #region Button_picrure
        private void button_pictureBox1_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(1, textBlock_pictureBox1_check);
        }

        private void button_pictureBox2_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(2, textBlock_pictureBox2_check);
        }

        private void button_pictureBox3_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(3, textBlock_pictureBox3_check);
        }

        private void button_pictureBox4_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(4, textBlock_pictureBox4_check);
        }

        private void button_pictureBox5_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(5, textBlock_pictureBox5_check);
        }

        private void button_pictureBox6_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(6, textBlock_pictureBox6_check);
        }

        private void button_pictureBox7_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(7, textBlock_pictureBox7_check);
        }

        private void button_pictureBox8_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(8, textBlock_pictureBox8_check);
        }

        private void button_pictureBox9_Click(object sender, RoutedEventArgs e)
        {
            Button_pictureBox(9, textBlock_pictureBox9_check);
        }

        void Button_pictureBox(int num, TextBlock but) //метод для цифры аватара, для очистки всех TextBlock и отображение галочки
        {
            numimg = num;

            textBlock_pictureBox1_check.Text = "";
            textBlock_pictureBox2_check.Text = "";
            textBlock_pictureBox3_check.Text = "";
            textBlock_pictureBox4_check.Text = "";
            textBlock_pictureBox5_check.Text = "";
            textBlock_pictureBox6_check.Text = "";
            textBlock_pictureBox7_check.Text = "";
            textBlock_pictureBox8_check.Text = "";
            textBlock_pictureBox9_check.Text = "";

            but.Text = "✓";
        }
        #endregion

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBox.Password.Length > 3 && textBlock_password.Text == "" && textBlock_password2.Text == "")
                {
                    if (textBox_login.Text.Length > 3 && textBox_login.Text != m_help_login && textBlock_login.Text == "")
                    {
                        if (passwordBox.Password != textBox_login.Text)
                        {
                            if (numimg != 0)
                            {
                                int check_entry = 0; //для проверки на совпадение и сохранения
                                var add_account = new Accounts();

                                add_account.Id_access_right = 2;
                                add_account.Password = passwordBox.Password;
                                add_account.Login = textBox_login.Text;
                                add_account.Id_ava = numimg;
                                add_account.Score = 0;
                                add_account.Level = 0;

                                SetClass.Login = textBox_login.Text;
                                SetClass.Password = passwordBox.Password;

                                itestentities.Accounts.Add(add_account);

                                foreach (var accounts in itestentities.Accounts)
                                {
                                    if (accounts.Login == textBox_login.Text)
                                    {
                                        check_entry++;
                                        break;
                                    }
                                }

                                if (check_entry == 0)
                                {
                                    itestentities.SaveChanges();
                                    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                                    Close();
                                }
                                else
                                    MessageBox.Show("Ошибка! Такой логин уже существует", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                                MessageBox.Show("Вы не выбрали аватар", "Аватар", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                            MessageBox.Show("Логин и пароль не могут совпадать", "Логин и пароль", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Ошибка синтаксиса", "Логин", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Ошибка синтаксиса", "Пароль", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ColorForeground(TextBox t, Brush c) //метод для изменения цвета шрифта
        {
            t.Foreground = c;
        }

        public void ColorBorder(TextBox t, Brush c) //метод для изменения цвета обводки
        {
            t.BorderBrush = c;
        }

        public void ColorBorderPass(PasswordBox t, Brush c) //метод для изменения цвета обводки
        {
            t.BorderBrush = c;
        }

        #region Login
        private void textBox_login_GotFocus(object sender, RoutedEventArgs e) //метод "в фокусе" для логина
        {
            if (textBox_login.Text == m_help_login)
                textBox_login.Text = "";
            ColorForeground(textBox_login, Brushes.Black);
        }

        private void textBox_login_LostFocus(object sender, RoutedEventArgs e) //метод "вне фокусе" для логина
        {
            if (textBox_login.Text == "")
            {
                ColorForeground(textBox_login, Brushes.LightSlateGray);
                textBox_login.Text = m_help_login;
            }
        }

        private void textBox_login_TextChanged(object sender, TextChangedEventArgs e) //метод для проверки синтаксиса логина
        {
            try
            {
                int check_entry = 0; //проверка на некорректные символы

                if (textBox_login.Text == m_help_login || textBox_login.Text == "")
                {
                    textBlock_login.Text = "";
                    ColorBorder(textBox_login, Brushes.DarkGray);
                }
                else if (textBox_login.Text != "")
                {
                    for (int i = 0; i < textBox_login.Text.Length; i++)
                    {
                        Match match_checksym = regex_check.Match(textBox_login.Text[i].ToString());
                        if (match_checksym.Success == false)
                        {
                            check_entry++;
                            ColorBorder(textBox_login, Brushes.Red);
                            textBlock_login.Text = m_error_sym;
                            break;
                        }
                    }

                    if (check_entry == 0)
                    {
                        ColorBorder(textBox_login, Brushes.DarkGray);
                        textBlock_login.Text = "";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Password
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e) //метод для проверки синтаксиса пароля
        {
            int check_entry_pass = 0; //проверка на некорректные символы

            try
            {
                if (passwordBox.Password == "")
                {
                    textBlock_password.Text = "";
                    ColorBorderPass(passwordBox, Brushes.DarkGray);
                }
                else
                {
                    for (int a = 0; a < passwordBox.Password.Length; a++)
                    {
                        Match match_checksym = regex_check.Match(passwordBox.Password[a].ToString());
                        if (match_checksym.Success == false)
                        {
                            check_entry_pass++;
                            ColorBorderPass(passwordBox, Brushes.Red);
                            textBlock_password.Text = m_error_sym;
                        }
                    }

                    if (check_entry_pass == 0)
                    {
                        ColorBorderPass(passwordBox, Brushes.DarkGray);
                        textBlock_password.Text = "";
                    }
                }

                DoublePass();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Test", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region DoublePassword
        void DoublePass()
        {
            try
            {
                if (passwordBox.Password.Length == passwordBox_2.Password.Length)
                {
                    if (textBlock_password.Text != "")
                    {
                        ColorBorderPass(passwordBox_2, Brushes.DarkGray);
                        textBlock_password2.Text = "";
                    }
                    else if (passwordBox.Password == passwordBox_2.Password)
                    {
                        ColorBorderPass(passwordBox_2, Brushes.DarkGray);
                        textBlock_password2.Text = "";
                    }
                    else
                    {
                        ColorBorderPass(passwordBox_2, Brushes.Red);
                        textBlock_password2.Text = "*пароли не совпадают";
                    }
                }
                else if (passwordBox_2.Password.Length > passwordBox.Password.Length)
                {
                    ColorBorderPass(passwordBox_2, Brushes.Red);
                    textBlock_password2.Text = "*пароли не совпадают";
                }
                else
                {
                    ColorBorderPass(passwordBox_2, Brushes.DarkGray);
                    textBlock_password2.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибочка вышла! Обратитесь к разработчику", "The Intellect Test", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void passwordBox_2_PasswordChanged(object sender, RoutedEventArgs e) //метод для проверки повторного пароля
        {
            DoublePass();
        }
        #endregion

        private void button_check_Click(object sender, RoutedEventArgs e) //метод для проверки на совпадения логина в базе данных
        {
            int check_entry_log = 0; //проверка на совпадение логина

            try
            {
                if (textBlock_login.Text == "")
                {
                    if (textBlock_login.Text == "" && textBox_login.Text == m_help_login)
                        textBlock_login_check.Text = "";
                    else
                    {
                        foreach (var accounts in itestentities.Accounts)
                        {
                            if (accounts.Login == textBox_login.Text)
                            {
                                check_entry_log++;
                                textBlock_login_check.Text = "";
                                textBlock_login_check.Foreground = Brushes.Red;
                                textBlock_login_check.Text = "✕";
                                textBlock_login_check.ToolTip = "Такой логин уже существует";
                                break;
                            }
                        }

                        if (check_entry_log == 0)
                        {
                            textBlock_login_check.Text = "";
                            textBlock_login_check.Foreground = Brushes.Green;
                            textBlock_login_check.Text = "✓";
                            textBlock_login_check.ToolTip = "Вы можете использовать этот логин";
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}