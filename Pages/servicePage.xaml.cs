using ConfigurateService.Class.Configurate;
using System.Windows;
using System.Windows.Controls;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Interaction logic for servicePage.xaml
    /// </summary>
    public partial class servicePage : Page
    {
        private readonly ServiceManager service = new ServiceManager();

        public servicePage()
        {
            InitializeComponent();

            btnServiceInstall.Click += (sender, e) =>
            {
                service.InstallService();
            };

            btnServiсeStop.Click += (sender, e) =>
            {
                service.StopService();
            };

            btnServiсeStart.Click += (sender, e) =>
            {
                service.StartService();
            };

            btnServiceDelete.Click += (sender, e) =>
            {
                service.DeleteService();
            };
        }
    }
}
