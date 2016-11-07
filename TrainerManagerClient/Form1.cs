using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TrainerManagerClient
{
    public partial class Form1 : Form
    {
        private static UdpClient  m_Client;
        private static IPAddress  m_MulticastGrp = IPAddress.Parse(ConfigurationManager.AppSettings["clientMulticastGrp"]);
        private static int m_MulticastPort = System.Convert.ToInt16(ConfigurationManager.AppSettings["clientMulticastPort"]);
        private static IPEndPoint m_ClientEndpoint;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;

            m_Client = new UdpClient(AddressFamily.InterNetwork);
            m_Client.JoinMulticastGroup(m_MulticastGrp);
            m_ClientEndpoint = new IPEndPoint(m_MulticastGrp, m_MulticastPort);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string host = Dns.GetHostName();
            foreach(var ip in Dns.GetHostAddresses(host))
            {
                //if (ip.ToString().StartsWith("14"))
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Send.SendData(m_Client, m_ClientEndpoint, ip.ToString());
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
#if !DEBUG
            if(e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
#endif

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                //               this.notifyIcon1.Visible = true;
                //               this.notifyIcon1.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Normal;
            //this.ShowInTaskbar = true;
            //            this.notifyIcon1.Visible = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }

    public class Send
    {
        public static void SendData(UdpClient c, IPEndPoint ep, string data)
        {
            c.Send(GetBytesToArray(data.ToCharArray()), data.Length, ep);
        }

        private static Byte[] GetBytesToArray(Char[] ch)
        {
            Byte[] ret = new Byte[ch.Length];
            for (int i = 0; i < ch.Length; i++)
                ret[i] = (Byte)ch[i];

            return ret;
        }
    }

}
