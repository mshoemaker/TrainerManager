using System;
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

namespace TrainerManager
{
    public partial class Form1 : Form
    {
        string machineListFile = ConfigurationManager.AppSettings["machineList"];

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
                foreach (XmlNode trainerNode in node)
                {
                    string ip = trainerNode.Attributes["ipAddress"]?.InnerText;
                    string status = trainerNode.Attributes["status"]?.InnerText;
                }
            }
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

            listView1.Columns.Add("Trainer #", 100);
            listView1.Columns.Add("IP Addres", 100);
            listView1.Columns.Add("Status", 100);

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
            // send manager heartbeat

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
            if(e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
