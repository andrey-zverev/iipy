using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceManager
{
    class ShowInfo
    {
        public ListViewItem[] GetDevicesList(List<Device> devices)
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (var device in devices)
            {
                list.Add(new ListViewItem(device.Name));
            };
            return list.ToArray();
        }

        public string PrintDeviceInfo(Device device)
        {
            string info = "Name: " + device.Name +
                "\n\nManufacturer: " + device.Manufacturer +
                "\n\nGUID: " + device.ClassGuid +
                "\n\nDeviceID: " + device.DeviceID;
            if (device.HardwareID != null)
            {
                info += "\n\nHardwareID: ";
                for (int i = 0; i < device.HardwareID.Length; i++)
                {
                    info += "\n" + device.HardwareID[i];
                }
            }
            for (int i = 0; i < device.PathName.Count; i++)
            {
                info += "\n\nPathName: " + device.PathName.ElementAt(i) +
                    "\n\nDescription: " + device.Description.ElementAt(i);
            }
            return info;
        }
    }
}
