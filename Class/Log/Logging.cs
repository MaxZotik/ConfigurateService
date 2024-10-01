using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConfigurateService.Class.Log
{
    class Logging : ILogger
    {
        private readonly string dir= @$"{Directory.GetCurrentDirectory()}\Loggings\LogsConfigurator\";
        private readonly string path = "app_log.log";
        private Object _lock = new Object();
        public void WtiteLog(in string message, StatusLog status = StatusLog.ACTION)
        {
            DirectoryCheck();

            lock (_lock)
            {
                using (StreamWriter writer = new StreamWriter(dir + path, true))
                    writer.WriteLineAsync($"|{status}| {DateTime.Now} {message}{Environment.NewLine}");
            }
            
        }
        private void DirectoryCheck()
        {
            if (Directory.Exists(dir) != true)
                Directory.CreateDirectory(dir);
        }
        internal string[] GetLogs(in int index)
        {
            List<string> logs = new List<string>();

            if((!File.Exists(dir + path) || !File.Exists(dir + "service_log.log")) && !Directory.Exists(dir))
            {
                logs.Add($"|{StatusLog.ERRORS}| {DateTime.Now} НЕТ ЛОГОВ");
                return logs.ToArray();
            }
            if (index==1)
                return File.ReadLines(dir + path).Where(x=>x.StartsWith("|")).ToArray();
            else if(index==2)
                return File.ReadLines(dir + "service_log.log").Where(x => x.StartsWith("|")).ToArray();
            else if (index == 3)
            {
                logs.AddRange(File.ReadLines(dir + path).Where(x => x.StartsWith("|ERRORS|")).ToArray());
                logs.AddRange(File.ReadLines(dir + "service_log.log").Where(x => x.StartsWith("|Errors|")).ToArray());
                return logs.ToArray();
            }
            else if(index==4)
            {
                logs.AddRange(File.ReadLines(dir + path).Where(x => x.StartsWith("|ACTION|")).ToArray());
                logs.AddRange(File.ReadLines(dir + "service_log.log").Where(x => x.StartsWith("|Action|")).ToArray());
                return logs.ToArray();
            }
            else
            {
                logs.AddRange(File.ReadLines(dir + path).Where(x => x.StartsWith("|")).ToArray());
                logs.AddRange(File.ReadLines(dir + "service_log.log").Where(x => x.StartsWith("|")).ToArray());
                return logs.ToArray();
            }
        }
    }
}
