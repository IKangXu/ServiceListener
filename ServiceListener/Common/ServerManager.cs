using project_install.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceListener.Common
{
    class ServerManager
    {

        private static Dictionary<string, Server> Datas = new Dictionary<string, Server>();

        private static ServerManager Instance = null;

        public static ServerManager GetInstance()
        {
            if (null == Instance)
            {
                Instance = new ServerManager();
            }
            return Instance;
        }

        public Dictionary<string, Server> Add(string key, Server val)
        {
            Datas.Add(key, val);
            return Datas;
        }

        public Dictionary<string, Server> Remove(string key)
        {
            Datas.Remove(key);
            return Datas;
        }

        public Server Get(string id)
        {
            return Datas[id];
        }
        
    }
}
