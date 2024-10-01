using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Interface;
using ConfigurateService.Class.Log;
using ConfigurateService.Class.Patterns;
using ConfigurateService.Class.Configurate;
using System.Xml.Linq;
using ConfigurateService.Class.Constants;

namespace ConfigurateService.Class.Database.MSSQL
{
     class MSSQL:IDatabase
    {
        private string query="";
        private Logging logging=new Logging();
        private RepositoryMSSQL mssql = new RepositoryMSSQL();

        /// <summary>
        /// Метод проверяет подключение к БД
        /// </summary>
        /// <returns>Возвращает True - если подключение прошло успешно, False - если подключение прошло не удачно</returns>
        public bool CheckConnection()
        {
            query = "select count(login_name) from sys.dm_exec_sessions where status = 'running'";
            try
            {
                SqlConnection sql = new SqlConnection(mssql.GetConnectionString());
                sql.Open();
                SqlCommand cmd = new SqlCommand(query, sql);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return true;
                sql.Close();
            }
            catch (Exception ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
                return false;
            }
            return false;
        }

        /// <summary>
        /// Метод проверяет подключение к БД
        /// </summary>
        /// <param name="dBClient">Объект подключения к БД</param>
        /// <returns>Возвращает True - если подключение прошло успешно, False - если подключение прошло не удачно</returns>
        public bool CheckConnectionTest(DBClient dBClient)
        {
            query = "select count(login_name) from sys.dm_exec_sessions where status = 'running'";
            try
            {
                SqlConnection sql = new SqlConnection(mssql.GetConnectionStringTest(dBClient));
                sql.Open();
                SqlCommand cmd = new SqlCommand(query, sql);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                    return true;
                sql.Close();
            }
            catch (Exception ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
                return false;
            }
            return false;
        }

        /// <summary>
        /// Метод выводит перечень баз данных MSSQL
        /// </summary>
        /// <param name="database">Список баз данных MSSQL</param>
        public void GetCurrentDatabases(out List<string> database)
        {
            database=new List<string>();
            //query = "select name from sys.databases where owner_sid<>0x01";
            query = "select name from sys.databases where name not in ('master', 'tempdb', 'model', 'msdb')";
            try
            {
                SqlConnection sql = new SqlConnection(mssql.DefaultConnectionString());
                sql.Open();
                SqlCommand cmd = new SqlCommand(query,sql);
                SqlDataReader reader= cmd.ExecuteReader();
                if(reader.HasRows) 
                {
                    while(reader.Read())
                    {
                        database.Add(reader[0].ToString());
                    }
                }
                sql.Close();
            }
            catch (Exception ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
            }
        }

        //public bool CheckOnComponents()
        //{
        //    int i = 0;
        //    string[] array = new string[3];
        //    query = "select name from sys.objects where type in ('P','FN') and left(name,2)<> 'sp' and left(name,2)<>'fn' order by name desc";
        //    try
        //    {
        //        SqlConnection sql = new SqlConnection(mssql.GetConnectionString());
        //        sql.Open();
        //        SqlCommand cmd = new SqlCommand(query, sql);
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                array[i] = reader[0].ToString();
        //                i++;
        //            }
        //        }
        //        sql.Close();
        //        return CheckOnProgramCode(ref array);
        //    }
        //    catch (Exception ex)
        //    {
        //        logging.WtiteLog(ex.Message, StatusLog.ERRORS);
        //    }
        //    return false;
        //}

        /// <summary>
        /// Метод установки компонентов в БД
        /// </summary>
        /// <param name="query">Строка запроса</param>
        public void InstallDBComponets(in string query)
        {
            try
            {
                SqlConnection sql = new SqlConnection(mssql.GetConnectionString());
                sql.Open();
                SqlCommand cmd = new SqlCommand(query, sql);
                cmd.ExecuteNonQuery();
                sql.Close();
            }
            catch (SqlException ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
            }
        }

        /// <summary>
        /// Метод установки начальных компонентов в БД
        /// </summary>
        public void InstallDBComponets()
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            List<DBClient> dBClients = configurationManager.GetDataForConnect();

            InstallDBComponets(CommandMSSQL.CreateTable(dBClients[0].Database));
        }

        /// <summary>
        /// Метод создания новой БД
        /// </summary>
        /// <param name="query">Строка запроса</param>
        public void CreateDB(in string nameDB)
        {
            try
            {
                SqlConnection sql = new SqlConnection(mssql.DefaultConnectionString());
                sql.Open();
                SqlCommand cmd = new SqlCommand(CommandMSSQL.CreateDatabase(nameDB), sql);
                cmd.ExecuteNonQuery();
                sql.Close();
            }
            catch (SqlException ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
            }
        }

        /// <summary>
        /// Метод создает в БД таблицу уровня хранения "ArchiveLevel" уровень 1 и 2
        /// </summary>
        /// <param name="average">Экземпляр настроек таблицы хранения</param>
        public void InstallDBAverage(Average average)
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            List<DBClient> dBClients = configurationManager.GetDataForConnect();

            int level = int.Parse(average.Name.Substring(average.Name.Length - 1));

