using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Managment;
using ConfigurateService.Class.Patterns;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Interaction logic for ConfigurateCurrentPage.xaml
    /// </summary>
    public partial class ConfigurateCurrentPage : Page
    {
        private ConfigurationManager configuration = new ConfigurationManager();
        List<ModbusClient>? modbusClient =new List<ModbusClient>();
        public ConfigurateCurrentPage()
        {
            InitializeComponent();
            Rendering();

            if (modbusClient==null)
            {
                btnRemove.IsEnabled= false;
                btnAdd.IsEnabled=false;
            }

            ///Удаление выбранного оборудования
            btnRemove.Click += (sender, e) =>
            {
                if (lbxDevice.SelectedIndex != -1)
                {
                    modbusClient.RemoveAt(lbxDevice.SelectedIndex);
                    configuration.MVKSetting(ref modbusClient);
                    Rendering();
                }
                else
                    MessageBox.Show("Выберите элемент", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            };

            ///Добавление нового оборудования
            btnAdd.Click += (sender, e) =>
            {
                CurrentModbusClient.SetCurrentList(modbusClient);
                Manager.Frame.Navigate(new editPage());
            };

            btnUpdate.Click += (sender, e) =>
            {
                List<ModbusClient> modbusClientsList = configuration.GetAllDevice();
                SearchDevice.NameEquipment(ref modbusClientsList);
            };
        }

        /// <summary>
        /// Обновление оборудования в листе
        /// </summary>
        private void Rendering()
        {
            modbusClient = null;
            configuration.GetAllDevice(out List<string> devices, out List<ModbusClient> modbus);
            lbxDevice.ItemsSource = devices;
            if(modbus.Count>0)
                modbusClient = modbus;
        }
    }
}
