using ConfigurateService.Class.Managment;
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

namespace ConfigurateService.Pages.DatabasePage
{
    /// <summary>
    /// Логика взаимодействия для SelectDatabaseSettingsPage.xaml
    /// </summary>
    public partial class SelectDatabaseSettingsPage : Page
    {
        public SelectDatabaseSettingsPage()
        {
            InitializeComponent();
            btnSettingsNew.Click += (sender, e) => Manager.Frame.Navigate(new DatabaseSettingsPage());
            btnSettingsCurrent.Click += (sender, e) => Manager.Frame.Navigate(new EditDatabaseSettingsPage());
        }
    }
}
