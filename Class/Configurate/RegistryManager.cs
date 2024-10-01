using ConfigurateService.Class.Enums;
using ConfigurateService.Class.Log;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.AccessControl;

namespace ConfigurateService.Class.Configurate
{
    class RegistryManager
    {
        private static readonly string dir = @$"{Directory.GetCurrentDirectory()}\Settings\";
        private readonly string nameKey = @"SOFTWARE\VAST\ArchiveService";
        private readonly string[] keys = new string[] { "InstallService" , "ComponentsInstall", "SyncPath" };
        private readonly Logging logger = new Logging();


        #region Старый IsInstall()

        //internal bool IsInstall()
        //{
        //    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32)
        //        .OpenSubKey(nameKey).GetValue(keys[0]) == null ? false : true;
        //}

        #endregion

        internal bool IsInstall()
        {
            bool temp = false;

            using (RegistryKey localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                using (RegistryKey setLocalMachineKey = localMachineKey.OpenSubKey(nameKey))
                {
                    if (setLocalMachineKey.GetValue(keys[0]) == null)
                    {
                        logger.WtiteLog(@$"Запись в разделе реестра отсутствует! {keys[0]}");
                    }
                    else
                    {
                        logger.WtiteLog(@$"Запись в реестре уже сделана! {keys[0]}");
                        temp = true;
                    }
                }
            }

            return temp;
        }

        internal void SetInstall()
        {
            SyncronizationPath();
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(nameKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            {
                key.SetValue(keys[0],1);
            }
            logger.WtiteLog("Установлена служба");
        }

        #region Старый IsHaveComponents()
        //internal bool IsHaveComponents()
        //{
        //    return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32)
        //        .OpenSubKey(nameKey).GetValue(keys[1]) == null ? false : true;

        //}

        #endregion

        /// <summary>
        /// Метод проверяет наличие записи в в реестре "HKEY_LOCAL_MACHINE" ключа "ComponentsInstall"
        /// </summary>
        /// <returns>Возвращае "true" или "false"</returns>
        internal bool IsHaveComponents()
        {
            bool temp = false;

            using (RegistryKey localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, 
                Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                using (RegistryKey setLocalMachineKey = localMachineKey.OpenSubKey(nameKey))
                {
                    if (setLocalMachineKey.GetValue(keys[1]) == null)
                    {
                        logger.WtiteLog(@$"Запись в разделе реестра отсутствует! {keys[1]}");
                    }
                    else
                    {
                        logger.WtiteLog(@$"Запись в реестре уже сделана! {keys[1]}");
                        temp = true;
                    }                       
                }
            }

            return temp;
        }

        #region Старый SetComponents()
        //internal void SetComponents()
        //{
        //    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(nameKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
        //    {
        //        key.SetValue(keys[1], 1);
        //    }
        //    logger.WtiteLog("Установлены компоненты");
        //}

        #endregion

        /// <summary>
        /// Метод записывает в реестр "HKEY_LOCAL_MACHINE" ключ "ComponentsInstall"
        /// </summary>
        /// <exception cref="InvalidOperationException">Выбрасывает исключение если не удалось сделатть запись</exception>
        internal void SetComponents()
        {
            using (RegistryKey localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                using (RegistryKey setLocalMachineKey = localMachineKey.CreateSubKey(nameKey, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (setLocalMachineKey == null)
                    {
                        logger.WtiteLog(@$"Не удалось создать раздел реестра! {keys[1]}");
                        throw new InvalidOperationException(@$"Не удалось создать раздел реестра! {keys[1]}");
                    }
                        
                    setLocalMachineKey.SetValue(keys[1], 1);
                }
            }

            logger.WtiteLog(@$"Создан раздел реестра! {keys[1]}");
        }

        private void SyncronizationPath()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(nameKey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
            {
                if (key == null)
                    Registry.LocalMachine.CreateSubKey(nameKey).SetValue(keys[2], dir);
                else
                    key.SetValue(keys[2], dir);
            }
            logger.WtiteLog("Синхронизация пути успешна");
        }
    }
}
