using ConfigurateService.Class.Database.MSSQL;
using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Log;
using ConfigurateService.Class.Patterns;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurateService.Class.Configurate
{
    internal static class SearchDevice
    {

        public static void NameEquipment(ref List<ModbusClient> modbus)
        {
            if (modbus.Count == 0)
            {
                return;
            }

            List<ModbusClient> temp = new List<ModbusClient>();

            temp.Add(modbus[0]);

            int index = -1;

            for (int i = 1; i < modbus.Count; i++)
            {
                for (int k = 0; k < temp.Count; k++)
                {
                    if (modbus[i].Equipment == temp[k].Equipment)
                    {
                        index = -1;
                        break;
                    }
                    else
                    {
                        index = i;
                    }                   
                }

                if (index != -1)
                {
                    temp.Add(modbus[index]);
                    index = -1;
                }
            }

            MSSQL mssql = new MSSQL();

            mssql.DeleteDBnameEquipment();
            mssql.InsertDBnameEquipment(temp);
        }


        public static void NameEquipment(ModbusClient modbus)
        {
            List<ModbusClient> temp = new ConfigurationManager().GetAllDevice();
            List<ModbusClient> resultList = new List<ModbusClient>();

            bool index = false;

            for (int i = 0; i < temp.Count; i++)
            {
                if (modbus.Equipment == temp[i].Equipment)
                {
                    index = false;
                    break;
                }
                else
                {
                    index = true;
                }
            }

            if (index)
            {
                resultList.Add(modbus);
            }

            new MSSQL().InsertDBnameEquipment(resultList);
        }




        //public static void NameEquipment(ref List<ModbusClient> modbus)
        //{
        //    if (modbus.Count == 0)
        //    {
        //        return;
        //    }

        //    List<ModbusClient> temp = new List<ModbusClient>();

        //    temp.Add(modbus[0]);

        //    int index = -1;

        //    for (int i = 1; i < modbus.Count; i++)
        //    {
        //        for (int k = 0; k < temp.Count; k++)
        //        {
        //            if (modbus[i].Equipment == temp[k].Equipment && modbus[i].EquipmentName == temp[k].EquipmentName)
        //            {
        //                index = -1;
        //                break;
        //            }
        //            else if (modbus[i].Equipment != temp[k].Equipment && modbus[i].EquipmentName == temp[k].EquipmentName)
        //            {
        //                index = i;
        //            }
        //            else if (modbus[i].Equipment == temp[k].Equipment && modbus[i].EquipmentName != temp[k].EquipmentName)
        //            {
        //                index = i;
        //            }
        //            else if (modbus[i].Equipment != temp[k].Equipment && modbus[i].EquipmentName != temp[k].EquipmentName)
        //            {
        //                index = i;
        //            }
        //        }

        //        if (index != -1)
        //        {
        //            temp.Add(modbus[index]);
        //            index = -1;
        //        }
        //    }

        //    NameEquipmentString(temp);
        //}

        //public static void NameEquipment(ModbusClient modbus)
        //{
        //    List<ModbusClient> temp = new ConfigurationManager().GetAllDevice();
        //    List<ModbusClient> resultList = new List<ModbusClient>();

        //    bool index = false;

        //    for (int i = 0; i < temp.Count; i++)
        //    {
        //        if (modbus.EquipmentName == temp[i].EquipmentName && modbus.Equipment == temp[i].Equipment)
        //        {
        //            index = false;
        //            break;
        //        }
        //        else if (modbus.Equipment != temp[i].Equipment && modbus.EquipmentName == temp[i].EquipmentName)
        //        {
        //            index = true;
        //        }
        //        else if (modbus.Equipment == temp[i].Equipment && modbus.EquipmentName != temp[i].EquipmentName)
        //        {
        //            index = true;
        //        }
        //        else if (modbus.Equipment != temp[i].Equipment && modbus.EquipmentName != temp[i].EquipmentName)
        //        {
        //            index = true;
        //        }
        //    }

        //    if (index)
        //    {
        //        resultList.Add(modbus);
        //    }

        //    NameEquipmentString(resultList);
        //}


        //private static void NameEquipmentString(List<ModbusClient> temp)
        //{
        //    List<string> resultList = new List<string>();

        //    if (temp.Count > 0)
        //    {
        //        for (int i = 0; i < temp.Count; i++)
        //        {
        //            string result = $"{temp[i].EquipmentName} N {temp[i].Equipment}";
        //            resultList.Add(result);
        //        }
        //    }

        //    new MSSQL().InsertDBnameEquipment(resultList);

        //}
    }
}
