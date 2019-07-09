using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД
        string m_help_login = "макс. 15 сим."; //подсказка для логина
        string m_error_sym = "*недопустимые символы"; //ошибка для пароля и логина
        int numimg = 0; //номер аватара
        Regex regex_check = new Regex("[a-zA-z0-9]"); //условие для логина и пароля
        int check_access = 0; //для проверки прав пользователя

        public EditWindow()
        {
            InitializeComponent();

            try
            {
                CheckAccess();

                if (check_access != 0)
                    button_reset.IsEnabled = false;

                textBox_login.Text = SetClass.Login;

                if (SetClass.IdAva == 1) SelectImage(textBlock_pictureBox1_check, button_pictureBox1); //при запуске окна нужно определить выбранную картинку, для этого создаём серию условий
                if (SetClass.IdAva == 2) SelectImage(textBlock_pictureBox2_check, button_pictureBox2);
                if (SetClass.IdAva == 3) SelectImage(textBlock_pictureBox3_check, button_pictureBox3);
                if (SetClass.IdAva == 4) SelectImage(textBlock_pictureBox4_check, button_pictureBox4);
                if (SetClass.IdAva == 5) SelectImage(textBlock_pictureBox5_check, button_pictureBox5);
                if (SetClass.IdAva == 6) SelectImage(textBlock_pictureBox6_check, button_pictureBox6);
                if (SetClass.IdAva == 7) SelectImage(textBlock_pictureBox7_check, button_pictureBox7);
                if (SetClass.IdAva == 8) SelectImage(textBlock_pictureBox8_check, button_pictureBox8);
                if (SetClass.IdAva == 9) SelectImage(textBlock_pictureBox9_check, button_pictureBox9);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Button_picrure
        void SelectImage(TextBlock check, Button img) //метод для "отметки" выбранного условия
        {
            check.Text = "✕";
            check.Foreground = Brushes.Red;
            check.ToolTip = "Этот аватар используется";
            img.IsEnabled = false;
        }

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

        void SelectedImage(TextBlock selimg) //метод для проверки TextBlock на ✕
        {
            if (selimg.Text != "✕")
                selimg.Text = "";
        }

        void Button_pictureBox(int num, TextBlock but) //метод для цифры аватара и для очистки всех TextBlock и отображение галочки
        {
            numimg = num; //для записи в бд

            SelectedImage(textBlock_pictureBox1_check); SelectedImage(textBlock_pictureBox2_check); SelectedImage(textBlock_pictureBox3_check); //проверить каждый TextBlock на ✕
            SelectedImage(textBlock_pictureBox4_check); SelectedImage(textBlock_pictureBox5_check); SelectedImage(textBlock_pictureBox6_check);
            SelectedImage(textBlock_pictureBox7_check); SelectedImage(textBlock_pictureBox8_check); SelectedImage(textBlock_pictureBox9_check);

            if (but.Text == "")
            { but.Text = "✓"; but.Foreground = Brushes.Green; }
        }
        #endregion

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (numimg != 0 || textBox_login.Text != SetClass.Login || oldpasswordBox.Password != "" || passwordBox.Password != "" || passwordBox_2.Password != "")
                {
                    int check_entry = 0;
                    int check_entry2 = 0;
                    foreach (var accounts in itestentities.Accounts)
                    {
                        if (accounts.Id == SetClass.Id)
                        {
                            if (numimg != 0)
                            {
                                check_entry2++;
                                if (oldpasswordBox.Password == "")
                                { MessageBox.Show("Ошибка синтаксиса", "Старый пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                else if (oldpasswordBox.Password == SetClass.Password)
                                { accounts.Id_ava = numimg; SetClass.IdAva = numimg; SetClass.IdAvaCheck++; }
                                else { MessageBox.Show("Ошибка! Введите пароль для изменения аватара", "Старый пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                            }

                            if (textBox_login.Text.Length != SetClass.Login.Length && textBox_login.Text != SetClass.Login)
                            {
                                check_entry2++;
                                if (textBox_login.Text != m_help_login && textBlock_login.Text == "" && textBox_login.Text.Length > 3)
                                {
                                    if (textBox_login.Text != SetClass.Login)
                                    {
                                        if (oldpasswordBox.Password == SetClass.Password)
                                        {
                                            foreach (var acc in itestentities.Accounts)
                                            {
                                                if (acc.Login == textBox_login.Text)
                                                { check_entry++; break; }
                                            }

                                            if (check_entry == 0)
                                            { accounts.Login = textBox_login.Text; SetClass.Login = textBox_login.Text; }
                                            else { MessageBox.Show("Ошибка! Такой логин уже существует", "Логин", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                        }
                                        else { MessageBox.Show("Ошибка! Неверный пароль", "Старый пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                    }
                                    else { MessageBox.Show("Ошибка! Новый логин не может совпадать со старым", "Логин", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                }
                                else { MessageBox.Show("Ошибка! Ошибка синтаксиса", "Логин", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                            }

                            if (passwordBox.Password != "" || passwordBox_2.Password != "")
                            {
                                check_entry2++;
                                if (passwordBox_2.Password != "")
                                {
                                    if (passwordBox.Password.Length > 3 || textBlock_password.Text == "" || textBlock_password2.Text == "")
                                    {
                                        if (passwordBox.Password != textBox_login.Text)
                                        {
                                            if (passwordBox.Password == passwordBox_2.Password)
                                            {
                                                if (passwordBox.Password != SetClass.Password)
                                                { accounts.Password = passwordBox.Password; SetClass.Password = passwordBox.Password; break; }
                                                else { MessageBox.Show("Ошибка! Новый пароль не может совпадать со старым", "Новый пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                            }
                                            else { MessageBox.Show("Ошибка! Пароли не совпадают", "Повторите пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                        }
                                        else { MessageBox.Show("Ошибка! Пароль не может совпадать с логином", "Новый пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                    }
                                    else { MessageBox.Show("Ошибка синтаксиса!", "Новый пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                                }
                                else { MessageBox.Show("Ошибка! Поле не заполнено", "Повторите пароль", MessageBoxButton.OK, MessageBoxImage.Error); check_entry++; break; }
                            }
                            else break;
                        }
                    }

                    if (check_entry == 0 && check_entry2 != 0)
                    {
                        itestentities.SaveChanges();
                        MessageBox.Show("Профиль изменён!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }

                    if (check_entry2 == 0)
                        MessageBox.Show("Ошибка! Ничего не изменено!", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else MessageBox.Show("Ошибка! Ничего не изменено!", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Error);

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
                int check_entry = 0;

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
        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e) //метод для проверки синтексиса пароля
        {
            int check_entry_pass = 0;

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
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Test", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void passwordBox_2_PasswordChanged(object sender, RoutedEventArgs e) //метод для проверки повторного пароля
        {
            DoublePass();
        }
        #endregion

        private void button_check_Click(object sender, RoutedEventArgs e) //метод для проверки на совпадения логина в базе данных
        {
            int check_entry_log = 0;

            try
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
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_reset_Click(object sender, RoutedEventArgs e)
        {
            int check = 0;

            try
            {
                if (oldpasswordBox.Password == SetClass.Password)
                {
                    foreach (var acc in itestentities.Accounts)
                    {
                        if (acc.Id == SetClass.Id && acc.Score == 0) check++;
                        else if (acc.Id == SetClass.Id)
                        {
                            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сделать сброс профиля?", "Удаление!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:

                                    foreach (var myword in itestentities.My_Dictionary)
                                    {
                                        if (myword.Id_account == SetClass.Id)
                                            itestentities.My_Dictionary.Remove(myword);
                                    }
                                    acc.Level = 0;
                                    acc.Score = 0;
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                            break;
                        }
                    }

                    if (check == 0) itestentities.SaveChanges();
                    else MessageBox.Show("Ошибка! У вас и так 0 очков, нечего сбрасывать", "Сброс", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                else MessageBox.Show("Ошибка! Неверный пароль", "Старый пароль", MessageBoxButton.OK, MessageBoxImage.Error);

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
