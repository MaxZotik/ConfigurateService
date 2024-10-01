using ConfigurateService.Class.Interface;
using ConfigurateService.Class.Patterns;
using ConfigurateService.Class.Configurate;
using System.Collections.Generic;
using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Log;

namespace ConfigurateService.Class.Database.MSSQL
{
    class RepositoryMSSQL : IRepository
    {
        private string connection = "";
        private List<DBClient> database = new List<DBClient>();
        ConfigurationManager manager = new ConfigurationManager();

        /// <summary>
        /// Метод создает строку подключения к БД
        /// </summary>
        private void ConnectionString()
        {
            database.Clear();
            database = manager.GetDataForConnect();
            connection = $"Data Source={database[0].Host_Server}; Initial Catalog={database[0].Database}; User ID = {database[0].Login}; Password ={database[0].Password}";
        }

        /// <summary>
        /// Метод создает строку подключения к БД
        /// </summary>
        /// <param name="dBClient">Объект подключения к БД</param>
        private void ConnectionStringTest(DBClient dBClient)
        {
            connection = $"Data Source={dBClient.Host_Server}; Initial Catalog={dBClient.Database}; User ID = {dBClient.Login}; Password ={dBClient.Password}";
        }

        /// <summary>
        /// Метод создает строку подключения к БД по наименованию БД
        /// </summary>
        private void ConnectionStringName(string nameSubd, string nameDB)
        {
            database.Clear();
            database = manager.GetDataForConnectName(nameSubd, nameDB);
            connection = $"Data Source={database[0].Host_Server}; Initial Catalog={database[0].Database}; User ID = {database[0].Login}; Password ={database[0].Password}";
        }

        /// <summary>
        /// Метод возвозвращает строку подключения к БД по умолчанию
        /// </summary>
        /// <returns>Строка подключения к БД</returns>
        public string DefaultConnectionString()
        {
            return @"Data Source=.\Dream; Initial Catalog=master;Integrated Security=true";
        }

        /// <summary>
        /// Метод возвозвращает строку подключения к БД
        /// </summary>
        /// <returns>Строка подключения к БД</returns>
        public string GetConnectionString()
        {
            ConnectionString();
            return connection;
        }

        /// <summary>
        /// Метод возвозвращает строку подключения к БД по наименованию БД
        /// </summary>
        /// <param name="nameSubd">Наименование СУБД</param>
        /// <param name="nameDB">Наименование БД</param>
        /// <returns>Строка подключения к БД</returns>
        public string GetConnectionStringName(string nameSubd, string nameDB)
        {
            ConnectionStringName(nameSubd, nameDB);
            return connection;
        }


        /// <summary>
        /// Метод возвозвращает строку подключения к БД
        /// </summary>
        /// <param name="dBClient">Объект подключения к БД</param>
        /// <returns></returns>
        public string GetConnectionStringTest(DBClient dBClient)
        {
            ConnectionStringTest(dBClient);
            return connection;
        }
    }
}
