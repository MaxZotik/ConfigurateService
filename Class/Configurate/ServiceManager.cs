using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ConfigurateService.Class.Configurate
{
    class ServiceManager
    {
        /// <summary>
        /// Метод останавливает службу
        /// </summary>
        internal void StopService()
        {
            Cmd("cmd", "sc stop ArchiveService");
            Cmd("cmd", "taskkill /IM ArchiveService.exe /F");
        }

        /// <summary>
        /// Метод запускает службу
        /// </summary>
        internal void StartService() 
        {
            Cmd("cmd", "sc start ArchiveService");
        }

        /// <summary>
        /// Метод устанавливает службу
        /// </summary>
        internal void InstallService()
        { 
            Cmd("cmd.exe", $@"cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319 & InstallUtil.exe ""{Directory.GetCurrentDirectory()}\ArchiveService.exe""");
        }


        /// <summary>
        /// Метод удаления службы
        /// </summary>
        internal void DeleteService()
        {
            Cmd("cmd.exe", $@"cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319 & InstallUtil.exe /u ""{Directory.GetCurrentDirectory()}\ArchiveService.exe""");
        }

        /// <summary>
        /// Метод для запуска командной строки
        /// </summary>
        /// <param name="fileName">Название файла для запуска</param>
        /// <param name="cmd">Команда для командной строки</param>
        private void Cmd(string fileName, string cmd)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = $"{fileName}",
                Arguments = $@"/c {cmd}",
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true
            });
        }



    }
}
