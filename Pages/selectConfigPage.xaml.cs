using ConfigurateService.Class.Managment;
using System.Windows.Controls;

namespace ConfigurateService.Pages
{
    /// <summary>
    /// Interaction logic for selectConfigPage.xaml
    /// </summary>
    public partial class selectConfigPage : Page
    {
        public selectConfigPage()
        {
            InitializeComponent();
            btnSettingsNew.Click += (sender, e) => Manager.Frame.Navigate(new configuratePage());
            btnSettingsCurrent.Click += (sender, e) => Manager.Frame.Navigate(new ConfigurateCurrentPage());
        }
    }
}
