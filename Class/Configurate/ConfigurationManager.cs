using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Log;
using ConfigurateService.Class.Patterns;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Xml;

namespace ConfigurateService.Class.Configurate
{
    class ConfigurationManager
    {
        private static readonly string dirSettings = $@"{Directory.GetCurrentDirectory()}\Settings\";
        private string pathMVKSettings = $@"{dirSettings}MVKSettings.xml";
        private string pathDatabaseSettings = $@"{dirSettings}DatabaseSettings.xml";
        private string path = "";

        private readonly Logging logger = new Logging();
        private static List<DBClient> database = new List<DBClient>();

        /// <summary>
        /// Метод записи настроек оборудования в файл "MVKSettings.xml"
        /// </summary>
        /// <param name="modbus"></param>
        internal void MVKSetting(ref List<ModbusClient> modbus)
        {
            RemoveOldXML();
            XmlTextWriter writer = new XmlTextWriter(pathMVKSettings, null);

            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;
            writer.WriteStartDocument();
            writer.WriteStartElement("MVKSettings");

            for (int i = 0; i < modbus.Count; i++)
            {
                writer.WriteStartElement($"Device");            //<Device Index="">
                writer.WriteStartAttribute("Index");                //<IPAddress></IPAddress>
                writer.WriteString($"{i + 1}");                     //<Endians></Endians>
                writer.WriteEndAttribute();                         //<Parameters></Parameters>
                writer.WriteStartElement("IPAddress");              //<Type></Type>
                writer.WriteString(modbus[i].IP);                   //<Chanel></Chanel>
                writer.WriteEndElement();                           //<StartAddress></StartAddress>
                writer.WriteStartElement("Endians");            //</Device>
                writer.WriteString(modbus[i].Endians);
                writer.WriteEndElement();
                writer.WriteStartElement("Parameters");
                writer.WriteString(modbus[i].Parameters);
                writer.WriteEndElement();
                writer.WriteStartElement("Type");
                writer.WriteString(modbus[i].Type);
                writer.WriteEndElement();
                writer.WriteStartElement("Chanel");
                writer.WriteString(modbus[i].Chanel);
                writer.WriteEndElement();
                writer.WriteStartElement("StartAddress");
                writer.WriteString(modbus[i].StartAddress);
                writer.WriteEndElement();
                writer.WriteStartElement("NumberMVK");
                writer.WriteString(modbus[i].NumberMVK);
                writer.WriteEndElement();
                writer.WriteStartElement("ModeWork");
                writer.WriteString(modbus[i].ModeWork);
                writer.WriteEndElement();
                writer.WriteStartElement("Equipment");
                writer.WriteString(modbus[i].Equipment);
                writer.WriteEndElement();
                writer.WriteStartElement("EquipmentName");
                writer.WriteString(modbus[i].EquipmentName);
                writer.WriteEndElement();
                writer.WriteStartElement("DreamChannelName");
                writer.WriteString(modbus[i].DreamChannelName);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            logger.WtiteLog("Данные успешно записаны в XML");
            modbus.Clear();
        }


        /// <summary>
        /// Метод сохранения настроек подключения к БД в файл "DatabaseSettings.xml"
        /// </summary>
        /// <param name="dbClient">Экземпляр данных клиента подключения</param>
        /// <param name="status">Статус БД "Active / NoActive"</param>
        internal void SaveDatabaseSettings(ref DBClient dbClient, StatusDatabase status = StatusDatabase.NoActive )
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);
            var root = xml.DocumentElement;

            XmlElement version = xml.CreateElement("Version");
            XmlAttribute name = xml.CreateAttribute("name");

            XmlElement activeDB = xml.CreateElement("ActiveDatabase");
            XmlElement server = xml.CreateElement("Server");
            XmlElement database = xml.CreateElement("Database");
            XmlElement user = xml.CreateElement("User");
            XmlElement password = xml.CreateElement("Password");

