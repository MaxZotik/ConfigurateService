using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurateService.Class.Constants
{
    internal static class Constant
    {
        public const string ARCHIVE = "Archive";
        public const string ARCHIVELEVEL = "ArchiveLevel";
        public const string PEAKVALUE = "PeakValue";
        public const string MSSQL = "MSSQL";

        /// <summary>
        /// Массив названий таблиц в БД
        /// </summary>
        public static readonly string[] NameTableArray = { ARCHIVE, ARCHIVELEVEL, PEAKVALUE };

        /// <summary>
        /// Массив названий СУБД
        /// </summary>
        public static readonly string[] SUBD = { MSSQL };

        /// <summary>
        /// Массив частот
        /// </summary>
        public static readonly string[] FREQUENCY = { "x-25Гц", "10-3000Гц", "10-2000Гц", "10-1000Гц", "2-1000Гц", "0.8-300Гц", "0.8-150Гц", "Фильтр 1", "Фильтр 2" };

        /// <summary>
        /// Массив последовательности передачи байт "Big Endian"
        /// </summary>
        public static readonly string[] ENDIANS = { "2301", "0123", "3210" };

        /// <summary>
        /// Массив параметров часть один
        /// </summary>
        public static readonly string[] PARAMETERS_ONE = { "Пик-Фактор", "Виброускорение" };

        /// <summary>
        /// Массив параметров часть два
        /// </summary>
        public static readonly string[] PARAMETERS_TWO = { "Виброускорение", "Виброскорость", "Виброперемещение" };

        /// <summary>
        /// Массив временных интервалов
        /// </summary>
        public static readonly string[] ARCHIVETIME = { "минута", "час", "день", "год" };
    }
}
