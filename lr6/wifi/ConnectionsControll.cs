using System;
using System.Collections.Generic;
using SimpleWifi;
using System.Diagnostics;
using System.Windows.Forms;

namespace lab6
{
    class ConnectionsControll
    {
        public List<Connection> Connections { get; private set; }
        private string[] _bssids;
        private Wifi _wifi;
        private const int LengthOfMacString = 17;

        public ConnectionsControll()
        {
            Connections = new List<Connection>();
            _wifi = new Wifi();
        }

        public int NumberOfConnections
        {
            get
            {
                return Connections.Count;
            }
        }

        public bool FindAllPoints()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = "cmd",
                    Arguments = @"/C ""netsh wlan show networks mode=bssid | findstr SSID""",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            _bssids = process.StandardOutput.ReadToEnd().Replace(" ", "").Replace("\r", "").Split('\n');
            process.WaitForExit();
            try
            {
                List<Connection> newConnections = new List<Connection>();
                foreach (var accessPoint in _wifi.GetAccessPoints())
                {
                    newConnections.Add(new Connection
                    {
                        AccessPoint = accessPoint,
                        Mac = FindMac(accessPoint)
                    });
                }
                Connections = newConnections;
                return true;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Connections.Clear();
                return false;
            }
        }

        public bool CheckWifi()
        {
            try
            {
                _wifi.GetAccessPoints();
                return true;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return false;
            }
        }

        private string FindMac(AccessPoint accessPoint)
        {
            bool foundMac = false;
            foreach (var bssid in _bssids)
            {
                if (foundMac)
                {
                    return bssid.Substring(bssid.Length - LengthOfMacString,LengthOfMacString);
                }
                foundMac = (bssid.Split(':')[0].Contains("SSID") && accessPoint.Name.Equals(bssid.Split(':')[1]));
            }
            return null;
        }

        public ListViewItem[] ConnectionsNames
        {
            get
            {
                var connectionsList = new ListViewItem[NumberOfConnections];
                for (int i = 0; i < NumberOfConnections; i++)
                {
                    connectionsList[i] = new ListViewItem(Connections[i].SSID ?? "Hidden connection");
                }
                return connectionsList;
            }
        }

        public ListViewItem[] ConnectionInfo(int index)
        {
            var connection = Connections[index];
            return new ListViewItem[] {
                    new ListViewItem($"Name: {connection.SSID ?? "Hidden connection"}"),
                    new ListViewItem($"Auth Type: {connection.AuthType}"),
                    new ListViewItem($"Mac: {connection.Mac}"),
                    new ListViewItem($"Signal Strength: {connection.SignalStrength}"),
                    new ListViewItem($"Is Secured: {connection.IsSecured}"),
                    new ListViewItem($"Has profile: {connection.HasProfile}"),
                    new ListViewItem($"Is Connected: {connection.IsConnected}")
                };
        }

        public void Connect(int index, string password, Action<bool> onConnectComplite,
            bool remember)
        {
            var connection = Connections[index];
            var authRequest = new AuthRequest(connection.AccessPoint)
            {
                Password = password
            };
            connection.ConnectAsync(authRequest, onConnectComplite, remember);
        }

        public void Disconnect()
        {
           _wifi.Disconnect();
        }
    }
}