            XmlText nameText = xml.CreateTextNode(dbClient.Version);
            XmlText activeDbText = xml.CreateTextNode(status == StatusDatabase.Active ? "active" : "");
            XmlText serverText = xml.CreateTextNode(dbClient.Host_Server);
            XmlText databaseText = xml.CreateTextNode(dbClient.Database);
            XmlText userText = xml.CreateTextNode(dbClient.Login);
            XmlText passwordText = xml.CreateTextNode(dbClient.Password);

            name.AppendChild(nameText);
            activeDB.AppendChild(activeDbText);
            server.AppendChild(serverText);
            database.AppendChild(databaseText);
            user.AppendChild(userText);
            password.AppendChild(passwordText);

            version.Attributes.Append(name);

            version.AppendChild(activeDB);
            version.AppendChild(server);
            version.AppendChild(database);
            version.AppendChild(user);
            version.AppendChild(password);

            root.AppendChild(version);

            if (root != null && status == StatusDatabase.Active)
            {
                foreach (XmlElement elem in root)
                {
                    if (dbClient.Database != elem.ChildNodes[2].InnerText)
                    {
                        elem.ChildNodes[0].InnerText = "";
                    }
                }
            }
                
            xml.Save(pathDatabaseSettings);

            logger.WtiteLog("Данные успешно записаны в XML");
        }

