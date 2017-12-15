using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManager
{
    class DeviceControll
    {
        private const string SelectRequest = "SELECT * FROM Win32_PNPEntity";
        public List<Device> Devices;

        public DeviceControll()
        {
            Devices = new List<Device>();
            var devicesObject = new ManagementObjectSearcher(SelectRequest);
            foreach (ManagementObject device in devicesObject.Get())
            {
                List<string> pathNameList = new List<string>();
                List<string> descriptionList = new List<string>();
                foreach (var sys in device.GetRelated("Win32_SystemDriver"))
                {
                    pathNameList.Add(sys["PathName"].ToString());
                    descriptionList.Add(sys["Description"].ToString());
                }
                Devices.Add(new Device
                {
                    Name = device["Name"] != null ? device["Name"].ToString() : "",
                    ClassGuid = device["ClassGuid"] != null ? device["ClassGuid"].ToString() : "",
                    HardwareID = device["HardwareID"] != null ? (string[])device["HardwareID"] : null,
                    Manufacturer = device["Manufacturer"] != null ? device["Manufacturer"].ToString() : "",
                    DeviceID = device["DeviceID"] != null ? device["DeviceID"].ToString() : "",
                    PathName = pathNameList,
                    Description = descriptionList,
                    Status = device["Status"].ToString() == "OK"
                });
            }
        }

        public void DisableDevice(Device _device)
        {
            var device = new ManagementObjectSearcher(SelectRequest).Get()
                .OfType<ManagementObject>()
                .FirstOrDefault(x => x.Properties["DeviceID"].Value.ToString().Equals(_device.DeviceID));
                device.InvokeMethod("Disable", new object[] { false });
        }

        public void EnableDevice(Device _device)
        {
            var device = new ManagementObjectSearcher(SelectRequest).Get()
                 .OfType<ManagementObject>()
                 .FirstOrDefault(x => x["DeviceID"].ToString().Equals(_device.DeviceID));
                device.InvokeMethod("Enable", new object[] { false });
        }
    }
}
