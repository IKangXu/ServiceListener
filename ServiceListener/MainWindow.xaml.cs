using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using project_install.Common;
using ServiceListener.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ServiceListener
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            InitialTray();

            Init();

            RefreshTable();

        }
        
        private void Init()
        {
            List<ServerInfo> Result = ServerInfoXmlUtils.All();
            for (int i = 0; i < Result.Count; i++)
            {
                ServerInfo ServerInfo = Result[i];
                if (Result[i].Started)
                {
                    // 启动
                    Server Server = new Server();
                    Server.Start(IPAddress.Parse(ServerInfo.LocalIP), int.Parse(ServerInfo.Port), 100, ServerInfo.Path);

                    ServerManager.GetInstance().Add(ServerInfo.Id, Server);
                }
            }
        }


        private void RefreshTable()
        {
            ObservableCollection<ServerInfo> Infos = new ObservableCollection<ServerInfo>();

            List<ServerInfo> Result = ServerInfoXmlUtils.All();

            for (int i = 0; i < Result.Count; i++)
            {
                Infos.Add(Result[i]);
            }
            ServerInfos.ItemsSource = Infos;
        }

        private void OnAddClick(object sender, RoutedEventArgs e)
        {
            AddServerInfo AddServerInfo = new AddServerInfo();
            AddServerInfo.RefreshEvent += AddServerInfoRefreshEvent;

            AddServerInfo.ShowDialog();
        }

        private void AddServerInfoRefreshEvent()
        {
            RefreshTable();
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Btn = sender as System.Windows.Controls.Button;
            object obj = Btn.Tag;

            ServerInfo ServerInfo = ServerInfoXmlUtils.GetValById(obj.ToString());

            // 启动
            Server Server = new Server();
            Server.Start(IPAddress.Parse(ServerInfo.LocalIP), int.Parse(ServerInfo.Port), 100, ServerInfo.Path);

            ServerManager.GetInstance().Add(obj.ToString(), Server);

            // 变更状态
            ServerInfoXmlUtils.Start(obj.ToString());

            // 刷新表格
            RefreshTable();
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Btn = sender as System.Windows.Controls.Button;
            object obj = Btn.Tag;

            ServerManager.GetInstance().Get(obj.ToString()).Stop();
            ServerManager.GetInstance().Remove(obj.ToString());

            // 变更状态
            ServerInfoXmlUtils.Stop(obj.ToString());

            RefreshTable();
        }

        private void OnRestartClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Btn = sender as System.Windows.Controls.Button;
            object obj = Btn.Tag;

            Server Server = ServerManager.GetInstance().Get(obj.ToString());
            if (null == Server)
            {
                return;
            }

            ServerInfo ServerInfo = ServerInfoXmlUtils.GetValById(obj.ToString());

            Server.Stop();
            Server.Start(IPAddress.Parse(ServerInfo.LocalIP), int.Parse(ServerInfo.Port), 100, ServerInfo.Path);
        }

        private NotifyIcon NotifyIcon = null;

        private void InitialTray()
        {
            //隐藏主窗体
            // this.Visibility = System.Windows.Visibility.Hidden;
            //设置托盘的各个属性
            NotifyIcon = new NotifyIcon();
            NotifyIcon.BalloonTipText = "服务监听程序运行中...";//托盘气泡显示内容
            NotifyIcon.Text = "服务监听";
            NotifyIcon.Visible = true;//托盘按钮是否可见
            NotifyIcon.Icon = new Icon("favicon.ico");//托盘中显示的图标
            NotifyIcon.ShowBalloonTip(2000);//托盘气泡显示时间
            NotifyIcon.MouseDoubleClick += OnNotifyIconMouseDoubleClick;

            System.Windows.Forms.ContextMenu MapContextMenu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem Show = new System.Windows.Forms.MenuItem();
            Show.Text = "显示";
            Show.Click += OnShowClick;
            MapContextMenu.MenuItems.Add(Show);

            System.Windows.Forms.MenuItem Exit = new System.Windows.Forms.MenuItem();
            Exit.Click += OnExitClick;
            Exit.Text = "退出";
            MapContextMenu.MenuItems.Add(Exit);

            NotifyIcon.ContextMenu = MapContextMenu;
            StateChanged += OnMainWindowStateChanged;

        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitClick(object sender, EventArgs e)
        {
            if (Visibility == System.Windows.Visibility.Hidden)
            {
                Visibility = System.Windows.Visibility.Visible;
                Activate();
            }
            DialogsBeforeExit();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowClick(object sender, EventArgs e)
        {
            if (Visibility == System.Windows.Visibility.Hidden)
            {
                Visibility = System.Windows.Visibility.Visible;
                Activate();
                Topmost = true;
            }
        }

        /// <summary>
        /// 窗口状态改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMainWindowStateChanged(object sender, EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
            {
                Visibility = System.Windows.Visibility.Hidden;
            }
        }

        /// <summary>
        /// 托盘图标鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNotifyIconMouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Visibility == System.Windows.Visibility.Visible)
                {
                    Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    Visibility = System.Windows.Visibility.Visible;
                    Activate();
                }
            }
        }

        /// <summary>
        /// 关闭窗体之前的提示对话框
        /// </summary>
        private async void DialogsBeforeExit()
        {
            MessageDialogResult clickresult = await this.ShowMessageAsync(this.Title, "程序正在执行中。如果您现在退出，您的程序将直接中止导致系统无法使用。\n\n您可以以后再运行启动程序来使用系统。\n\n退出运行程序吗？", MessageDialogStyle.AffirmativeAndNegative);
            if (clickresult == MessageDialogResult.Negative)//取消
            {
                return;
            }
            else//确认
            {
                //Close();
                //Process.GetCurrentProcess().Kill();
                // 直接执行cmd命令，结束start-server.exe进程树
                Exit();
            }
        }

        private void OnAppcationClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            CancellationToken token;
            TaskScheduler uiSched = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(Exit, token, TaskCreationOptions.None, uiSched);
        }

        private void Exit()
        {
            ExecuteInCmd("TASKKILL /F /T /IM \"ServiceListener.exe\"");
        }

        /// <summary>
        /// 执行内部命令（cmd.exe 中的命令）
        /// </summary>
        /// <param name="cmdline">命令行</param>
        /// <returns>执行结果</returns>
        public static string ExecuteInCmd(string cmdline)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine(cmdline + "&exit");

                //获取cmd窗口的输出信息  
                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();

                return output;
            }
        }

        private void OnRemoveClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Btn = sender as System.Windows.Controls.Button;
            object obj = Btn.Tag;

            DialogsBeforeRemove(obj.ToString());
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Btn = sender as System.Windows.Controls.Button;
            object obj = Btn.Tag;

            ServerInfo ServerInfo = ServerInfoXmlUtils.GetValById(obj.ToString());

            AddServerInfo AddServerInfo = new AddServerInfo(ServerInfo);
            AddServerInfo.RefreshEvent += AddServerInfoRefreshEvent;

            AddServerInfo.ShowDialog();
        }

        private void OnLookClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button Btn = sender as System.Windows.Controls.Button;
            object obj = Btn.Tag;

            ServerInfo ServerInfo = ServerInfoXmlUtils.GetValById(obj.ToString());

            AddServerInfo AddServerInfo = new AddServerInfo(ServerInfo, true);
            AddServerInfo.ShowDialog();
        }

        private async void DialogsBeforeRemove(string Id)
        {
            MessageDialogResult clickresult = await this.ShowMessageAsync("数据删除", "删除之后数据无法恢复，您确定要删除吗？", MessageDialogStyle.AffirmativeAndNegative);
            if (clickresult == MessageDialogResult.Negative)//取消
            {
                return;
            }
            else//确认
            {
                ServerInfoXmlUtils.Remove(Id);
                RefreshTable();
            }
        }

    }
}
