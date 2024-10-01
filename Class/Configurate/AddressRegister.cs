using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Patterns;
using ConfigurateService.Class.Log;
using System.Collections.Generic;

namespace ConfigurateService.Class.Configurate
{
     class AddressRegister
    {
        private readonly Logging logger = new Logging();
        internal void SetStartAddress(ref List<ModbusClient> device)
        {
            for(int i=0;i<device.Count;i++)
            {
                SetupAddress(ref device,i);
            }
        }
        private void SetupAddress(ref List<ModbusClient> device, int index)
        {
            switch (device[index].Parameters)
            {
                case "25Гц":
                    device[index].StartAddress = device[index].Type == "Пик-Фактор" ? 
                        ((int)Register.PickFactor_25Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString() : 
                        ((int)Register.VibroAcceleration_25Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "10-3000Гц":
                    if (device[index].Type=="Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_10_3000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if(device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_10_3000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_10_3000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "10-2000Гц":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_10_2000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_10_2000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_10_2000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "10-1000Гц":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_10_1000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_10_1000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_10_1000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "2-1000Гц":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_2_1000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_2_1000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_2_1000Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "0.8-300Гц":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_300Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_300Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_300Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "0.8-150Гц":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_150Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_150Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_150Hz + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "Фильтр 1":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_Filter1 + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_Filter1 + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_Filter1 + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                case "Фильтр 2":
                    if (device[index].Type == "Виброускорение")
                        device[index].StartAddress = ((int)Register.VibroAcceleration_Filter2 + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else if (device[index].Type == "Виброскорость")
                        device[index].StartAddress = ((int)Register.VibroSpeed_Filter2 + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    else
                        device[index].StartAddress = ((int)Register.VibroMoving_Filter2 + ((512 * (int.Parse(device[index].Chanel) - 1)))).ToString();
                    break;
                default:
                    logger.WtiteLog("Неизвестная опция в полосе", StatusLog.ERRORS);
                    break;
            }
        }
    }
}
