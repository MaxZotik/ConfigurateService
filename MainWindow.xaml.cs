using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Managment;
using ConfigurateService.Pages;
using System.Windows;

namespace ConfigurateService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigurationManager manager = new ConfigurationManager();
        public MainWindow()
        {
            InitializeComponent();
            Manager.Frame = mainFrame;
            btnBack.Click += (sender, e) => mainFrame.GoBack();
            btnHelper.Click +=  (sender, e) => manager.OpenWordFile();
            mainFrame.ContentRendered += (sender, e) => btnBack.Visibility = mainFrame.CanGoBack ? Visibility.Visible : Visibility.Hidden;
            Manager.Frame.Navigate(new mainPage());
        }
    }
}
