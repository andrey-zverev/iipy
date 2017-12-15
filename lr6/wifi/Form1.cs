using System;
using System.Windows.Forms;

namespace lab6
{
    public partial class Form1 : Form
    {
        private ConnectionsControll _connectionsControll;
        private Timer _timer;
        private int _selectedIndex;
        private bool IsValidSelection
        {
            get
            {
                if ((_selectedIndex != -1) && (_selectedIndex < _connectionsControll.NumberOfConnections))
                {
                    return true;
                }
                else
                {
                    _selectedIndex = -1;
                    return false;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
            _connectionsControll = new ConnectionsControll();
            _selectedIndex = -1;
            Load += (s, e) =>
            {
                RepaintUI();
                _timer = new Timer
                {
                    Interval = 5000
                };
                _timer.Tick += new EventHandler(Timer_Tick);
                _timer.Start();
            };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            RepaintUI();
        }

        private void RepaintUI()
        {
            _timer?.Stop();

            ShowConnections(_connectionsControll.FindAllPoints());
            if (IsValidSelection)
            {
                connectionsList.Items[_selectedIndex].Selected = true;
                ShowSelectedInfo();
            }
           
            _timer?.Start();
        }

        private void ShowConnections(bool turnedOn)
        {
            connectionsList.Items.Clear();
            if (turnedOn)
                connectionsList.Items.AddRange(_connectionsControll.ConnectionsNames);
            else
                connectionsList.Items.Add("Wi-Fi отключен");
        }

        private void ShowSelectedInfo()
        {
            infoList.Items.Clear();
            infoList.Items.AddRange(_connectionsControll.ConnectionInfo(_selectedIndex));
        }

        private void ConnectMessage(bool success)
        {
            if (success)
                MessageBox.Show("Успешное подключение");
            else
                MessageBox.Show("Ошибка при подключении");
            passwordBox.Text = "";
        }

        private void DisconnectButton_Click(object sender, EventArgs e)  
        {
            _connectionsControll.Disconnect();
            MessageBox.Show("Успешное отключение");
        }

        private void ConnectButton_Click(object sender, EventArgs e)  
        {
            if (IsValidSelection)
            {
                var password = passwordBox.Text;
                _connectionsControll.Connect(_selectedIndex, password, ConnectMessage,checkBox1.Checked);
            }
        }

        private void ConnectionsList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var selected = connectionsList.FocusedItem;
                if ((selected != null) && (selected.Bounds.Contains(e.Location) == true) && _connectionsControll.CheckWifi())
                {
                    _selectedIndex = selected.Index;
                    ShowSelectedInfo();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordBox.Enabled = !checkBox1.Checked;
        }
    }
}
