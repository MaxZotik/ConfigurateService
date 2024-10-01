namespace ConfigurateService.Class.Patterns
{
    class ModbusClient
    {
        internal string IP { get; private set; }
        internal string Endians { get; private set; }
        internal string Parameters { get; private set; }
        internal string Type { get; private set; }
        internal string Chanel { get; private set; }
        internal string? StartAddress { get; set; }
        internal string NumberMVK { get; set; }
        internal string ModeWork { get; set; }
        internal string Equipment { get; set; }

        internal string EquipmentName {  get; set; }
        internal string DreamChannelName { get; set; }

        internal ModbusClient(string ip, string endians, string parameters, string type, string chanel, string numberMVK, string equipment, string equipmenName, string dreamChannelName)
        {
            IP = ip;
            Endians = endians;
            Parameters = parameters;
            Type = type;
            Chanel = chanel;
            NumberMVK = numberMVK;
            ModeWork = "0";
            Equipment = equipment;
            EquipmentName = equipmenName;
            DreamChannelName = dreamChannelName;
        }
        //internal ModbusClient(string ip, string endians, string parameters, string type, string chanel, string startAdress, string numberMVK, string equipment) : 
        //    this (ip, endians, parameters, type, chanel, numberMVK, equipment)
        //{
        //    IP = ip;
        //    Endians = endians;
        //    Parameters = parameters;
        //    Type = type;
        //    Chanel = chanel;
        //    NumberMVK = numberMVK;
        //    Equipment = equipment;
        //    StartAddress = startAdress;
        //}
    }
}