            InstallDBComponets(CommandMSSQL.CreateTableArchiveLevel(dBClients[0].Database, level));
        }

        /// <summary>
        /// Метод создает в БД таблицу уровня хранения "ArchiveLevel" уровень 3 и тд
        /// </summary>
        /// <param name="average">Экземпляр настроек таблицы хранения</param>
        public void InstallDBAverageNext(Average average)
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            List<DBClient> dBClients = configurationManager.GetDataForConnect();

            int level = int.Parse(average.Name.Substring(average.Name.Length - 1));

            InstallDBComponets(CommandMSSQL.CreateTableArchiveLevelNext(dBClients[0].Database, level));
        }

        /// <summary>
        /// Метод создает в БД таблицу уровня хранения "ArchiveLevel" уровень 3 и тд
        /// </summary>
        /// <param name="average">Экземпляр настроек таблицы хранения</param>
        public void InstallDBArchive()
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            List<DBClient> dBClients = configurationManager.GetDataForConnect();

            InstallDBComponets(CommandMSSQL.CreateTableArchive(dBClients[0].Database));
        }

        /// <summary>
        /// Метод устанавливает в БД таблицу хранеия пиковых значений
        /// </summary>
        public void InstallDBPeakValue()
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            List<DBClient> dBClients = configurationManager.GetDataForConnect();

            InstallDBComponets(CommandMSSQL.CreateTablePeakValues(dBClients[0].Database));
        }

        //private bool CheckOnProgramCode(ref string [] array)
        //{
        //    return (array[0]== "GetParametersID" && array[1]== "GetFrequencyID" && array[2]== "AddValue") ? true : false;
        //}


        /// <summary>
        /// Метод получает из БД перечень таблиц уровней хранения
        /// </summary>
        /// <returns>Массив таблиц уровней хранения из БД</returns>
        public string[] GetCurrentAverageLevel()
        {
            string[] arrayAverage = Array.Empty<string>();

            string queryAverage = "select name from sys.objects where name like 'ArchiveLevel%'";

            using (IDbConnection connection = new SqlConnection(mssql.GetConnectionString()))
            {
                try
                {
                    arrayAverage = connection.Query<string>(queryAverage).Count() > 0 ? connection.Query<string>(queryAverage).ToArray() : new string[] { "нет уровней прореживания" };              
                }
                catch (SqlException ex)
                {
                    logging.WtiteLog(ex.Message, StatusLog.ERRORS);
                }
            }

            return arrayAverage;
        }

        /// <summary>
        /// Метод получает из БД перечень названий таблиц
        /// </summary>
        /// <returns>Массив названий таблиц из БД</returns>
        public string[] GetCurrentTableDB(string nameSubd, string nameDB)
        {
            string[] arrayTable = Array.Empty<string>();
            

            for (int i = 0; i < Constant.NameTableArray.Length; i++)
            {
                string queryString = (Constant.NameTableArray[i] == Constant.ARCHIVELEVEL) ? 
                    $@"use [{nameDB}] select name from sys.objects where name like '{Constant.NameTableArray[i]}%'" : 
                    $@"use [{nameDB}] select name from sys.objects where name like '{Constant.NameTableArray[i]}'";

                string[] arrayTemp = Array.Empty<string>();

                using (IDbConnection connection = new SqlConnection(mssql.GetConnectionStringName(nameSubd, nameDB)))
                {
                    try
                    {
                        arrayTemp = connection.Query<string>(queryString).ToArray();
                    }
                    catch (SqlException ex)
                    {
                        logging.WtiteLog(ex.Message, StatusLog.ERRORS);
                    }
                }

                foreach (string str in arrayTemp)
                {
                    arrayTable = arrayTable.Append(str).ToArray();
                }
            }

            if (arrayTable.Length == 0)
            {
                arrayTable = arrayTable.Append("нет уровней прореживания").ToArray();
            }

            return arrayTable;
        }


        public bool CheckOnComponents(DBClient dBClient)
        {
            query = $"select name from sys.databases where name = '{dBClient.Database}'";

            try
            {
                SqlConnection sql = new SqlConnection(mssql.GetConnectionStringTest(dBClient));
                sql.Open();
                SqlCommand cmd = new SqlCommand(query, sql);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                sql.Close();
            }
            catch (Exception ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
            }
            return false;
        }

        /// <summary>
        /// Метод проверяеи наличие БД на сервере по наименовнию
        /// </summary>
        /// <param name="nameSubd">Наименование СУБД</param>
        /// <param name="nameDb">Наименование БД</param>
        /// <returns>Возвращает True - если такая БД существует и False - если такой БД нет</returns>
        public bool CheckOnNameDb(string nameSubd, string nameDb)
        {
            query = $"select name from sys.databases where name = '{nameDb}'";

            try
            {
                SqlConnection sql = new SqlConnection(mssql.GetConnectionStringName(nameSubd, nameDb));
                sql.Open();
                SqlCommand cmd = new SqlCommand(query, sql);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                sql.Close();
            }
            catch (Exception ex)
            {
                logging.WtiteLog(ex.Message, StatusLog.ERRORS);
            }
            return false;
        }

        /// <summary>
        /// Метод вставляет в таблицу Equipment в БД название оборудования 
        /// </summary>
        public void InsertDBnameEquipment(List<ModbusClient> name)
        {
            if (name.Count > 0)
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                List<DBClient> dBClients = configurationManager.GetDataForConnect();

                for (int i = 0; i < name.Count; i++)
                {
                    InstallDBComponets(CommandMSSQL.InsertNameEquipment(dBClients[0].Database, name[i].EquipmentName, int.Parse(name[i].Equipment)));
                }
            }                      
        }


        /// <summary>
        /// Метод записи из таблицы Equipment в БД
        /// </summary>
        public void DeleteDBnameEquipment()
        {
            ConfigurationManager configurationManager = new ConfigurationManager();
            List<DBClient> dBClients = configurationManager.GetDataForConnect();
          
            InstallDBComponets(CommandMSSQL.DeleteNameEquipment(dBClients[0].Database));
        }
    }
}
