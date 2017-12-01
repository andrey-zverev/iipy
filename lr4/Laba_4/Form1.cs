using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_4
{
    //Class of form, where we have all important methods for GUI
    public partial class Form1 : Form
    {
        //Constant for rewriting system method
        private const int WM_DEVICECHANGE = 0X219;
        private static readonly Manager _manager = new Manager();
        private List<Usb> _deviceList;
        //Our selecting table
        private readonly DataTable _table = new DataTable();

        //We follow for system messages
        protected override void WndProc(ref Message m)
        {
            //Redirect messages to our program
            base.WndProc(ref m);
            //If config of our devices has changed
            if (m.Msg == WM_DEVICECHANGE)
            {
                //Reload our data
                ReloadForm();
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        //When we first load our form
        private void LoadForm(object sender, EventArgs e)
        {
            _deviceList = new List<Usb>();
            //We should make a template for our program
            _table.Columns.Add("Название", typeof(string));
            //Get all our devices
            ReloadForm();
            usbList.DataSource = _table;
            removeButton.Enabled = false;
            timer.Enabled = true;
        }
        //Method for reloading a program
        private void ReloadForm()
        {
            int currentPosition = 0;
            //Checking if our selected row is chosen
            if (usbList.CurrentRow != null)
            {
                currentPosition = usbList.CurrentRow.Index;
            }
            //Delete all past data
            _table.Clear();
            _deviceList = _manager.DeviseListCreate();
            foreach(Usb device in _deviceList)
            {
                _table.Rows.Add(device.DeviceName);
            }
            //If there are no index of bounds
            if (usbList.RowCount - 1 > currentPosition)
            {
                //Then we select this row
                usbList.Rows[currentPosition].Selected = true;
            }
            label1.Text = "";
        }

        //Timer for live reload every 5 seconds
        private void TickTimer(object sender, EventArgs e)
        {
            ReloadForm();
        }

        //Event for selecting some row in list
        private void ChangeSelect(object sender, EventArgs e)
        {
            //If row is exists
            if (usbList.CurrentRow != null)
            {
                //If there are no index of bounds
                if (usbList.CurrentRow.Index >= 0 && usbList.CurrentRow.Index < _deviceList.Count)
                {
                    //We can eject only USB, not MTP
                    removeButton.Enabled = !_deviceList[usbList.CurrentRow.Index].IsMtpDevice;
                    //The same with output
                    if (!_deviceList[usbList.CurrentRow.Index].IsMtpDevice)
                    {
                        spaceTextBox.Text = "Свободно памяти: " + _deviceList[usbList.CurrentRow.Index].FreeSpace + "\r\n" +
                                        "Занято памяти: " + _deviceList[usbList.CurrentRow.Index].UsedSpace + "\r\n" +
                                        "Общая память: " + _deviceList[usbList.CurrentRow.Index].TotalSpace;
                    }
                }
                else
                {
                    //In other situations just block everything
                    removeButton.Enabled = false;
                    spaceTextBox.Text = "";
                }
            }
        }

        //Event for clicking deleting button
        private void OnclickButton(object sender, EventArgs e)
        {
            //If we choose what device we want to eject
            if (usbList.CurrentRow != null)
            {
                //Then we should invoke a method Eject_Device()
                bool isEjected = _deviceList[usbList.CurrentRow.Index].EjectDevice();
                if (isEjected == false)
                {
                    label1.Text = "Устройство занято.";
                }
                else
                {
                    spaceTextBox.Text = "";
                }
            }
        }

        private void spaceTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
