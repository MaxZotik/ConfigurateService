using ConfigurateService.Class.Managment;
using ConfigurateService.WIndows;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConfigurateService.Pages.DatabasePage;
using ConfigurateService.Pages.AverageSettingPage;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Interaction logic for mainPage.xaml
    /// </summary>
    public partial class mainPage : Page
    {
        public mainPage()
        {
            InitializeComponent();
            btnDBSettings.Click += (sender, e) => Manager.Frame.Navigate(new SelectDatabaseSettingsPage());
            btnSetting.Click += (sender, e) => Manager.Frame.Navigate(new selectConfigPage());
            btnLogs.Click += (sender, e) => 
            {
                if (!App.Current.Windows.OfType<logsWIndows>().Any())
                {
                    logsWIndows logs = new logsWIndows();
                    logs.Show();
                }
                else
                    MessageBox.Show("Уже запущено", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
             };
            btnStopService.Click += (sender, e) => Manager.Frame.Navigate(new servicePage());
            btnAvg.Click += (sender, e) => Manager.Frame.Navigate(new AveragePage());
            btnServerSettings.Click += (sender, e) => Manager.Frame.Navigate(new EditServerSettingsPage()); 
        }
    }
}
