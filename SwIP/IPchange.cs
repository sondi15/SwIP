using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace PIT
{
    class IPchange
    {
        /// <summary>
        /// Set IP for the specified network card name
        /// </summary>
        /// <PARAM name="nicName">Caption of the network card</PARAM>
        /// <PARAM name="IpAddresses">Comma delimited string 
        ///           containing one or more IP</PARAM>
        /// <PARAM name="SubnetMask">Subnet mask</PARAM>
        /// <PARAM name="Gateway">Gateway IP</PARAM>
        /// <PARAM name="DnsSearchOrder">Comma delimited DNS IP</PARAM>
        public static void SetIP(string nicName, string IpAddresses,
          string SubnetMask, string Gateway, string DnsSearchOrder)
        {
            ManagementClass mc = new ManagementClass(
              "Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                // Make sure this is a IP enabled device. 
                // Not something like memory card or VM Ware
                    if (mo["Caption"].Equals(nicName))
                    {

                        ManagementBaseObject newIP =
                          mo.GetMethodParameters("EnableStatic");
                        ManagementBaseObject newGate =
                          mo.GetMethodParameters("SetGateways");
                        ManagementBaseObject newDNS =
                          mo.GetMethodParameters("SetDNSServerSearchOrder");

                        newGate["DefaultIPGateway"] = new string[] { Gateway };
                        newGate["GatewayCostMetric"] = new int[] { 1 };

                        newIP["IPAddress"] = IpAddresses.Split(',');
                        newIP["SubnetMask"] = new string[] { SubnetMask };

                        newDNS["DNSServerSearchOrder"] = DnsSearchOrder.Split(',');


                        ManagementBaseObject setIP = mo.InvokeMethod(
                          "EnableStatic", newIP, null);
                        ManagementBaseObject setGateways = mo.InvokeMethod(
                          "SetGateways", newGate, null);

                        ManagementBaseObject setDNS = mo.InvokeMethod(
                          "SetDNSServerSearchOrder", newDNS, null);
                    break;
                    }
            }
        }
    }
}
