using ConfigurateService.Class.Configurate;
using ConfigurateService.Class.Database.MSSQL;
using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Managment;
using ConfigurateService.Class.Patterns;
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
    /// Логика взаимодействия для AveragePage.xaml
    /// </summary>
    public partial class AveragePage : Page
    {
        private MSSQL mssql = new MSSQL();
        private ConfigurationManager manager = new ConfigurationManager();
        private ServiceManager service = new ServiceManager();
        private List<DBClientFull> dbList = new List<DBClientFull>();

        public AveragePage()
        {
            InitializeComponent();
            if (MessageBox.Show("Прежде чем настроить прореживание, надо отключить службу. Отключить службу?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                cmbDatabase.IsEnabled = false;
            }
            else
            {
                service.StopService();
            }

            cmbDatabase.ItemsSource = Constant.SUBD;

            Rendering();

            cmbDatabase.SelectionChanged += (sender, e) =>
            {
                cmbBDname.ItemsSource = null;

                if (cmbDatabase.SelectedIndex != -1)
                {
                    ResizeList();

                    var nameDbArray = dbList.Select(p => p.Database);

                    cmbBDname.ItemsSource = nameDbArray;
                }
            };

            cmbBDname.SelectionChanged += (sender, e) =>
            {
                if (cmbDatabase.SelectedIndex != -1)
                {
                    Rendering();
                }
            };

            btnAverage.Click += (sender, e) => Manager.Frame.Navigate(new AverageSettingsPage(cmbDatabase.SelectedValue.ToString(), cmbBDname.SelectedValue.ToString()));

            btnAverageEdit.Click += (sender, e) =>
            {
                if (lbAverage.SelectedIndex != -1 && lbAverage.Items[0].ToString() != "Нет уровней прореживания")
                {
                    Manager.Frame.Navigate(new AverageSettingsEditPage(cmbBDname.SelectedValue.ToString(), lbAverage.SelectedValue.ToString()));
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать элемент для редактирования!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            };

            Manager.Frame.ContentRendered += (sender, e) => 
            { 
                Rendering(); 
            };
        }

        /// <summary>
        /// Метод обновляет в форме список наименований таблиц
        /// </summary>
        private void Rendering()
        {
            lbAverage.ItemsSource = null;
            string[] tempNameTable = Array.Empty<string>();

            if (cmbDatabase.SelectedIndex != -1 && cmbBDname.SelectedIndex != -1)
            {
                if (cmbDatabase.SelectedIndex == (int)NameDatabase.MSSQL)
                {
                    List<PeakValueStorage> listPeak = manager.GetPeakValueStorages(cmbBDname.SelectedValue.ToString());

                    if (listPeak.Count != 0)
                    {
                        tempNameTable = tempNameTable.Append(listPeak[0].NameTable).ToArray();
                    }

                    List<Average> listAverage = manager.GetAverage(cmbBDname.SelectedValue.ToString());

                    if (listAverage.Count != 0)
                    {
                        foreach (Average average in listAverage)
                        {
                            tempNameTable = tempNameTable.Append(average.Name).ToArray();
                        }
                        
                    }
                }

                if (tempNameTable == Array.Empty<string>())
                {
                    tempNameTable = tempNameTable.Append("Нет уровней прореживания").ToArray();
                }
                
                lbAverage.ItemsSource = tempNameTable;             
            }
        }

        //private void Rendering()
        //{
        //    lbAverage.ItemsSource = null;

        //    if (cmbDatabase.SelectedIndex != -1 && cmbBDname.SelectedIndex != -1)
        //    {
        //        if (cmbDatabase.SelectedIndex == (int)NameDatabase.MSSQL)
        //        {
        //            lbAverage.ItemsSource = mssql.GetCurrentTableDB(cmbDatabase.Text, cmbBDname.SelectedValue.ToString());
        //        }
        //    }
        //}


        /// <summary>
        /// Метод заполняет список подключений к БД
        /// </summary>
        private void ResizeList()
        {
            dbList.Clear();
            manager.GetSettingsOnDB(cmbDatabase.SelectedValue.ToString(), in dbList);
        }
    }
}
