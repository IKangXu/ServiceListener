using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using project_install.Common;
using ServiceListener.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ServiceListener
{
    //定义委托
    public delegate void RefreshHandler();

    /// <summary>
    /// AddServerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AddServerInfo : MetroWindow
    {
        public event RefreshHandler RefreshEvent;

        public AddServerInfo()
        {
            InitializeComponent();

            Init();
        }

        public AddServerInfo(ServerInfo ServerInfo)
        {
            InitializeComponent();

            this.Title = "编辑服务信息";

            this.Id.Text = ServerInfo.Id;
            LocalIP.Text = ServerInfo.LocalIP;
            Port.Text = ServerInfo.Port;
            Path.Text = ServerInfo.Path;
        }

        public AddServerInfo(ServerInfo ServerInfo, bool Looked)
        {
            InitializeComponent();

            this.Title = "查看服务信息";

            this.Id.Text = ServerInfo.Id;
            LocalIP.Text = ServerInfo.LocalIP;
            Port.Text = ServerInfo.Port;
            Path.Text = ServerInfo.Path;

            LocalIP.IsEnabled = false;
            Port.IsEnabled = false;
            Path.IsEnabled = false;

            SureBtn.Visibility = Visibility.Hidden;

        }

        private void Init()
        {
            ArrayList LocalIPs = IPUtils.IPAddress();
            if (null == LocalIPs)
            {
                LocalIP.Text = "127.0.0.1";
                return;
            }
            if (LocalIPs.Count == 0)
            {
                LocalIP.Text = "127.0.0.1";
                return;
            }
            LocalIP.Text = LocalIPs[0].ToString();
        }

        private void OnSureClick(object sender, RoutedEventArgs e)
        {
            string LocalIPText = LocalIP.Text.Trim();
            string PortText = Port.Text.Trim();
            string PathText = Path.Text.Trim();

            if (string.IsNullOrEmpty(LocalIPText) || string.IsNullOrEmpty(PortText) || string.IsNullOrEmpty(PathText) || IPUtils.PortInUse(int.Parse(PortText)))
            {
                this.ShowMessageAsync("填写错误提示", "本地IP、端口、路径均不能为空；端口不能被占用");
                return;
            }

            if (string.IsNullOrEmpty(Id.Text))
            {
                ServerInfoXmlUtils.Add(new ServerInfo(Guid.NewGuid().ToString(), LocalIPText, PortText, PathText, false));
            }
            else
            {
                ServerInfoXmlUtils.Edit(new ServerInfo(Id.Text.ToString(), LocalIPText, PortText, PathText, false));
            }

            RefreshEvent();

            Close();
        }


    }
}
