using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurateService.Class.Patterns
{
    public class PeakValueStorage
    {
        public string NameTable { get; set; }

        public int Time { get; set; }

        public string TimeMesuament { get; set; }
        
        public PeakValueStorage(string nameTable, int time, string timeMesuament)
        {
            NameTable = nameTable;
            Time = time;
            TimeMesuament = timeMesuament;
        }
    }
}
