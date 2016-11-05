﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using System.Net;
using System.Net.Sockets;

namespace TrainerManager
{
    public partial class Form1 : Form
    {
        string machineListFile = ConfigurationManager.AppSettings["machineList"];

        IPAddress managerMulticastGrp = IPAddress.Parse(ConfigurationManager.AppSettings["managerMulticastGrp"]);
        int managerMulticastPort = System.Convert.ToInt16(ConfigurationManager.AppSettings["managerMulticastPort"]);
        IPAddress clientMulticastGrp = IPAddress.Parse(ConfigurationManager.AppSettings["clientMulticastGrp"]);
        int clientMulticastPort = System.Convert.ToInt16(ConfigurationManager.AppSettings["clientMulticastPort"]);

        List<ListViewItem> machines = new List<ListViewItem>();

        IPAddress _multicastIp;
        int _port;

        public Form1()
        {
            InitializeComponent();
        }

        private void outputFile()
        {

        }

        private void readFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(machineListFile);

            foreach(XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string trainerNum = "Trainer " + node.Attributes["trainerNum"]?.InnerText;
                foreach (XmlNode trainerNode in node)
                {
                    string ip = trainerNode.Attributes["ipAddress"]?.InnerText;
                    string status = trainerNode.Attributes["status"]?.InnerText;

                    string[] listItem = { trainerNum, ip};
                    int led = 0;
                    if (status.Equals("0"))
                        led = 0;
                    else
                        led = 1;
                    ListViewItem item = new ListViewItem(listItem, led);
                    machines.Add(item);
                    //this.listView1.Items.Add(item);
                }
            }
            listView1.Items.AddRange(machines.ToArray());
        }

        private void sendManagerHeartbeat()
        {

        }

        private void receiveManagerHeartbeat()
        {

        }

        private void receiveClientHeartbeat()
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            int colWidth = (listView1.Width / 2) - 2;
            listView1.Columns.Add("Trainer #", colWidth);
            listView1.Columns.Add("IP Addres", colWidth);

            // read machine list from file, local machines only?  Entire map?
            readFile();

            // display table of machines read from the file, marked as RED until they respond

            // start timer that will listen for client heartbeats

            // start timer that will listen for manager heartbeats

            // start timer that will send manager heartbeats
        }

        private void tmrReceiveClientHeartbeat_Tick(object sender, EventArgs e)
        {
            // check network for client heartbeats

        }

        private void tmrReceiveManagerHeartbeat_Tick(object sender, EventArgs e)
        {
            // check network for manager heartbeats

        }

        private void tmrSendManagerHeartbeat_Tick(object sender, EventArgs e)
        {
            //// send manager heartbeat
            //foreach (IPAddress localIp in Dns.GetHostAddresses(Dns.GetHostName()).Where(i => i.AddressFamily == AddressFamily.InterNetwork))
            //{
            //    IPAddress ipToUse = localIp;
            //    using (var mSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            //    {
            //        mSendSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
            //                                    new MulticastOption(_multicastIp, localIp));
            //        mSendSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 255);
            //        mSendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //        mSendSocket.MulticastLoopback = true;
            //        mSendSocket.Bind(new IPEndPoint(ipToUse, _port));

            //        byte[] bytes = Encoding.ASCII.GetBytes("This is my welcome message");
            //        var ipep = new IPEndPoint(_multicastIp, _port);
            //        mSendSocket.SendTo(bytes, ipep);
            //    }
            //}
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if(FormWindowState.Minimized == this.WindowState)
            {
 //               this.notifyIcon1.Visible = true;
 //               this.notifyIcon1.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
//            this.notifyIcon1.Visible = false;
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_Resize(object sender, EventArgs e)
        {
            if (listView1.Columns.Count > 0)
            {
                int colWidth = (listView1.Width / 2) - 2;
                listView1.Columns[0].Width = colWidth;
                listView1.Columns[1].Width = colWidth;
            }
        }

        private void tmrStartupDelay_Tick(object sender, EventArgs e)
        {
            tmrReceiveClientHeartbeat.Start();
            tmrReceiveManagerHeartbeat.Start();
            tmrSendManagerHeartbeat.Start();

            tmrStartupDelay.Stop();
        }
    }
}
