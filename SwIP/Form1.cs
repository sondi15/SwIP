using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Management;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace PIT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string ip;
        public string gw;
        public string sn;
        private void Form1_Load(object sender, EventArgs e)
        {
            GetIP(null, null);
            XMLRead(null, null);
            //Check online Network Interfaces
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_NetworkAdapterConfiguration");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["IPEnabled"].ToString() == "True")
                    {
                        cbNetworkCard.Items.Add(queryObj["Caption"].ToString());
                    }

                }
            }
            catch (ManagementException s)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + s.Message);
            }


        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            cbNetworkCard.SelectedIndex = -1;
            cbNetworkCard.Items.Clear();
            Form1_Load(null, null);
            GetIP(null, null);
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            XMLWrite(null, null);
            if (cbDHCP.Checked)
            {
                DialogResult result = MessageBox.Show("Möchtest du wirklich die Netzwerkkarte: \r\n" + cbNetworkCard.SelectedItem.ToString() + 
                    "\r\nauf DHCP umstellen?", "DHCP aktivieren?",
                   MessageBoxButtons.OKCancel);
                switch (result)
                {
                    case DialogResult.OK:
                        {
                            SetDHCP.SetDHCP_(cbNetworkCard.SelectedItem.ToString());
                            break;
                        }
                    case DialogResult.Cancel:
                        {
                            break;
                        }
                }

                
            }
            else
            {
                if (cbNetworkCard.SelectedIndex == -1)
                { MessageBox.Show("Bitte zuerst NIC auswählen"); }
                else
                {
                    /// <Profile Check>
                    if (rbConfig1.Checked == true)
                    { ip = tbIPconf1.Text; gw = tbGWconf1.Text; sn = tbSNconf1.Text; }
                    else if (rbConfig2.Checked == true)
                    { ip = tbIPconf2.Text; gw = tbGWconf2.Text; sn = tbSNconf2.Text; }
                    else if (rbConfig3.Checked == true)
                    { ip = tbIPconf3.Text; gw = tbGWconf3.Text; sn = tbSNconf3.Text; }
                    else if (rbConfig4.Checked == true)
                    { ip = tbIPconf4.Text; gw = tbGWconf4.Text; sn = tbSNconf4.Text; }
                    else if (rbConfig5.Checked == true)
                    { ip = tbIPconf5.Text; gw = tbGWconf5.Text; sn = tbSNconf5.Text; }
                    else if (rbConfig6.Checked == true)
                    { ip = tbIPconf6.Text; gw = tbGWconf6.Text; sn = tbSNconf6.Text; }
                    else if (rbConfig7.Checked == true)
                    { ip = tbIPconf7.Text; gw = tbGWconf7.Text; sn = tbSNconf7.Text; }
                    else if (rbConfig8.Checked == true)
                    { ip = tbIPconf8.Text; gw = tbGWconf8.Text; sn = tbSNconf8.Text; }

                    DialogResult result = MessageBox.Show(
                        "NIC:\t" + cbNetworkCard.SelectedItem.ToString() + "\n" +
                        "IP:\t" + ip + "\n" +
                        "SN:\t" + sn + "\n" +
                        "GW:\t" + gw, "Einstellungen übernehmen?",
                        MessageBoxButtons.OKCancel);
                    string dnsconfig;
                    if (cBDNS.Checked)
                    {
                        dnsconfig = tbDNS1.Text + "," + tbDNS2.Text;
                    }
                    else { dnsconfig = ","; }
                    switch (result)
                    {
                     
                        case DialogResult.OK:
                            {
                                IPchange.SetIP(cbNetworkCard.SelectedItem.ToString(), ip, sn, gw, dnsconfig);
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                break;
                            }
                    }
                    GetIP(null, null);
                }
                
            }
            
        }

        public void XMLRead(Object sender, EventArgs e)
        {
            if (File.Exists("config.xml"))
            {
                var doc = new XmlDocument();
                doc.Load("config.xml");
                foreach (XmlNode node in doc.SelectNodes("//ITEM[@id]"))
                {
                    if (node.Attributes["id"].Value == "1")
                    {
                        gBox1.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf1.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf1.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf1.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "2")
                    {
                        gBox2.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf2.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf2.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf2.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "3")
                    {
                        gBox3.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf3.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf3.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf3.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "4")
                    {
                        gBox4.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf4.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf4.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf4.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "5")
                    {
                        gBox5.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf5.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf5.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf5.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "6")
                    {
                        gBox6.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf6.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf6.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf6.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "7")
                    {
                        gBox7.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf7.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf7.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf7.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "8")
                    {
                        gBox8.Text = node.Attributes["name"].Value.ToString();
                        tbIPconf8.Text = node.Attributes["ip"].Value.ToString();
                        tbSNconf8.Text = node.Attributes["sn"].Value.ToString();
                        tbGWconf8.Text = node.Attributes["gw"].Value.ToString();
                    }
                    else if (node.Attributes["id"].Value == "dns")
                    {
                        tbDNS1.Text = node.Attributes["dns1"].Value.ToString();
                        tbDNS2.Text = node.Attributes["dns2"].Value.ToString();
                    }
                }
            }

            else { XMLCreate(null, null); };
        }

        public void XMLWrite(Object sender, EventArgs e)
        {
            var doc = new XmlDocument();
            doc.Load("config.xml");
            foreach (XmlNode node in doc.SelectNodes("//ITEM[@id]"))
            {
                if (node.Attributes["id"].Value == "1")
                {
                    node.Attributes["name"].Value = gBox1.Text;
                    node.Attributes["ip"].Value = tbIPconf1.Text;
                    node.Attributes["sn"].Value = tbSNconf1.Text;
                    node.Attributes["gw"].Value = tbGWconf1.Text;
                }
                else if (node.Attributes["id"].Value == "2")
                {
                    node.Attributes["name"].Value = gBox2.Text;
                    node.Attributes["ip"].Value = tbIPconf2.Text;
                    node.Attributes["sn"].Value = tbSNconf2.Text;
                    node.Attributes["gw"].Value = tbGWconf2.Text;
                }
                else if (node.Attributes["id"].Value == "3")
                {
                    node.Attributes["name"].Value = gBox3.Text;
                    node.Attributes["ip"].Value = tbIPconf3.Text;
                    node.Attributes["sn"].Value = tbSNconf3.Text;
                    node.Attributes["gw"].Value = tbGWconf3.Text;
                }
                else if (node.Attributes["id"].Value == "4")
                {
                    node.Attributes["name"].Value = gBox4.Text;
                    node.Attributes["ip"].Value = tbIPconf4.Text;
                    node.Attributes["sn"].Value = tbSNconf4.Text;
                    node.Attributes["gw"].Value = tbGWconf4.Text;
                }
                else if (node.Attributes["id"].Value == "5")
                {
                    node.Attributes["name"].Value = gBox5.Text;
                    node.Attributes["ip"].Value = tbIPconf5.Text;
                    node.Attributes["sn"].Value = tbSNconf5.Text;
                    node.Attributes["gw"].Value = tbGWconf5.Text;
                }
                else if (node.Attributes["id"].Value == "6")
                {
                    node.Attributes["name"].Value = gBox6.Text;
                    node.Attributes["ip"].Value = tbIPconf6.Text;
                    node.Attributes["sn"].Value = tbSNconf6.Text;
                    node.Attributes["gw"].Value = tbGWconf6.Text;
                }
                else if (node.Attributes["id"].Value == "7")
                {
                    node.Attributes["name"].Value = gBox7.Text;
                    node.Attributes["ip"].Value = tbIPconf7.Text;
                    node.Attributes["sn"].Value = tbSNconf7.Text;
                    node.Attributes["gw"].Value = tbGWconf7.Text;
                }
                else if (node.Attributes["id"].Value == "8")
                {
                    node.Attributes["name"].Value = gBox8.Text;
                    node.Attributes["ip"].Value = tbIPconf8.Text;
                    node.Attributes["sn"].Value = tbSNconf8.Text;
                    node.Attributes["gw"].Value = tbGWconf8.Text;
                }
                else if (node.Attributes["id"].Value == "dns")
                {
                    node.Attributes["dns1"].Value = tbDNS1.Text;
                    node.Attributes["dns2"].Value = tbDNS2.Text;
                }
                doc.Save("config.xml");
            }
        }

        public void XMLCreate(object sender, EventArgs e)
        {
            string[] lines = { "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>",
                "<configs>",
            "  <ITEM id=\"1\" name=\"IP Config1\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"2\" name=\"IP Config2\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"3\" name=\"IP Config3\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"4\" name=\"IP Config4\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"5\" name=\"IP Config5\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"6\" name=\"IP Config6\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"7\" name=\"IP Config7\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"8\" name=\"IP Config8\" ip=\"\" sn=\"\" gw=\"\"/>",
            "  <ITEM id=\"dns\" dns1=\"\" dns2=\"\"/>",
            "</configs>"};
            System.IO.File.WriteAllLines("config.xml", lines);
        }

        private void rbConfig1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit1.Visible = true;
                tbEdit1.Text = gBox1.Text;
                tbEdit1.Focus();
            }
        }

        private void rbConfig2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit2.Visible = true;
                tbEdit2.Text = gBox2.Text;
                tbEdit2.Focus();
            }
        }

        private void rbConfig3_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit3.Visible = true;
                tbEdit3.Text = gBox3.Text;
                tbEdit3.Focus();
            }
        }

        private void rbConfig4_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit4.Visible = true;
                tbEdit4.Text = gBox4.Text;
                tbEdit4.Focus();
            }
        }

        private void rbConfig5_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit5.Visible = true;
                tbEdit5.Text = gBox5.Text;
                tbEdit5.Focus();
            }
        }

        private void rbConfig6_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit6.Visible = true;
                tbEdit6.Text = gBox6.Text;
                tbEdit6.Focus();
            }
        }

        private void rbConfig7_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit7.Visible = true;
                tbEdit7.Text = gBox7.Text;
                tbEdit7.Focus();
            }
        }

        private void rbConfig8_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tbEdit8.Visible = true;
                tbEdit8.Text = gBox8.Text;
                tbEdit8.Focus();
            }
        }

        private void tbEdit1_Enter(object sender, EventArgs e)
        {
            gBox1.Text = tbEdit1.Text;
            tbEdit1.Visible = false;
        }

        private void tbEdit2_Leave(object sender, EventArgs e)
        {
            gBox2.Text = tbEdit2.Text;
            tbEdit2.Visible = false;
        }

        private void tbEdit3_Leave(object sender, EventArgs e)
        {
            gBox3.Text = tbEdit3.Text;
            tbEdit3.Visible = false;
        }

        private void tbEdit4_Leave(object sender, EventArgs e)
        {
            gBox4.Text = tbEdit4.Text;
            tbEdit4.Visible = false;
        }

        private void tbEdit5_Leave(object sender, EventArgs e)
        {
            gBox5.Text = tbEdit5.Text;
            tbEdit5.Visible = false;
        }

        private void tbEdit6_Leave(object sender, EventArgs e)
        {
            gBox6.Text = tbEdit6.Text;
            tbEdit6.Visible = false;
        }

        private void tbEdit7_Leave(object sender, EventArgs e)
        {
            gBox7.Text = tbEdit7.Text;
            tbEdit7.Visible = false;
        }

        private void tbEdit8_Leave(object sender, EventArgs e)
        {
            gBox8.Text = tbEdit8.Text;
            tbEdit8.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XMLWrite(null, null);
        }

        private void OnTextBox_Name_TextChanged(object sender, EventArgs a_textArgs)
        {
            TextBox a_textBox = (TextBox)sender;
            string a_newText = string.Empty;

            for (int i = 0; i < a_textBox.Text.Length; i++)
            {
                if (Regex.IsMatch(a_textBox.Text[i].ToString(), "^[0-9-.]+$"))
                {
                    a_newText += a_textBox.Text[i];
                }
            }

            a_textBox.Text = a_newText;
            a_textBox.SelectionStart = a_textBox.Text.Length;
        }

        private void GetIP(object sender, EventArgs e)
        {

            tbIP.Text = "";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
               foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        //Pseudo Interface Loopback wird ausgeblendet.
                        if(ip.Address.ToString() == "127.0.0.1" && ni.Name.Contains("Pseudo") || ni.Name.Contains("Loopback"))
                        { }
                        else
                        {
                            tbIP.Text = tbIP.Text + (ni.Name) + ":\r\n";
                            tbIP.Text = tbIP.Text + "IP: " + (ip.Address.ToString()) + " \r\n";
                            tbIP.Text = tbIP.Text + "SN: " + (ip.IPv4Mask.ToString() + " \r\n");
                            foreach (GatewayIPAddressInformation gipi in ni.GetIPProperties().GatewayAddresses)
                            {
                                tbIP.Text = tbIP.Text + "GW: " + gipi.Address + " \r\n";
                            }
                            tbIP.Text = tbIP.Text + "\r\n";
                        }       
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (cBDNS.Checked)
            {
                gBDNS.Visible = true;
            }
            else
            {
                gBDNS.Visible = false;
            }
        }
    }
}
