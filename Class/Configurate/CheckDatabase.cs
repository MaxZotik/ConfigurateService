using ConfigurateService.Class.Enums;
using System.IO;
using System;

namespace ConfigurateService.Class.Configurate
{
    class CheckDatabase
    {
        internal bool Check(NameDatabase database)
        {
            string[] list = Environment.GetLogicalDrives();
            for (int i = 0; i < list.Length; i++)
            {
                foreach (string folder in Directory.GetDirectories(list[i]))
                {
                    if ((folder.Contains("Documents and Settings") || folder.StartsWith(@"C:\$") || folder.Contains(@":\System Volume") || folder.EndsWith("WindowsApps") || folder.Contains("Config.Msi")) || folder.StartsWith("W:"))
                        continue;
                    else
                        foreach (string subfolder in Directory.GetDirectories(folder))
                        {
                            if (database == NameDatabase.PostgreSQL)
                            {
                                if (subfolder.EndsWith("PostgreSQL"))
                                    return true;
                                else
                                    continue;
                            }
                            else if (database == NameDatabase.MSSQL)
                            {
                                if (subfolder.EndsWith("Microsoft SQL Server"))
                                    return true;
                                else
                                    continue;
                            }
                            else
                                continue;
                        }
                }
            }
            return false;
        }
    }
}
