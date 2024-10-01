namespace ConfigurateService.Class.Patterns
{
    internal class Average
    {
        /// <summary>
        /// Время хранения архива
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Наименование времени "час, день, год"
        /// </summary>
        public string MesuamentUnit { get; set; }

                /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Время прореживания
        /// </summary>
        public int TimeValue {  get; set; }

        /// <summary>
        /// Наименование времени "минут, час, день, год"
        /// </summary>
        public string TimeAverage {  get; set; }

        public Average(string name, int value, string mesuamentUnit, int timeValue, string timeAverage)
        {
            Name = name;
            Value = value;
            MesuamentUnit = mesuamentUnit;
            TimeValue = timeValue;
            TimeAverage = timeAverage;
        }
    }
}
