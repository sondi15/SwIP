using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace PIT
{
    class SetDHCP
    {
        public static void SetDHCP_(string nicName)
        {
            ManagementClass mc = new ManagementClass(
              "Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (var o in moc)
            {
                var mo = (ManagementObject)o;
                if (!(bool)mo["IPEnabled"]) continue;
                if (!mo["Caption"].Equals(nicName)) continue;

                var ndns = mo.GetMethodParameters("SetDNSServerSearchOrder");
                ndns["DNSServerSearchOrder"] = null;
                var enableDhcp = mo.InvokeMethod("EnableDHCP", null, null);
                var setDns = mo.InvokeMethod("SetDNSServerSearchOrder", ndns, null);
            }
        }
}
}
