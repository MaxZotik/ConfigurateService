using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurateService.Class.Patterns
{
    internal class Peak
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Коэфицент пиковых значений
        /// </summary>
        public float CoefficientPeak { get; set; }

        public Peak(string name, float coefficientPeak)
        {
            Name = name;
            CoefficientPeak = coefficientPeak;
        }





        /// <summary>
        /// Коэфицент прореживания
        /// </summary>
        //public float CoefficientThinning { get; set; }


        //public float CoefficientEnd { get; set; }

        //public Peak(string name, float coefficientPeak, float coefficientThinning, float coefficientEnd)
        //{
        //    Name = name;
        //    CoefficientPeak = coefficientPeak;
        //    CoefficientThinning = coefficientThinning;
        //    CoefficientEnd = coefficientEnd;
        //}
    }
}
