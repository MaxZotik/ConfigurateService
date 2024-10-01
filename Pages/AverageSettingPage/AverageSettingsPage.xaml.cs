using ConfigurateService.Class.Database.MSSQL;
using ConfigurateService.Class.Patterns;
using ConfigurateService.Class.Constants;
using ConfigurateService.Class.Configurate;
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
    /// Логика взаимодействия для AverageSettingsPage.xaml
    /// </summary>
    public partial class AverageSettingsPage : Page
    {
        private readonly ConfigurationManager manager = new ConfigurationManager();
        private List<Average> averages;
        private List<PeakValueStorage> peakValueStorages;
        private Average? average;
        private PeakValueStorage? peakValueStorage;
        private Peak? peak;
        private MSSQL mssql = new MSSQL();


        public AverageSettingsPage(string subd, string nameDb)
        {
            InitializeComponent();

            averages = manager.GetAverage(nameDb);
            peakValueStorages = manager.GetPeakValueStorages(nameDb);

            tbxTimeArchive.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0)) ? true : false;
            tbxTimeAverage.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0)) ? true : false;
            tbxPeakValue.PreviewTextInput += (sender, e) => e.Handled = (!char.IsDigit(e.Text, 0) && e.Text != ",") ? true : false;

            cmbTimeArchive.ItemsSource = Constant.ARCHIVETIME;
            cmbTimeAverage.ItemsSource = Constant.ARCHIVETIME;

            tbxNameLevel.Text = GetNameLevel();
            btnInstal.IsEnabled = false;

            Rendering();

            btnSave.Click += (sender, e) =>
            {
                if (tbxNameLevel.Text == Constant.PEAKVALUE)
                {
                    peakValueStorage = new PeakValueStorage(
                    tbxNameLevel.Text,
                    int.Parse(tbxTimeArchive.Text),
                    cmbTimeArchive.SelectedValue.ToString());

                    peakValueStorages.Add(peakValueStorage);
                    manager.SetPeakValueStorages(peakValueStorage, nameDb);

                    peak = new Peak(Constant.PEAKVALUE, float.Parse(tbxPeakValue.Text));
                    manager.SetPeakValue(ref peak, nameDb);
                }
                else
                {
                    average = new Average(tbxNameLevel.Text,
                    int.Parse(tbxTimeArchive.Text),
                    cmbTimeArchive.SelectedValue.ToString(),
                    int.Parse(tbxTimeAverage.Text),
                    cmbTimeAverage.SelectedValue.ToString());

                    averages.Add(average);
                    manager.SetAverage(ref averages, nameDb);
                }

                btnInstal.IsEnabled = true;
                MessageBox.Show($"Настройки таблицы {tbxNameLevel.Text} сохранены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            };


            btnInstal.Click += (sender, e) =>
            {
                if (mssql.CheckOnNameDb(subd, nameDb))
                {
                    if (CheckOnNameTable(subd, nameDb))
                    {
                        if (tbxNameLevel.Text == Constant.ARCHIVE)
                        {
                            average = new Average(tbxNameLevel.Text,
                            int.Parse(tbxTimeArchive.Text),
                            cmbTimeArchive.SelectedValue.ToString(),
                            int.Parse(tbxTimeAverage.Text),
                            cmbTimeAverage.SelectedValue.ToString());

                            mssql.InstallDBArchive();
                        }
                        else if (tbxNameLevel.Text == Constant.PEAKVALUE)
                        {
                            peakValueStorage = new PeakValueStorage(
                            tbxNameLevel.Text,
                            int.Parse(tbxTimeArchive.Text),
                            cmbTimeArchive.SelectedValue.ToString());

                            mssql.InstallDBPeakValue();
                        }
                        else if (tbxNameLevel.Text == (Constant.ARCHIVELEVEL + "1") || tbxNameLevel.Text == (Constant.ARCHIVELEVEL + "2"))
                        {
                            average = new Average(tbxNameLevel.Text,
                            int.Parse(tbxTimeArchive.Text),
                            cmbTimeArchive.SelectedValue.ToString(),
                            int.Parse(tbxTimeAverage.Text),
                            cmbTimeAverage.SelectedValue.ToString());

                            mssql.InstallDBAverage(average);
                        }
                        else
                        {
                            average = new Average(tbxNameLevel.Text,
                            int.Parse(tbxTimeArchive.Text),
                            cmbTimeArchive.SelectedValue.ToString(),
                            int.Parse(tbxTimeAverage.Text),
                            cmbTimeAverage.SelectedValue.ToString());

                            mssql.InstallDBAverageNext(average);
                        }

                        MessageBox.Show($"Таблица {tbxNameLevel.Text} создана в БД!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"На сервере СУБД: {subd} БД с именем {nameDb} уже создана!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }                    
                }
                else
                {
                    MessageBox.Show($"На сервере СУБД: {subd} БД с именем {nameDb} не создана! Необходимо сначала создать БД {nameDb}!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                btnInstal.IsEnabled = false;
            };
        }

        /// <summary>
        /// Метод задает наименование таблиц для БД
        /// </summary>
        /// <returns>Наименование таблицы БД</returns>
        private string GetNameLevel()
        {
            string result = "";

            int countAverage = averages.Count;
            int countPeak = peakValueStorages.Count;


            if (countAverage == 0)
            {
                result = Constant.ARCHIVE;
            }
            else if (countPeak == 0)
            {
                result = Constant.PEAKVALUE;
            }
            else if (countAverage > 0)
            {
                result = Constant.ARCHIVELEVEL + $"{countAverage}";
            }

            return result;
        }

        private void Rendering()
        {
            if (tbxNameLevel.Text == Constant.PEAKVALUE)
            {
                wpTimeAverage.Visibility = Visibility.Hidden;
                wpPeakValue.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Метод проверяет наличие таблицы в БД
        /// </summary>
        /// <param name="subd">Наименование СУБД</param>
        /// <param name="nameDb">Наименование БД</param>
        /// <returns>Возвращает True - если в БД нет такой таблицы и False - если в БД такая таблица уже есть</returns>
        private bool CheckOnNameTable(string subd, string nameDb)
        {
            string[] nameTableDb = mssql.GetCurrentTableDB(subd, nameDb);

            if (nameTableDb.Length == 0)
                return true;

            foreach (string db in nameTableDb)
            {
                if (tbxNameLevel.Text == db)
                    return false;
            }

            return true;
        }
    }
}
