using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Database.MSSQL;
using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Patterns;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ConfigurateService.Class.Constants;

namespace ConfigurateService.Pages.DatabasePage
{
    /// <summary>
    /// Логика взаимодействия для DatabaseSettings.xaml
    /// </summary>
    public partial class DatabaseSettingsPage : Page
    {
        private MSSQL mssql = new MSSQL();
        private ConfigurationManager configuration = new ConfigurationManager();
        private CheckDatabase check = new CheckDatabase();

        public DatabaseSettingsPage()
        {
            InitializeComponent();

            //Ресурс для поля "Выберите CУБД"
            cmbDatabase.ItemsSource = Constant.SUBD;
            cmbDatabase.SelectedValue = Constant.SUBD[0];

            #region cmbDatabase.SelectionChanged

            ///Проверяет наличие сервера и выводит перечень доступных БД на сервере
            //cmbDatabase.SelectionChanged += (sender, e) =>
            //{
            //    cmbDB.ItemsSource = null;

            //    if (cmbDatabase.SelectedIndex != -1)
            //    {
            //        if (check.Check(NameDatabase.MSSQL) == true)
            //        {
            //            mssql.GetCurrentDatabases(out List<string> database);
            //            cmbDB.ItemsSource = database;
            //        }
            //        else
            //        {
            //            MessageBox.Show("MSSQL не установлена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        }
            //    }
            //};

            #endregion

            ///Проверяет наличие сервера и выводит перечень доступных БД на сервере
            cmbDatabase.SelectionChanged += (sender, e) =>
            {
                if (cmbDatabase.SelectedIndex != -1)
                {
                    if (!check.Check(NameDatabase.MSSQL))
                    {
                        MessageBox.Show("MSSQL не установлена!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            };

            ///Изменение активной БД в файле "DatabaseSettings.xml"
            //chkActive.Checked += (sender, e) =>
            //{
            //    if (cmbDatabase.SelectedIndex != -1)
            //        configuration.SetActiveDatabase(cmbDatabase.SelectedValue.ToString(), StatusDatabase.Active);
            //    else
            //        MessageBox.Show("Не выбрана БД!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            //};

            #region Старый chkActive.Checked

            //chkActive.Checked += (sender, e) =>
            //{
            //    if (cmbDatabase.SelectedIndex != -1)
            //        configuration.SetActiveDatabase(cmbDatabase.SelectedValue.ToString(), StatusDatabase.Active);
            //};

            #endregion

            #region Старый - btnConfig.Click

            //btnConfig.Click += (sender, e) =>
            //{
            //    if (Validate())
            //    {
            //        if (registry.IsHaveComponents())
            //        {
            //            List<DBClient> client = new List<DBClient>(1);
            //            client.Add(new DBClient(cmbDatabase.SelectedValue.ToString(), txbServer.Text, cmbDB.SelectedValue.ToString(), txbLogin.Text, txbPsw.Password));
            //            configuration.DatabaseSettings(ref client, chkActive.IsChecked.Value == true ? StatusDatabase.Active : StatusDatabase.NoActive);
            //            MessageBox.Show("Данные записаны", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            //        }
            //        else
            //        {
            //            MessageBox.Show("Не могу записать, не установлены компоненты", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            btnConfig.IsEnabled = false;
            //            btnInstall.Visibility = Visibility.Visible;
            //        }
            //    }
            //    else
            //        MessageBox.Show("Заполни поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            //};

            #endregion

            ///Сохраняет настройки подключения к БД в файл "DatabaseSettings.xml"
            btnConfigSave.Click += (sender, e) =>
            {
                if (Validate())
                {
                    if (configuration.CheckNameDb(cmbDatabase.Text, txtDB.Text))
                    {
                        configuration.CereateDirectorySetting(txtDB.Text);

                        DBClient client = new DBClient(cmbDatabase.Text,
                        txbServer.Text,
                        txtDB.Text,
                        txbLogin.Text,
                        txbPsw.Password);

                        configuration.SaveDatabaseSettings(ref client, chkActive.IsChecked.Value == true ? StatusDatabase.Active : StatusDatabase.NoActive);
                        MessageBox.Show("Данные записаны в файл!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    else
                    {
                        MessageBox.Show($"Настройки с названием БД \"{txtDB.Text}\" уже есть в файле настроек! Необходимо изменить название БД!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }                   
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                    
            };

            ///Проверяет подключение к БД
            btnTest.Click += (sender, e) =>
            {
                if (Validate())
                {
                    DBClient client = new DBClient(
                        cmbDatabase.Text,
                        txbServer.Text,
                        txtDB.Text,
                        txbLogin.Text,
                        txbPsw.Password);

                    TestConnect(NameDatabase.MSSQL, client);
                }
                else
                    MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

            ///Проверяет наличие установленых компонентов в БД
            btnCheck.Click += (sender, e) =>
            {
                if (Validate())
                {
                    DBClient client = new DBClient(cmbDatabase.Text,
                        txbServer.Text,
                        txtDB.Text,
                        txbLogin.Text,
                        txbPsw.Password);

                    //configuration.SetDataForDisconnect(client);
                    CheckComponents(NameDatabase.MSSQL, client);
                }
                else
                    MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };


            ///Устанавливает необходимые компоненты в БД
            btnInstall.Click += (sender, e) =>
            {
                if (cmbDatabase.SelectedIndex == (int)NameDatabase.MSSQL && Validate() && chkActive.IsChecked == true)
                {
                    mssql.CreateDB(txtDB.Text);

                    DBClient client = new DBClient(cmbDatabase.Text,
                        txbServer.Text,
                        txtDB.Text,
                        txbLogin.Text,
                        txbPsw.Password);

                    configuration.SetDataForDisconnect(client);

                    mssql.InstallDBComponets();

                    MessageBox.Show("Компоненты установлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);                 
                }
                else
                    MessageBox.Show("Компоненты не установлены! Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
        }

        /// <summary>
        /// Метод проверяет на корретность заполнения полей формы
        /// </summary>
        /// <returns>Возвращает значение (true / false)</returns>
        private bool Validate()
        {
            return (cmbDatabase.SelectedIndex != -1 && txtDB.Text != null
                && txbLogin.Text != null && txbServer.Text != null && txbPsw.Password != null) ? true : false;
        }


        /// <summary>
        /// Метод проверяет подключение к БД
        /// </summary>
        /// <param name="database">Перечисление названия БД</param>
        private void TestConnect(NameDatabase database, DBClient client)
        {
            bool answer = database == NameDatabase.MSSQL ? mssql.CheckConnectionTest(client) : false;
            MessageBox.Show(answer == true ? @$"Подключение {database} прошло успешно!" :
                @$"Подключение {database} прошло с ошибкой, необходимо исправить настройки!",
                "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /// <summary>
        /// Метод проверяет установлены ли компоненты в БД
        /// </summary>
        /// <param name="database">Перечисление названия БД</param>
        //private void CheckComponents(NameDatabase database)
        //{
        //    bool answer = database == NameDatabase.MSSQL ? mssql.CheckOnComponents() : false;
        //    MessageBox.Show(answer == true ? @$"Компоненты для { database } установлены!" : 
        //        @$"Компоненты для {database} не установлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        //}

        /// <summary>
        /// Метод проверяет существует ли БД
        /// </summary>
        /// <param name = "database">Название БД</param>
        private void CheckComponents(NameDatabase database, DBClient client)
        {
            bool answer = database == NameDatabase.MSSQL ? mssql.CheckOnComponents(client) : false;
            MessageBox.Show(answer == true ? @$"Компоненты для {database} установлены!" :
                @$"Компоненты для {database} не установлены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