        internal void EditDatabaseSettings(ref DBClient dbClient, StatusDatabase status = StatusDatabase.NoActive)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);
            var root = xml.DocumentElement;

            if (root != null)
            {
                foreach (XmlElement elem in root)
                {
                    if (dbClient.Version == elem.GetAttribute("name")  && dbClient.Database == elem.ChildNodes[2].InnerText)
                    {
                        elem.ChildNodes[0].InnerText = status == StatusDatabase.Active ? "active" : "";
                        elem.ChildNodes[1].InnerText = dbClient.Host_Server;
                        elem.ChildNodes[2].InnerText = dbClient.Database;
                        elem.ChildNodes[3].InnerText = dbClient.Login;
                        elem.ChildNodes[4].InnerText = dbClient.Password;
                    }
                    else if(dbClient.Database != elem.ChildNodes[2].InnerText && status == StatusDatabase.Active)
                    {
                        elem.ChildNodes[0].InnerText = "";
                    }
                }
            }

            xml.Save(pathDatabaseSettings);

            logger.WtiteLog("Данные успешно изменены в XML");
        }



        /// <summary>
        /// Метод изменения активной БД в файле "DatabaseSettings.xml"
        /// </summary>
        /// <param name="database">Название БД</param>
        /// <param name="status">Статус БД "Active / NoActive"</param>
        internal void SetActiveDatabase(in string database, in StatusDatabase status)
        {
            if (!File.Exists(pathDatabaseSettings))
            {
                logger.WtiteLog($"Нет файла настроек БД \"DatabaseSettings.xml\"!");
                return;
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);

            var root = xml.DocumentElement;
            
            foreach (XmlElement elem in root)
            {
                if (database == elem.GetAttribute("name"))
                    elem.ChildNodes[0].InnerText = status == StatusDatabase.Active ? "active" : " ";
                else
                    elem.ChildNodes[0].InnerText = status == StatusDatabase.Active ? "" : "active";
            }
            xml.Save(pathDatabaseSettings);
            logger.WtiteLog($"Успешно изменена активная база данных. Текущая база данных: {database}");
            
        }

        /// <summary>
        /// Метод возвращает настройки клиента для подключения к БД
        /// </summary>
        /// <returns>Клиент "DBClient"</returns>
        internal List<DBClient> GetDataForConnect()
        {
            GetActiveSettingsOnDB();
            return database;
        }

        /// <summary>
        /// Метод возвращает настройки клиента для подключения к БД по наименованию БД
        /// </summary>
        /// <returns>Клиент "DBClient"</returns>
        internal List<DBClient> GetDataForConnectName(string nameSubd, string nameDB)
        {
            GetSettingsOnDB(nameSubd, nameDB);
            return database;
        }

        /// <summary>
        /// Метод устанавливает клиента для подключения к БД
        /// </summary>
        /// <param name="dbClient">Принимает клиента "DBClient"</param>
        internal void SetDataForDisconnect(DBClient dbClient)  
        {         
            database.Clear(); 
            database.AddRange(new List<DBClient>() { dbClient});
        }

        /// <summary>
        /// Метод получения настроек оборудования из вайла "MVKSettings.xml"
        /// </summary>
        /// <param name="list"></param>
        /// <param name="modbus"></param>
        internal void GetAllDevice(out List<string> list, out List<ModbusClient> modbus)
        {
            list = new List<string>();
            modbus = new List<ModbusClient>();

            if (File.Exists(pathMVKSettings))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(pathMVKSettings);
                var root = xml.DocumentElement;
                if (root != null)
                {
                    foreach (XmlElement elem in root)
                    {
                        list.Add($"Номер устройства: {elem.GetAttribute("Index")}{Environment.NewLine}IP адрес: {elem.ChildNodes[0].InnerText}{Environment.NewLine}Endians: {elem.ChildNodes[1].InnerText}" +
                            $"{Environment.NewLine}Полоса: {elem.ChildNodes[2].InnerText}{Environment.NewLine}Тип: {elem.ChildNodes[3].InnerText}{Environment.NewLine}Канал: {elem.ChildNodes[4].InnerText}" +
                            $"{Environment.NewLine}Номер МВК: {elem.ChildNodes[6].InnerText}{Environment.NewLine}Номер оборудования: {elem.ChildNodes[8].InnerText}" +
                            $"{Environment.NewLine}Название оборудования: {elem.ChildNodes[9].InnerText}{Environment.NewLine}Название в БД DREAM: {elem.ChildNodes[10].InnerText}");

                        modbus.Add(new ModbusClient(
                            elem.ChildNodes[0].InnerText, 
                            elem.ChildNodes[1].InnerText, 
                            elem.ChildNodes[2].InnerText, 
                            elem.ChildNodes[3].InnerText, 
                            elem.ChildNodes[4].InnerText,
                            elem.ChildNodes[6].InnerText,
                            elem.ChildNodes[8].InnerText,
                            elem.ChildNodes[9].InnerText,
                            elem.ChildNodes[10].InnerText));
                    }
                }
            }
            else
                list.Add("НЕТ ПАРАМЕТРОВ");
        }

        internal List<ModbusClient> GetAllDevice()
        {
            List<ModbusClient> modbus = new List<ModbusClient>();

            if (File.Exists(pathMVKSettings))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(pathMVKSettings);
                var root = xml.DocumentElement;
                if (root != null)
                {
                    foreach (XmlElement elem in root)
                    {
                        modbus.Add(new ModbusClient(
                            elem.ChildNodes[0].InnerText,
                            elem.ChildNodes[1].InnerText,
                            elem.ChildNodes[2].InnerText,
                            elem.ChildNodes[3].InnerText,
                            elem.ChildNodes[4].InnerText,
                            elem.ChildNodes[6].InnerText,
                            elem.ChildNodes[8].InnerText,
                            elem.ChildNodes[9].InnerText,
                            elem.ChildNodes[10].InnerText));
                    }
                }
            }

            return modbus;
        }

        /// <summary>
        /// Метод открывает файл справки "Readme.docx"
        /// </summary>
        internal void OpenWordFile()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd",
                Arguments = @$"/c ""{Directory.GetCurrentDirectory()}\Readme.docx""",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Метод удаляет старый файл настроек оборудования "MVKSettings.xml"
        /// </summary>
        private void RemoveOldXML()
        {
            if (File.Exists(pathMVKSettings) == true)
                File.Delete(pathMVKSettings);
            else
                return;
        }

        //internal List<Average> GetAverage()
        //{
        //    List<Average> list = new List<Average>();

        //    path = dirSettings + "TimeSaveArchive.xml";
        //    if (File.Exists(path))
        //    {
        //        XmlDocument xml = new XmlDocument();
        //        xml.Load(path);
        //        var root = xml.DocumentElement;
        //        if (root != null)
        //        {
        //            foreach (XmlElement elem in root)
        //                list.Add(new Average(
        //                    elem.ChildNodes[0].InnerText, 
        //                    int.Parse(elem.ChildNodes[1].InnerText), 
        //                    elem.ChildNodes[2].InnerText, 
        //                    int.Parse(elem.ChildNodes[3].InnerText),
        //                    elem.ChildNodes[4].InnerText));
        //        }
        //    }
        //    return list;
        //}

        /// <summary>
        /// Метод возвращает список объектов таблиц с настройками по названию БД из файла "TimeSaveArchive.xml"
        /// </summary>
        /// <param name="nameDb">Наименование БД</param>
        /// <returns>Список объектов таблиц с настройками</returns>
        internal List<Average> GetAverage(string nameDb)
        {
            List<Average> list = new List<Average>();

            path = $@"{dirSettings}{nameDb}\TimeSaveArchive.xml";

            if (File.Exists(path))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                var root = xml.DocumentElement;
                if (root != null)
                {
                    foreach (XmlElement elem in root)
                        list.Add(new Average(
                            elem.ChildNodes[0].InnerText,
                            int.Parse(elem.ChildNodes[1].InnerText),
                            elem.ChildNodes[2].InnerText,
                            int.Parse(elem.ChildNodes[3].InnerText),
                            elem.ChildNodes[4].InnerText));
                }
            }
            return list;
        }

        /// <summary>
        /// Метод сохраняет настройки таблиц БД в файл "TimeSaveArchive.xml"
        /// </summary>
        /// <param name="list">Список настроек обектов таблиц БД</param>
        /// <param name="nameDb">Наименование БД</param>
        internal void SetAverage(ref List<Average> list, string nameDb)
        {
            path = $@"{dirSettings}{nameDb}\TimeSaveArchive.xml";
            int level = 1;

            if (File.Exists(path) == true)
                File.Delete(path);

            XmlTextWriter writer = new XmlTextWriter(path, null);

            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;
            writer.WriteStartDocument();
            writer.WriteStartElement("SettingsTimeSave");

            for (int i = 0; i < list.Count; i++)
            {
                if(i != 0)
                    level = int.Parse(list[i].Name.Substring(list[i].Name.Length - 1)) + 1;

                writer.WriteStartElement($"Level");            
                writer.WriteStartAttribute("Index");                
                writer.WriteString($"{level}");                     
                writer.WriteEndAttribute();                        
                writer.WriteStartElement("NameLevel");              
                writer.WriteString(list[i].Name);                  
                writer.WriteEndElement();                           
                writer.WriteStartElement("Time");           
                writer.WriteString(list[i].Value.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("TimeMesuament");
                writer.WriteString(list[i].MesuamentUnit);
                writer.WriteEndElement();
                writer.WriteStartElement("TimeValue");
                writer.WriteString(list[i].TimeValue.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("TimeAverage");
                writer.WriteString(list[i].TimeAverage);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            logger.WtiteLog("Данные успешно записаны в XML");
        }

        /// <summary>
        /// Метод сохранияет настройки подключения к серверу
        /// </summary>
        /// <param name="serverSettings">Объект настроек подключения</param>
        /// <returns>Возращает True файл записан в файл</returns>
        internal void ServerSettingsSave(ServerSettings serverSettings, string nameDb)
        {
            path = $@"{dirSettings}{nameDb}\ServerSettings.xml";

            if (File.Exists(path) == true)
                File.Delete(path);

            XmlTextWriter writer = new XmlTextWriter(path, null);

            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;
            writer.WriteStartDocument();
            writer.WriteStartElement("ServerSettings");

            writer.WriteStartElement("IP");
            writer.WriteString(serverSettings.IP);
            writer.WriteEndElement();
            writer.WriteStartElement("Port");
            writer.WriteString(serverSettings.Port);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.Close();
        }

        /// <summary>
        /// Метод считывает настройки подключения к серверу
        /// </summary>
        /// <returns>Возвращает оюъект нстроек подключения к серверу<returns>
        internal ServerSettings? ServerSettingsLoad(string nameDb)
        {
            path = $@"{dirSettings}{nameDb}\ServerSettings.xml";

            if (File.Exists(path))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                var root = xml.DocumentElement;

                if (root != null)
                {
                    return new ServerSettings(root.ChildNodes[0].InnerText, root.ChildNodes[1].InnerText);
                }
            }

            return null;
        }

        /// <summary>
        /// Метод сохраняет настройки коэффициента пиковых значений "PeakValue" в файл "PeakValue.xml"
        /// </summary>
        /// <param name="peak">Объект настроек коэффициента</param>
        /// <param name="nameDb">Наименование БД</param>
        internal void SetPeakValue(ref Peak peak, string nameDb)
        {
            path = $@"{dirSettings}{nameDb}\PeakValue.xml";

            if (File.Exists(path) == true)
                File.Delete(path);

            XmlTextWriter writer = new XmlTextWriter(path, null);

            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;
            writer.WriteStartDocument();
            writer.WriteStartElement("SettingsPeake");

            writer.WriteStartElement("NameAverage");
            writer.WriteString(peak.Name);
            writer.WriteEndElement();
            writer.WriteStartElement("CoefficientPeak");
            writer.WriteString(peak.CoefficientPeak.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.Close();
        }

        internal Peak? GetPeakValue(string nameDb)
        {
            path = $@"{dirSettings}{nameDb}\PeakValue.xml";

            if (File.Exists(path))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                var root = xml.DocumentElement;

                if (root != null)
                {
                    return new Peak(root.ChildNodes[0].InnerText, float.Parse(root.ChildNodes[1].InnerText));                    
                }
            }

            return null;
        }

        /// <summary>
        /// Метод сохраняет настройки для таблицы пиковых значений "PeakValue" в файл "TimeSavePeakValue.xml"
        /// </summary>
        /// <param name="peakValueStorage">Экземпляр настроек пиковых значений</param>
        /// <param name="nameDb">Наименование БД</param>
        public void SetPeakValueStorages(PeakValueStorage peakValueStorage, string nameDb)
        {
            path = $@"{dirSettings}{nameDb}\TimeSavePeakValue.xml";

            if (File.Exists(path) == true)
                File.Delete(path);

            XmlTextWriter writer = new XmlTextWriter(path, null);

            writer.Formatting = Formatting.Indented;
            writer.IndentChar = '\t';
            writer.Indentation = 1;
            writer.WriteStartDocument();
            writer.WriteStartElement("SettingsTimeSavePeak");

            writer.WriteStartElement("NameTable");
            writer.WriteString(peakValueStorage.NameTable);
            writer.WriteEndElement();
            writer.WriteStartElement("Time");
            writer.WriteString(peakValueStorage.Time.ToString());
            writer.WriteEndElement();
            writer.WriteStartElement("TimeMesuament");
            writer.WriteString(peakValueStorage.TimeMesuament);
            writer.WriteEndElement();
            
            writer.WriteEndElement();
            writer.Close();
        }

        /// <summary>
        /// Метод возвращает список объектов таблиц пиковых значений из файла "TimeSavePeakValue.xml"
        /// </summary>
        /// <param name="nameDb">Наименование БД</param>
        /// <returns>Возвращает список объектов таблицы пиковых значений</returns>
        internal List<PeakValueStorage> GetPeakValueStorages(string nameDb)
        {
            List<PeakValueStorage> list = new List<PeakValueStorage>();

            path = $@"{dirSettings}{nameDb}\TimeSavePeakValue.xml";

            if (File.Exists(path))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(path);
                var root = xml.DocumentElement;

                if (root != null)
                {
                    list.Add(new PeakValueStorage(
                            root.ChildNodes[0].InnerText,
                            int.Parse(root.ChildNodes[1].InnerText),
                            root.ChildNodes[2].InnerText));
                }
            }

            return list;
        }


        internal string GetActiveDB()
        {
            path = pathDatabaseSettings;
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            var root = xml.DocumentElement;
            {
                foreach (XmlElement elem in root)
                {
                    if (elem.ChildNodes[0].InnerText == "active")
                    {
                        return elem.GetAttribute("name");
                    }    
                    else
                        continue;
                }
            }
            return "";
        }

        internal string GetActiveDBName(in string database)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);

            var root = xml.DocumentElement;

            foreach (XmlElement elem in root)
            {
                if (database == elem.GetAttribute("name") && elem.ChildNodes[0].InnerText == "active")
                    return elem.ChildNodes[2].InnerText;
                else
                    continue;
            }

            return "";
        }

        private void GetActiveSettingsOnDB()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);
            var root = xml.DocumentElement;
            database.Clear();
            if (root != null)
            {
                foreach (XmlElement elem in root)
                {
                    if (elem.ChildNodes[0].InnerText == "active")
                        database.Add(new DBClient(elem.GetAttribute("name"), elem.ChildNodes[1].InnerText, elem.ChildNodes[2].InnerText, elem.ChildNodes[3].InnerText, elem.ChildNodes[4].InnerText));
                    else
                        continue;
                }
            }
        }

        /// <summary>
        /// Метод считывает все настройки подключений к БД из файла "DatabaseSettings.xml"
        /// </summary>
        /// <returns>Возвращает список всех настроек из файла "DatabaseSettings.xml"</returns>
        internal void GetSettingsOnDB(string nameSubd, in List<DBClientFull> dbList)
        {            
            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);
            var root = xml.DocumentElement;

            if (root != null)
            {              
                foreach (XmlElement elem in root)
                {
                    if (nameSubd == elem.GetAttribute("name"))
                    {
                        dbList.Add(new DBClientFull(elem.GetAttribute("name"),
                        elem.ChildNodes[1].InnerText,
                        elem.ChildNodes[2].InnerText,
                        elem.ChildNodes[3].InnerText,
                        elem.ChildNodes[4].InnerText,
                        elem.ChildNodes[0].InnerText == "active" ? StatusDatabase.Active : StatusDatabase.NoActive));
                    }                 
                }
            }
        }


        /// <summary>
        /// Метод считывает настройки подключений к БД из файла "DatabaseSettings.xml"
        /// </summary>
        /// <param name="nameSubd">Наименование СУБД</param>
        /// <param name="nameDb">Наименование БД</param>
        internal void GetSettingsOnDB(string nameSubd, string nameDb)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);
            var root = xml.DocumentElement;

            database.Clear();

            if (root != null)
            {
                foreach (XmlElement elem in root)
                {
                    if (nameSubd == elem.GetAttribute("name") && nameDb == elem.ChildNodes[2].InnerText)
                    {
                        database.Add(new DBClient(elem.GetAttribute("name"),
                        elem.ChildNodes[1].InnerText,
                        elem.ChildNodes[2].InnerText,
                        elem.ChildNodes[3].InnerText,
                        elem.ChildNodes[4].InnerText));
                    }
                    else
                    {
                        continue;
                    }
                        
                }
            }
        }

        /// <summary>
        /// Метод проверяет наличие сохраненных настроек подключения к БД по наименованию БД в файле "DatabaseSettings.xml"
        /// </summary>
        /// <param name="nameSubd">Наименование СУБД</param>
        /// <param name="nameDb">Наименование БД</param>
        /// <returns>Возвращает True - настройки отсутствуют, False - настройки уже есть</returns>
        internal bool CheckNameDb(string nameSubd, string nameDb)
        {
            if (!File.Exists(pathDatabaseSettings))
            {
                logger.WtiteLog($"Нет файла настроек БД \"DatabaseSettings.xml\"!");
                return false;
            }

            XmlDocument xml = new XmlDocument();
            xml.Load(pathDatabaseSettings);
            var root = xml.DocumentElement;

            if (root != null)
            {
                foreach (XmlElement elem in root)
                {
                    if (nameSubd == elem.GetAttribute("name") && nameDb == elem.ChildNodes[2].InnerText)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Метод создает директорию с названием БД для хранения настроек
        /// </summary>
        /// <param name="nameDb">Название БД</param>
        internal void CereateDirectorySetting(string nameDb)
        {
            string pathTemp = $@"{dirSettings}\{nameDb}";

            if (!Directory.Exists(pathTemp))
            {
                Directory.CreateDirectory(pathTemp);
            }
        }
    }
}
