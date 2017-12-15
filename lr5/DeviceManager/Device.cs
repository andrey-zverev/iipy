using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManager
{
    public class Device
    {
        public string Name { get; set; }
        public string ClassGuid { get; set; }
        public string[] HardwareID { get; set; }
        public string Manufacturer { get; set; }
        public List<string> PathName { get; set; }
        public List<string> Description { get; set; }
        public string DeviceID { get; set; }
        public bool Status { get; set; }
    }
}
