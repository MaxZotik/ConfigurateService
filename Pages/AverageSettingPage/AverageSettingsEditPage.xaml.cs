using ConfigurateService.Class.Database.MSSQL;
using ConfigurateService.Class.Patterns;
using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Constants;
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

namespace ConfigurateService.Pages.AverageSettingPage
{
    /// <summary>
    /// Логика взаимодействия для AverageSettingsEditPage.xaml
    /// </summary>
    public partial class AverageSettingsEditPage : Page
    {
        private readonly ConfigurationManager manager = new ConfigurationManager();
        private List<Average>? averages;
        private List<PeakValueStorage>? peakValueStorages;
        private Average? average;
        private Peak? peak;

        public AverageSettingsEditPage(string nameDb, string nameTable)
        {
            InitializeComponent();

            tbxTimeArchive.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0)) ? true : false;
            tbxTimeAverage.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0)) ? true : false;
            tbxPeakValue.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0) && e.Text != ",") ? true : false;

            cmbTimeArchive.ItemsSource = Constant.ARCHIVETIME;
            cmbTimeAverage.ItemsSource = Constant.ARCHIVETIME;

            tbxNameLevel.Text = nameTable;

            if (nameTable == Constant.PEAKVALUE)
            {
                peakValueStorages = manager.GetPeakValueStorages(nameDb);
                peak = manager.GetPeakValue(nameDb);

                tbxTimeArchive.Text = peakValueStorages[0].Time.ToString();
                cmbTimeArchive.Text = peakValueStorages[0].TimeMesuament;
                tbxPeakValue.Text = peak?.CoefficientPeak.ToString();

                wpTimeAverage.Visibility = Visibility.Hidden;
                wpPeakValue.Visibility = Visibility.Visible;
            }
            else
            {
                averages = manager.GetAverage(nameDb);

                for (int i = 0; i < averages.Count; i++)
                {
                    if (averages[i].Name == nameTable)
                    {
                        average = averages[i];
                    }
                }

                tbxTimeArchive.Text = average?.Value.ToString();
                cmbTimeArchive.Text = average?.MesuamentUnit;
                tbxTimeAverage.Text = average?.TimeValue.ToString();
                cmbTimeAverage.Text = average?.TimeAverage;

                wpTimeAverage.Visibility = Visibility.Visible;
                wpPeakValue.Visibility = Visibility.Hidden;
            }

            btnSave.Click += (sender, e) =>
            {
                bool result = false;

                if (tbxNameLevel.Text == Constant.PEAKVALUE)
                {
                    if (tbxTimeArchive.Text != "" && cmbTimeArchive.SelectedIndex != -1 && tbxPeakValue.Text != "")
                    {
                        peakValueStorages[0].Time = int.Parse(tbxTimeArchive.Text);
                        peakValueStorages[0].TimeMesuament = cmbTimeArchive.SelectedValue.ToString();

                        manager.SetPeakValueStorages(peakValueStorages[0], nameDb);

                        peak = new Peak(Constant.PEAKVALUE, float.Parse(tbxPeakValue.Text));
                        manager.SetPeakValue(ref peak, nameDb);

                        result = true;
                    }                 
                }
                else
                {
                    if (tbxTimeArchive.Text != "" && cmbTimeArchive.SelectedIndex != -1 && tbxTimeAverage.Text != "" && cmbTimeAverage.SelectedIndex != -1)
                    {
                        for (int i = 0; i < averages.Count; i++)
                        {
                            if (averages[i].Name == nameTable)
                            {
                                averages[i].Value = int.Parse(tbxTimeArchive.Text);
                                averages[i].MesuamentUnit = cmbTimeArchive.SelectedValue.ToString();
                                averages[i].TimeValue = int.Parse(tbxTimeAverage.Text);
                                averages[i].TimeAverage = cmbTimeAverage.SelectedValue.ToString();
                            }
                        }

                        manager.SetAverage(ref averages, nameDb);

                        result= true;
                    }                   
                }

                if (result)
                {
                    MessageBox.Show($"Настройки таблицы {tbxNameLevel.Text} сохранены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            };

        }
    }
}
