using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Interface;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;

namespace ConfigurateService.Class.Database.MSSQL
{
    /// <summary>
    /// Служебный класс хранения SQL запросов MSSQL
    /// </summary>
    static class CommandMSSQL
    {
        /// <summary>
        /// Метод хранит строку MSSQL создания базы данных
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <returns>Строка создания создания базы данных</returns>
        public static string CreateDatabase(string nameDatabase)
        {
            return $"CREATE DATABASE {nameDatabase} COLLATE Cyrillic_General_CI_AS";
        }

        /// <summary>
        /// Метод хранит строку MSSQL создания таблиц "Archive", "Parameters", "Frequency"
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <returns>Строка создания создания таблиц "Archive", "Parameters", "Frequency"</returns>
        public static string CreateTable(string nameDatabase)
        {
            return $"USE [{nameDatabase}]" +
                    "create table[dbo].[Frequency]([ID] int identity primary key, [Name] varchar(30))" +
                    "create table[dbo].[Parameters] ([ID] int identity primary key, [Name] varchar(30))" +
                    "create table[dbo].[Equipment] ([ID] int identity primary key, [Number] int, [Name] varchar(30))" +
                    "create table[dbo].[Mode] ([ID] int identity primary key, [number] int, [Name] varchar(30))" +                   
                    "insert[dbo].[Frequency]([Name]) values(N'10-2000гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'10-1000гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'2-1000гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'x-25гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'10-3000гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'0.8-300гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'0.8-150гц')" +
                    "insert[dbo].[Frequency]([Name]) values(N'Фильтр 1')" +
                    "insert[dbo].[Frequency]([Name]) values(N'Фильтр 2')" +
                    "insert[dbo].[Parameters]([Name]) values(N'Пик-Фактор')" +
                    "insert[dbo].[Parameters]([Name]) values(N'СКЗ виброускорение')" +
                    "insert[dbo].[Parameters]([Name]) values(N'СКЗ виброскорость')" +
                    "insert[dbo].[Parameters]([Name]) values(N'СКЗ виброперемещение')" +
                    "insert[dbo].[Mode]([Number], [Name]) values(0, N'Нет данных')" +
                    "insert[dbo].[Mode]([Number], [Name]) values(1, N'Вне режима')" +
                    "insert[dbo].[Mode]([Number], [Name]) values(2, N'Прокат')" +
                    "insert[dbo].[Mode]([Number], [Name]) values(3, N'Холостой ход')" +
                    "insert[dbo].[Mode]([Number], [Name]) values(4, N'Стоп')";
        }


        /// <summary>
        /// Метод хранит строку MSSQL создания таблицы "Archive"
        /// </summary>
        /// <param name="nameDatabase"></param>
        /// <returns>Строка создания создания таблицы "Archive"</returns>
        public static string CreateTableArchive(string nameDatabase)
        {
            return $"USE [{nameDatabase}]" +
                    "create table[dbo].[Archive] ([ID] int identity primary key, [ID Parameters] int foreign key references[dbo].[Parameters] ([ID])," +
                        "[ID Frequency] int foreign key references[dbo].[Frequency] ([ID]), [Equipment] int foreign key references[dbo].[Equipment] ([ID]), " +
                        "[Time] datetime, [MVK Value] float, [Chanel] int, [MVK Number] int, " +
                        "[Mode Work] int, [DreamDb channel_name] varchar(30))";
        }

        //Исправлено
        /// <summary>
        /// Метод хранит строку MSSQL создания таблицы для хранения пиковых значений "PeakValue"
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <returns>Строка создания создания таблицы хранения пиковых значений</returns>
        public static string CreateTablePeakValues(string nameDatabase)
        {
            return $"USE [{nameDatabase}]" +
                    "create table[dbo].[PeakValue]([ID] int identity primary key, [ID Parameters] int foreign key references [dbo].[Parameters]([ID]), " +
                    "[ID Frequency] int foreign key references [dbo].[Frequency] ([ID]), [Equipment] int, " +
                    "[Time] datetime, [MVK Value] float, " +
                    "[Chanel] int, [MVK Number] int, [Mode Work] int, [DreamDb channel_name] varchar(30))";
        }      

        /// <summary>
        /// Метод хранит строку MSSQL создания таблицы для усреднения данных "ArchiveLevel" для уровней 1 и 2
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <param name="numLevel">Номер таблицы усреднения данных</param>
        /// <returns>Строка создания создания таблицы усреднения данных</returns>
        public static string CreateTableArchiveLevel(string nameDatabase, int numLevel)
        {
            return $"USE [{nameDatabase}]" +
                    $"create table[dbo].[ArchiveLevel{numLevel}](" +
                    "[ID] int identity primary key, " +
                    "[ID Parameters] int foreign key references [dbo].[Parameters]([ID])," +
                    "[ID Frequency] int foreign key references [dbo].[Frequency] ([ID]), " +
                    "[Equipment] int, " +
                    "[Time] datetime, [MVK Value Max] float, [MVK Value Avg] float, [Chanel] int, " +
                    "[Deviation] float, [Counts] int, [MVK Number] int, [Mode Work] int, [DreamDb channel_name] varchar(30), [Period One] float, [Period Two] float)";
        }


        /// <summary>
        /// Метод хранит строку MSSQL создания таблицы для усреднения данных "ArchiveLevel" для уровней 3 и последующих
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <param name="numLevel">Номер таблицы усреднения данных</param>
        /// <returns>Строка создания создания таблицы усреднения данных</returns>
        public static string CreateTableArchiveLevelNext(string nameDatabase, int numLevel)
        {
            return $"USE [{nameDatabase}]" +
                    $"create table[dbo].[ArchiveLevel{numLevel}](" +
                    "[ID] int identity primary key, " +
                    "[ID Parameters] int foreign key references [dbo].[Parameters]([ID])," +
                    "[ID Frequency] int foreign key references [dbo].[Frequency] ([ID]), " +
                    "[Equipment] int, " +
                    "[Time] datetime, [MVK Value Max] float, [MVK Value Avg] float, [Chanel] int, " +
                    "[Deviation] float, [Counts] int, [MVK Number] int, [Mode Work] int, [DreamDb channel_name] varchar(30))";
        }


        /// <summary>
        /// Метод добавляет запись в таблицу Equipment
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <param name="nameEquipment">Название оборудования</param>
        /// <param name="number">Номер оборудования</param>
        /// <returns>Строка вставки записи в таблицу Equipment</returns>
        public static string InsertNameEquipment(string nameDatabase, string nameEquipment, int number)
        {
            return $"USE [{nameDatabase}]" +
                    $"insert[dbo].[Equipment]([Number],[Name]) values({number}, N'{nameEquipment}')";
        }

        /// <summary>
        /// Метод удаляет все записи из таблицы Equipment
        /// </summary>
        /// <param name="nameDatabase">Название базы данных</param>
        /// <returns>Строка удаления записей из таблицы Equipment</returns>
        public static string DeleteNameEquipment(string nameDatabase)
        {
            return $"USE [{nameDatabase}]" +
                    $"delete from [dbo].[Equipment]";
        }

    }
}
