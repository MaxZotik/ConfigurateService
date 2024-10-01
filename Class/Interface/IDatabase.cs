using ConfigurateService.Class.Patterns;
using System.Collections.Generic;

namespace ConfigurateService.Class.Interface
{
    interface IDatabase
    {
        void GetCurrentDatabases(out List<string> database);
        bool CheckConnection();
        bool CheckOnComponents(DBClient dBClient);
        void InstallDBComponets(in string query);
    }
}
