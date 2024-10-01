using ConfigurateService.Class.Log;
using System.Windows;

namespace ConfigurateService.WIndows
{
    /// <summary>
    /// Interaction logic for logsWIndows.xaml
    /// </summary>
    public partial class logsWIndows : Window
    {
        Logging log = new Logging();
        public logsWIndows()
        {
            InitializeComponent();
            cmbFIlter.ItemsSource = new string[] {"Все", "Только конфигуратор","Только службу","Только ошибки","Только действия" };
            cmbFIlter.SelectedIndex = 0;
            cmbFIlter.SelectionChanged +=(sender,e)=>logs.ItemsSource = log.GetLogs(cmbFIlter.SelectedIndex);
        }
    }
}
