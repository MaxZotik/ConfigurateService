using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigurateService.Class.Enums;

namespace ConfigurateService.Class.Patterns
{
    internal class DBClientFull : DBClient
    {
        public StatusDatabase ActiveDatabase {  get; set; }


        internal DBClientFull(string version, string host_server, string database, string login, string password, StatusDatabase activeDatabase) : 
            base(version, host_server, database, login, password)
        {
            ActiveDatabase = activeDatabase;
        }
    }
}
