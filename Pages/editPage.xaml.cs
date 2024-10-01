using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Patterns;
using ConfigurateService.Class.Constants;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Interaction logic for editPage.xaml
    /// </summary>
    public partial class editPage : Page
    {
        private List<ModbusClient> list = new List<ModbusClient>();
        private AddressRegister register = new AddressRegister();
        private ConfigurationManager manager = new ConfigurationManager();
        public editPage()
        {
            InitializeComponent();
            list = CurrentModbusClient.Current;

            //Полоса частот
            cmbFrequency.ItemsSource = Constant.FREQUENCY;
            
            //Последовательность передачи байт "Big Endian"
            cmbEndians.ItemsSource = Constant.ENDIANS;

            ///Устанавливает параметр оборудования
            cmbFrequency.SelectionChanged += (sender, e) =>
            {
                if (cmbFrequency.SelectedIndex == 0)
                {
                    cmbType.ItemsSource = null;
                    cmbType.ItemsSource = Constant.PARAMETERS_ONE;
                }
                else
                {
                    cmbType.ItemsSource = null;
                    cmbType.ItemsSource = Constant.PARAMETERS_TWO;
                }
            };

            ///Устанавливает канал (1-9)
            tbxChanel.PreviewTextInput += (sender, e) =>
            {
                if (char.IsDigit(e.Text, 0) && int.TryParse(e.Text, out int digit) && (digit > 0 && digit < 9))
                    e.Handled = false;
                else
                    e.Handled = true;
            };

            tbxNumber.PreviewTextInput += (sender, e) =>
            {
                if (!char.IsDigit(e.Text, 0))
                    e.Handled = true;
            };

            tbxEquipment.PreviewTextInput += (sender, e) =>
            {
                if (!char.IsDigit(e.Text, 0))
                    e.Handled = true;
            };

            ///Проверка соединения IPAdress
            btnPing.Click += (sender, e) =>
            {
                btnPing.IsEnabled = false;

                if (IPAddress.TryParse(tbxAddress.Text, out IPAddress address) == true)
                {
                    if (Process.GetProcessesByName("cmd").Length < 1)
                    {
                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = "cmd",
                            Arguments = $"/c ping {address} -t",
                        });
                    }
                    else
                        MessageBox.Show("Пинг уже запущен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }

                btnPing.IsEnabled = true;
             };

            ///Сохранение настроек в файл "MVKSettings.xml"
            btnSave.Click += (sender, e) =>
            {
                if (CheckOnCorrect() == true)
                {
                    ModbusClient temp = new ModbusClient(
                        tbxAddress.Text,
                        cmbEndians.SelectedValue.ToString(),
                        cmbFrequency.SelectedValue.ToString(),
                        cmbType.SelectedValue.ToString(),
                        tbxChanel.Text,
                        tbxNumber.Text,
                        tbxEquipment.Text,
                        tbxNameEquipment.Text,
                        tbxDreamChannelName.Text);

                    SearchDevice.NameEquipment(temp);

                    list.Add(temp);
                    register.SetStartAddress(ref list);                   

                    manager.MVKSetting(ref list);
                    MessageBox.Show("Настройки оборудования сохранены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    btnSave.IsEnabled = false;
                }
                else
                    MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

        }

        /// <summary>
        /// Метод проверяет правильность заполнения полей
        /// </summary>
        /// <returns>Возвращает (true / false)</returns>
        private bool CheckOnCorrect()
        {
            if (tbxAddress.Text != null && cmbEndians.SelectedIndex != -1 && cmbFrequency.SelectedIndex != -1 && cmbType.SelectedIndex != -1 && tbxChanel.Text != null &&
                tbxNumber.Text != null && tbxEquipment.Text != null && tbxNameEquipment.Text != null &&
                IPAddress.TryParse(tbxAddress.Text, out IPAddress address) == true)
                return true;
            else
                return false;
        }
    }
}
