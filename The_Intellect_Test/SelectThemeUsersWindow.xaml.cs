using System;
using System.Windows;

namespace The_Intellect_Test
{
    /// <summary>
    /// Логика взаимодействия для SelectThemeUsersWindow.xaml
    /// </summary>
    public partial class SelectThemeUsersWindow : Window
    {
        ITestEntities itestentities = new ITestEntities(); //подключение БД
        int check_access = 0; //для проверки прав пользователя
        int check_edit = 0; //для проверки прав редактирования

        public SelectThemeUsersWindow()
        {
            InitializeComponent();
            try
            {
                CheckAccess();

                if (check_access != 0)
                    Title = "The Intellect Test | Редактор теста";

                if (check_access == 0) //выводит только из раздела пользовательские
                {
                    foreach (var themes in itestentities.Themes)
                    {
                        foreach (var section in itestentities.Section)
                        {
                            if (themes.Id_section == section.Id && section.Name == "Пользовательский")
                                listBox_themes.Items.Add(themes);
                        }
                    }
                }
                else //выводит все
                {
                    foreach (var themes in itestentities.Themes)
                        listBox_themes.Items.Add(themes);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool CheckContent()
        {
            int check = 0;
            foreach (var themes in itestentities.Themes)
            {
                foreach (var words in itestentities.Words)
                {
                    if (themes.Id == (listBox_themes.SelectedItem as Themes).Id && words.Question == "" || words.Question == "(пусто)" || words.Answer == "" || words.Answer == "(пусто)" || words.Variation_2 == "" || words.Variation_2 == "(пусто)" ||
                        words.Variation_3 == "" || words.Variation_3 == "(пусто)" || words.Variation_4 == "" || words.Variation_4 == "(пусто)" || words.Determination == "" || words.Determination == "(пусто)")
                        { check++; break; }
                }

                if (check != 0) break;
            }

            if (check == 0)
                return true;
            else
                return false;
        }

        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var select_themes = listBox_themes.SelectedItem as Themes;
                CheckAccess();

                int check_words = 0;
                if (check_access == 0) //у пользователя ряд ограничений
                {
                    if (CheckContent())
                    {
                        foreach (var themes in itestentities.Themes)
                        {
                            foreach (var words in itestentities.Words)
                            {
                                if (themes.Name == select_themes.ToString() && words.Id_themes == themes.Id)
                                    check_words++;
                            }
                        }

                        if (check_words > 6)
                            MessageBox.Show("Прохождение теста невозможно т. к. в нём меньше 7 вопросов", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            if (select_themes != null)
                            {
                                SetClass.Themes = select_themes.ToString();
                                TestWindow tw = new TestWindow();
                                tw.ShowDialog();
                                Close();

                                SelectThemeWindow stw = new SelectThemeWindow();
                                stw.Close();

                                UserWindow uw = new UserWindow();
                                uw.Close();
                                uw.ShowDialog();
                            }
                            else MessageBox.Show("Ошибка! Вы не выбрали тест", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else  MessageBox.Show("Ошибка! Тест не готов к прохождению. Сообщите его создателю или администратору", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else //у администратора ограничений нет
                {
                    if (select_themes != null)
                    {
                        foreach (var section in itestentities.Section)
                        {
                            foreach (var themes in itestentities.Themes)
                            {
                                if (themes.Id == (listBox_themes.SelectedItem as Themes).Id && themes.Id_section == section.Id && section.Name != "Пользовательский")
                                {
                                    SetClass.Themes = select_themes.ToString();
                                    TestWindow tw = new TestWindow();
                                    tw.ShowDialog();
                                    Close();

                                    SelectThemeWindow stw = new SelectThemeWindow();
                                    stw.Close();

                                    UserWindow uw = new UserWindow();
                                    uw.Close();
                                    uw.ShowDialog();
                                }
                                    else MessageBox.Show("Ошибка! Из этого окна прохождение теста возможно только из пользовательского раздела", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else MessageBox.Show("Вы не выбрали тест", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
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
                var select_themes = listBox_themes.SelectedItem as Themes;

                CheckAccess();

                if (check_access == 0) //у пользователя ограничения для редактирования
                {
                    if (select_themes != null)
                    {
                        foreach (var themes in itestentities.Themes)
                        {
                            if (select_themes.ToString() == themes.Name && themes.Id_user == SetClass.Id)
                            {
                                check_edit++;
                                SetClass.Themes = select_themes.ToString();
                            }
                        }

                        if (check_edit == 0)
                            MessageBox.Show("Вы не создавали этот тест и не можете его изменить", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (check_edit != 0)
                        {
                            TestEditorEditWindow teew = new TestEditorEditWindow();
                            teew.ShowDialog();
                            Close();

                            SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
                            stuw.ShowDialog();
                        }
                    }
                    else MessageBox.Show("Вы не выбрали тест", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else //у администратора нет ограничений
                {
                    if (select_themes != null)
                    {
                        SetClass.Themes = select_themes.ToString();
                        TestEditorEditWindow teew = new TestEditorEditWindow();
                        teew.ShowDialog();
                        Close();

                        SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
                        stuw.ShowDialog();
                    }
                    else MessageBox.Show("Вы не выбрали тест", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Обратитесь к разработчику", "The Intellect Text", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void button_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckAccess();

                foreach (var themes in itestentities.Themes)
                {
                    if (themes.Id_user == SetClass.Id)
                        check_edit++;
                }


                if (check_access == 0 && check_edit == 0) //для пользователя отображаются подсказки 
                {
                    MessageBox.Show("Вы пока не создали ни одного теста. Совeтую вам заглянуть в Помощь.", "Редактор тестов", MessageBoxButton.OK, MessageBoxImage.Information);
                    SetClass.CheckOpen++;
                    FAQWindow fw = new FAQWindow();
                    fw.ShowDialog();

                    TestEditorAddWindow teaw = new TestEditorAddWindow();
                    teaw.ShowDialog();
                    Close();

                    SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
                    stuw.ShowDialog();
                }
                else //для администратора не отображаются
                {
                    TestEditorAddWindow teaw = new TestEditorAddWindow();
                    teaw.ShowDialog();
                    Close();

                    SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
                    stuw.ShowDialog();
                }
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

        private void button_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var select_themes = listBox_themes.SelectedItem as Themes;

                CheckAccess();

                if (check_access == 0) //у пользователя ограничения для редактирования
                {
                    if (select_themes != null)
                    {
                        foreach (var themes in itestentities.Themes)
                        {
                            if (select_themes.ToString() == themes.Name && themes.Id_user == SetClass.Id)
                            {
                                check_edit++;
                                SetClass.Themes = select_themes.ToString();
                            }
                        }

                        if (check_edit == 0)
                            MessageBox.Show("Вы не создавали этот тест и не можете его удалить", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                        else if (check_edit != 0)
                        {
                            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            switch (result)
                            {
                                case MessageBoxResult.Yes:
                                    if (select_themes != null)
                                    {
                                        int wordsmax = 0;
                                        foreach (var words in itestentities.Words)
                                            wordsmax++;

                                        for (int i = 0; i < wordsmax; i++)
                                        {
                                            foreach (var words in itestentities.Words)
                                            {
                                                if (words.Id_themes == select_themes.Id)
                                                { itestentities.Words.Remove(words); break; }
                                            }
                                            itestentities.SaveChanges();
                                        }

                                        itestentities.Themes.Remove(select_themes);
                                        itestentities.SaveChanges();

                                        Close();
                                        SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
                                        stuw.ShowDialog();
                                    }
                                    else
                                        MessageBox.Show("Ошибка! Выберите тест", "Удалить тест", MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                                case MessageBoxResult.No:
                                    break;
                            }
                        }
                    }
                    else MessageBox.Show("Вы не выбрали тест", string.Format("The Intellect Test | Редактор тестов: {0}", select_themes.ToString()), MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else //у администратора нет ограничений
                {
                    MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            if (select_themes != null)
                            {
                                int wordsmax = 0;
                                foreach (var words in itestentities.Words)
                                    wordsmax++;

                                for (int i = 0; i < wordsmax; i++)
                                {
                                    foreach (var words in itestentities.Words)
                                    {
                                        if (words.Id_themes == select_themes.Id)
                                        { itestentities.Words.Remove(words); break; }
                                    }
                                    itestentities.SaveChanges();
                                }

                                itestentities.Themes.Remove(select_themes);
                                itestentities.SaveChanges();

                                Close();
                                SelectThemeUsersWindow stuw = new SelectThemeUsersWindow();
                                stuw.ShowDialog();
                            }
                            else
                                MessageBox.Show("Ошибка! Выберите тест", "Удалить тест", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case MessageBoxResult.No:
                            break;
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
