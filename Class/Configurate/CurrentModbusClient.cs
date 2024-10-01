using ConfigurateService.Class.Patterns;
using System.Collections.Generic;

namespace ConfigurateService.Class.Configurate
{
    static class CurrentModbusClient
    {
        internal static List<ModbusClient>? Current { get; private set; }
        internal static void SetCurrentList(in List<ModbusClient> current) => Current=current;
    }
}
