using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurateService.Class.Patterns
{
    internal class ServerSettings
    {
        /// <summary>
        /// IP адрес вервера
        /// </summary>
        public string IP {  get; set; }

        /// <summary>
        /// Port сервера
        /// </summary>
        public string Port { get; set; }

        public ServerSettings(string ip, string port) 
        {
            IP = ip;
            Port = port;
        }
    }
}
