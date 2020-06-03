using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ServiceListener.Common
{
    class ServerInfoXmlUtils
    {

        private static string XmlPath = @"Config\ServerInfo.xml";

        public static void Add(ServerInfo serverInfo)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlPath);

            XmlElement Root = XmlDoc.DocumentElement;//取到根结点

            XmlElement ServerInfo = XmlDoc.CreateElement("ServerInfo");

            XmlNode Id = XmlDoc.CreateNode("element", "Id", "");
            XmlNode LocalIP = XmlDoc.CreateNode("element", "LocalIP", "");
            XmlNode Port = XmlDoc.CreateNode("element", "Port", "");
            XmlNode Path = XmlDoc.CreateNode("element", "Path", "");
            XmlNode Started = XmlDoc.CreateNode("element", "Started", "");

            Id.InnerText = serverInfo.Id;
            LocalIP.InnerText = serverInfo.LocalIP;
            Port.InnerText = serverInfo.Port;
            Path.InnerText = serverInfo.Path;
            Started.InnerText = serverInfo.Started.ToString();

            ServerInfo.AppendChild(Id);
            ServerInfo.AppendChild(LocalIP);
            ServerInfo.AppendChild(Port);
            ServerInfo.AppendChild(Path);
            ServerInfo.AppendChild(Started);

            if(Root.ChildNodes.Count == 0)
            {
                Root.AppendChild(ServerInfo);
            }
            else
            {
                Root.InsertBefore(ServerInfo, Root.FirstChild);
            }
            

            XmlDoc.Save(XmlPath);
        }

        public static void Edit(ServerInfo serverInfo)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlPath);

            XmlNode Node = GetNodeById(serverInfo.Id, XmlDoc);
           
            XmlNodeList ChildNodes = Node.ChildNodes;
            foreach (XmlNode ChildNode in ChildNodes)
            {
                if ("LocalIP".Equals(ChildNode.Name))
                {
                    ChildNode.InnerText = serverInfo.LocalIP.Trim();

                }
                else if ("Port".Equals(ChildNode.Name))
                {
                    ChildNode.InnerText = serverInfo.Port.Trim();

                }
                else if ("Path".Equals(ChildNode.Name))
                {
                    ChildNode.InnerText = serverInfo.Path.Trim();

                }
            }
            XmlDoc.Save(XmlPath);

        }

        public static void Remove(string Id)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlPath);

            XmlNode Node = GetNodeById(Id, XmlDoc);

            XmlElement Root = XmlDoc.DocumentElement;//取到根结点
            Root.RemoveChild(Node);
            
            XmlDoc.Save(XmlPath);
        }

        public static void Start(string Id)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlPath);

            XmlNode Node = GetNodeById(Id, XmlDoc);

            XmlNodeList ChildNodes = Node.ChildNodes;
            foreach (XmlNode ChildNode in ChildNodes)
            {
                if ("Started".Equals(ChildNode.Name))
                {
                    ChildNode.InnerText = true.ToString();

                    break;
                }
            }

            XmlDoc.Save(XmlPath);
        }

        public static void Stop(string Id)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlPath);

            XmlNode Node = GetNodeById(Id, XmlDoc);

            XmlNodeList ChildNodes = Node.ChildNodes;
            foreach (XmlNode ChildNode in ChildNodes)
            {
                if ("Started".Equals(ChildNode.Name))
                {
                    ChildNode.InnerText = false.ToString();

                    break;
                }
            }
            XmlDoc.Save(XmlPath);
        }

        public static ServerInfo GetValById(string Id)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(XmlPath);

            XmlNode ServerInfoNode = GetNodeById(Id, XmlDoc);
            
            return ConverNode(ServerInfoNode);
        }

        private static ServerInfo ConverNode(XmlNode ServerInfoNode)
        {
            ServerInfo ServerInfo = new ServerInfo();

            XmlNodeList XmlNodes = ServerInfoNode.ChildNodes;
            foreach (XmlNode XmlNode in XmlNodes)
            {
                if ("Id".Equals(XmlNode.Name))
                {
                    ServerInfo.Id = XmlNode.InnerText.ToString();
                }
                else if ("LocalIP".Equals(XmlNode.Name))
                {
                    ServerInfo.LocalIP = XmlNode.InnerText.ToString();
                }
                else if ("Port".Equals(XmlNode.Name))
                {
                    ServerInfo.Port = XmlNode.InnerText.ToString();
                }
                else if ("Path".Equals(XmlNode.Name))
                {
                    ServerInfo.Path = XmlNode.InnerText.ToString();
                }
                else if ("Started".Equals(XmlNode.Name))
                {
                    ServerInfo.Started = bool.Parse(XmlNode.InnerText.ToString());
                }
            }
            return ServerInfo;
        }

        public static XmlNode GetNodeById(string Id, XmlDocument XmlDoc)
        {
            XmlNodeList Nodes = XmlDoc.SelectNodes("ServerInfos/ServerInfo/Id");

            foreach (XmlNode Node in Nodes)
            {
                if (Id.Equals(Node.InnerText))
                {
                    return Node.ParentNode;
                }
            }

            return null;
        }

        public static List<ServerInfo> All()
        {
            List<ServerInfo> ServerInfos = new List<ServerInfo>();

            XmlDocument Doc = new XmlDocument();
            Doc.Load(XmlPath);
            XmlNode ServerInfosNode = Doc.SelectSingleNode("ServerInfos");
            XmlNodeList ServerInfoNodes = ServerInfosNode.ChildNodes;
            foreach (XmlNode ServerInfoNode in ServerInfoNodes)
            {
                ServerInfos.Add(ConverNode(ServerInfoNode));
            }
            return ServerInfos;
        }

    }
}
