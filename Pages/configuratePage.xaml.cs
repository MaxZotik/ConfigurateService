using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Patterns;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using ConfigurateService.Class.Constants;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Interaction logic for configuratePage.xaml
    /// </summary>
    public partial class configuratePage : Page
    {
        private static int index = 1;
        private int current =0;
        private List<ModbusClient> device = new List<ModbusClient>();
        private AddressRegister register=new AddressRegister();
        private ConfigurationManager config=new ConfigurationManager();
        public configuratePage()
        {
            InitializeComponent();
            tbxChanel.MaxLength = 1;
            tbxAddress.MaxLength = 15;
            cmbParameters.ItemsSource = Constant.FREQUENCY;
            cmbEndians.ItemsSource = Constant.ENDIANS;

            cmbParameters.SelectionChanged += (sender, e) =>
            {
                if(cmbParameters.SelectedIndex==0)
                {
                    cmbType.ItemsSource = null;
                    cmbType.ItemsSource = Constant.PARAMETERS_ONE;
                }
                else
                {
                    cmbType.ItemsSource = null;
                    cmbType.ItemsSource = Constant.PARAMETERS_TWO;
                }
                if (int.TryParse(tbxCount.Text, out int quantity))
                    index = quantity;
            };

            tbxCount.PreviewTextInput += (sender, e) =>
            {
                if (!char.IsDigit(e.Text, 0))
                    e.Handled = true;
            };

            tbxAddress.PreviewTextInput += (sender, e) =>
            {
                for (int i = 0; i < tbxAddress.Text.Length; i++)
                {
                    if (i == 3 || i == 7 || i == 9)
                        if (char.IsPunctuation(tbxAddress.Text, i))
                            e.Handled = false;
                        else
                            e.Handled = true;
                    else
                        if (char.IsDigit(tbxAddress.Text, i))
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
            };

            tbxChanel.PreviewTextInput += (sender, e) => 
            {
                if (char.IsDigit(e.Text, 0) && int.TryParse(e.Text, out int digit) && ( digit>0 && digit < 9))
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

            btnBack.Click += (sender, e) => 
            {
                if (CheckOnCorrect() == true)
                {
                    if(current==0)
                        btnBack.IsEnabled = false;
                    else
                    {
                        current--;
                        btnNext.IsEnabled = true;
                        GetSelectedDevice();
                    }
                }
                else
                    MessageBox.Show("Заполни все строки", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbDevice.Text = $"Устройство: {current+1} из {index}";
            };

            btnNext.Click += (sender, e) => 
            {
                if (CheckOnCorrect() == true)
                {
                    if (index == current)
                    {
                        btnNext.IsEnabled = false;
                        tbDevice.Text = $"Устройство: {current} из {index}";
                    }
                    else
                    {
                        SetNewData();
                        current += 1;
                        btnBack.IsEnabled = true;
                        tbDevice.Text = $"Настроено устройств {current} из {index}";
                    }
                }
                else
                    MessageBox.Show("Заполни все строки", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

            btnPing.Click += (sender, e) =>
            {
                if(IPAddress.TryParse(tbxAddress.Text, out IPAddress address) == true)
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
                        MessageBox.Show("Уже запущен пинг", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                    MessageBox.Show("Заполни поле адрес устройства", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

            btnSave.Click += (sender, e) =>
            {
                if (device.Count < index)
                    MessageBox.Show("Заполните все устройства", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    SearchDevice.NameEquipment(ref device);
                    config.MVKSetting(ref device);                  
                    MessageBox.Show("Успешно настроены", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    btnSave.IsEnabled = btnNext.IsEnabled = btnBack.IsEnabled = false;
                }
                    
            };
        }
        private bool CheckOnCorrect()
        {
            if (tbxAddress.Text != null && cmbEndians.SelectedIndex != -1 && cmbParameters.SelectedIndex != -1 && cmbType.SelectedIndex != -1 && tbxChanel.Text != null && 
                tbxNumber.Text != null && tbxEquipment.Text != null && tbxNameEquipment.Text != null &&
                tbxCount.Text != null && IPAddress.TryParse(tbxAddress.Text, out IPAddress address) == true)
                return true;
            else
                return false;
        }
        private void SetNewData()
        {
            if (((device.Count >= 1 && device.Count == current) || device.Count == 0) && device.Count < index)
            {
                device.Add(new ModbusClient(
                    tbxAddress.Text, 
                    cmbEndians.SelectedValue.ToString(), 
                    cmbParameters.SelectedValue.ToString(), 
                    cmbType.SelectedValue.ToString(), 
                    tbxChanel.Text, 
                    tbxNumber.Text, 
                    tbxEquipment.Text,
                    tbxNameEquipment.Text,
                    tbxDreamChannelName.Text));

                register.SetStartAddress(ref device);
            }
            else
                GetSelectedDevice();
        }
        private void GetSelectedDevice()
        {
            tbxAddress.Text = device[current].IP;
            tbxChanel.Text = device[current].Chanel;
            tbxNumber.Text = device[current].NumberMVK;
            tbxEquipment.Text = device[current].Equipment;
            tbDevice.IsEnabled = false;
            cmbParameters.SelectedValue = device[current].Parameters;
            cmbType.SelectedValue = device[current].Type;
            cmbEndians.SelectedValue = device[current].Endians;
        }



    }
}
