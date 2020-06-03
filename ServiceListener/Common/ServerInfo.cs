using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceListener.Common
{
    public class ServerInfo
    {
        public string Id { get; set; }
        public string LocalIP { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
        public bool Started { get; set; }
        public ServerInfo(string Id, string LocalIP, string Port, string Path, bool Started)
        {
            this.Id = Id;
            this.LocalIP = LocalIP;
            this.Port = Port;
            this.Path = Path;
            this.Started = Started;
        }

        public ServerInfo(string Id, string LocalIP, string Port, string Path)
        {
            this.Id = Id;
            this.LocalIP = LocalIP;
            this.Port = Port;
            this.Path = Path;
        }

        public ServerInfo() { }
    }
}
