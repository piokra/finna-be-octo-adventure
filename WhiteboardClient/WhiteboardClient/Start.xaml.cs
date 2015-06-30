using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Net.Sockets;
using System.Net;
namespace WhiteboardClient
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow mw = null;
            try
            {
                int port = Int32.Parse(PortServer.Text);
                System.Diagnostics.Process.Start("Broadcaster.exe", PortServer.Text);
                Thread t = Thread.CurrentThread;
                System.Timers.Timer ti = new System.Timers.Timer();
                ti.Interval = 1000;
                ti.Start();
                while (!ti.Enabled) ;
                ti.Stop();
                mw = new MainWindow("localhost", port);
                mw.Show();
                this.Close();
                
            }
            catch (Exception)
            {
                
                
            }
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = null;
            try
            {
                string hostname = HostnameBox.Text;
                IPAddress ip;
                TcpClient tcp = null;
                int port = Int32.Parse(PortConnect.Text);
                
                if(IPAddress.TryParse(hostname,out ip))
                {
                    IPEndPoint iep = new IPEndPoint(ip,port);
                    tcp = new TcpClient();
                    tcp.Connect(iep);
                }
                else
                {
                    tcp = new TcpClient(hostname, port);
                }
                
                
                
                
                tcp.Close();
                mw = new MainWindow(hostname, port);
                mw.Show();
                this.Close();
            }
            catch (Exception)
            {
                
                
            }
        }
    }
}
