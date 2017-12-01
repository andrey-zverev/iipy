using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbEject;

namespace Laba_4
{
    //Generic class for USB devices
    class Usb
    {
        //Name of the device such as D:/
        public string DeviceName { get; set; }
        //Free space on disk
        public string FreeSpace { get; set; }
        //Used space on disk
        public string UsedSpace { get; set; }
        //Total space on disk
        public string TotalSpace { get; set; }
        //Mtp device check
        public bool IsMtpDevice { get; set; }

        public Usb(string name, string freeSize, string usedSize, string totalSize, bool check)
        {
            DeviceName = name;
            FreeSpace = freeSize;
            UsedSpace = usedSize;
            TotalSpace = totalSize;
            IsMtpDevice = check;
        }
        //Method for ejecting device with utility RemoveDrive
        public bool EjectDevice()
        {
            var tempName = this.DeviceName.Remove(2);
            var ejectedDevice = new VolumeDeviceClass().SingleOrDefault(v => v.LogicalDrive == this.DeviceName.Remove(2));
            ejectedDevice.Eject(false);
            ejectedDevice = new VolumeDeviceClass().SingleOrDefault(v => v.LogicalDrive == tempName);
            if (ejectedDevice == null)
                return true;
            else
                return false;
        }
    }
}
