using ConfigurateService.Class.Database.MSSQL;
using ConfigurateService.Class.Log;
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
using ConfigurateService.Class.Configurate;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Логика взаимодействия для PeakValuePage.xaml
    /// </summary>
    public partial class PeakValuePage : Page
    {

        private ConfigurationManager manager = new ConfigurationManager();
        private Peak? peak;


        public PeakValuePage()
        {
            InitializeComponent();

            tbxPeakValue.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0) && e.Text != ",") ? true : false;

            btnSave.Click += (sender, e) =>
            {

                if (tbxPeakValue.Text != "")
                {
                    peak = new Peak("PeakValue", float.Parse(tbxPeakValue.Text));

                    //manager.SetPeakValue(ref peak);
                    new MSSQL().InstallDBPeakValue();

                    MessageBox.Show("Коэфициент пиковых значений сохранен!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                    MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            };


            //tbxAverageValue.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0) && e.Text != ",") ? true : false;
            //tbxAverageEnd.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0) && e.Text != ",") ? true : false;

            //btnSave.Click += (sender, e) =>
            //{
            //    if (float.Parse(tbxAverageEnd.Text) > 1 || float.Parse(tbxAverageEnd.Text) < 0)
            //    {
            //        MessageBox.Show($"Коэфициент границы прореживания должен быть от 0 до 1!", "Уведомление!", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return;
            //    }

            //    if (tbxPeakValue.Text != "" && tbxAverageValue.Text != "" && tbxAverageEnd.Text != "")
            //    {
            //        peak = new Peak("PeakValue", float.Parse(tbxPeakValue.Text), float.Parse(tbxAverageValue.Text), float.Parse(tbxAverageEnd.Text));

            //        manager.SetPeakValue(ref peak);
            //        new MSSQL().InstallDBPeakValue();
            //        MessageBox.Show("Коэфициенты прореживания сохранены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

            //    }
            //    else
            //        MessageBox.Show("Необходимо заполнить все поля!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            //};
        }
    }
}
