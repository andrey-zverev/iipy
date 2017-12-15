using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceManager
{
    public partial class Form1 : Form
    {
        private DeviceControll _deviceControll = new DeviceControll();
        private ShowInfo _showInfo = new ShowInfo();

        public Form1()
        {
            InitializeComponent();
            InitBox();
        }

        private void InitBox()
        {
            deviceBox.Items.AddRange(_showInfo.GetDevicesList(_deviceControll.Devices));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string devName = deviceBox.SelectedItems[0].SubItems[0].Text;
            Device device = _deviceControll.Devices.FirstOrDefault(p => p.Name == devName);
            if (device.Status)
            {
                _deviceControll.DisableDevice(device);
            }
            else
            {
                _deviceControll.EnableDevice(device);
            }
            device.Status = !device.Status;
            offBtn.Text = device.Status ? "Выключить" : "Включить";
            offBtn.Enabled = false;
        }

        private void deviceBox_MouseClick(object sender, MouseEventArgs e)
        {
            string devName = deviceBox.SelectedItems[0].SubItems[0].Text;
            Device device = _deviceControll.Devices.FirstOrDefault(p => p.Name == devName);
            infoBox.Text = _showInfo.PrintDeviceInfo(device);
            offBtn.Text = device.Status ? "Выключить" : "Включить";
            offBtn.Enabled = true;
        }
    }
}
