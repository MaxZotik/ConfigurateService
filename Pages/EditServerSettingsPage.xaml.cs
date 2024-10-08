using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Constants;
using ConfigurateService.Class.Managment;
using ConfigurateService.Class.Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditServerSettingsPage.xaml
    /// </summary>
    public partial class EditServerSettingsPage : Page
    {
        private ServerSettings? serverSettings;
        private ConfigurationManager configurationManager = new ConfigurationManager();
        private List<DBClientFull> dbList = new List<DBClientFull>();

        public EditServerSettingsPage()
        {
            InitializeComponent();

            cmbDatabase.ItemsSource = Constant.SUBD;

            cmbDatabase.SelectionChanged += (sender, e) =>
            {
                cmbBDname.ItemsSource = null;

                if (cmbDatabase.SelectedIndex != -1)
                {
                    ResizeList();

                    var nameDbArray = dbList.Select(p => p.Database);

                    cmbBDname.ItemsSource = nameDbArray;
                }
            };

            cmbBDname.SelectionChanged += (sender, e) =>
            {
                if (cmbDatabase.SelectedIndex != -1)
                {
                   ResizeServerSetting(cmbBDname.SelectedValue.ToString());
                }
            };

            tbxIP.PreviewTextInput += (sender, e) =>
            {
                bool result = true;

                if (char.IsDigit(e.Text, 0) || char.IsPunctuation(e.Text, 0))
                    result = false;

                e.Handled = result;
            };

           tbxPort.PreviewTextInput += (sender, e) =>
            {
                if (!char.IsDigit(e.Text, 0))
                    e.Handled = true;
            };
                      

            btnSave.Click += (sender, e) => 
            {

                if (CheckOnCorrect())
                {
                    string str = $@"{cmbBDname.SelectedValue}\ServerSettings.xml";

                    ServerSettings serverSettingsTemp = new ServerSettings(tbxIP.Text, tbxPort.Text);
                    configurationManager.ServerSettingsSave(serverSettingsTemp, cmbBDname.SelectedValue.ToString());

                    MessageBox.Show($@"Настройки подключения записаны в файл: {str}!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Не все поля заполнены или введены не корректные значения! Настройки подключения не записаны!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };
        }

        /// <summary>
        /// Метод проверяет заполнены ли поля
        /// </summary>
        /// <returns>Возвращает True все поля заполнены</returns>
        private bool CheckOnCorrect()
        {
            if (cmbDatabase.SelectedIndex != -1 && cmbBDname.SelectedIndex != -1 &&
                IPAddress.TryParse(tbxIP.Text, out IPAddress address) == true && tbxPort.Text != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Метод заполняет список подключений к БД
        /// </summary>
        private void ResizeList()
        {
            dbList.Clear();
            configurationManager.GetSettingsOnDB(cmbDatabase.SelectedValue.ToString(), in dbList);
        }

        private void ResizeServerSetting(string nameDb)
        {
            serverSettings = configurationManager.ServerSettingsLoad(nameDb);

            if (serverSettings != null)
            {
                tbxIP.Text = serverSettings.IP;
                tbxPort.Text = serverSettings.Port;
            }
        }

    }
}
