using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Patterns;
using System;
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
        public EditServerSettingsPage()
        {
            InitializeComponent();

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

            serverSettings = configurationManager.ServerSettingsLoad();

            if (serverSettings != null)
            {
                tbxIP.Text = serverSettings.IP;
                tbxPort.Text = serverSettings.Port;
            }

            btnSave.Click += (sender, e) => 
            {

                if (CheckOnCorrect())
                {
                    ServerSettings serverSettingsTemp = new ServerSettings(tbxIP.Text, tbxPort.Text);
                    configurationManager.ServerSettingsSave(serverSettingsTemp);

                    MessageBox.Show("Настройки подключения записаны в файл \"ServerSettings.xml\"!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Настройки подключения не записаны!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };
        }

        /// <summary>
        /// Метод проверяет заполнены ли поля
        /// </summary>
        /// <returns>Возвращает True все поля заполнены</returns>
        private bool CheckOnCorrect()
        {
            if (IPAddress.TryParse(tbxIP.Text, out IPAddress address) == true && tbxPort.Text != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
