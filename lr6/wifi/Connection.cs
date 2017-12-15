using System;
using SimpleWifi;


namespace lab6
{
    class Connection
    {
        private AccessPoint accessPoint;
        public string Mac { get; set; }
        public AccessPoint AccessPoint
        {
            get
            {
                return accessPoint;
            }
            set
            {
                accessPoint = value;
            }

        }

        public Connection()
        {

        }

        public Connection(AccessPoint AccessPoint, string Mac)
        {
            this.accessPoint = AccessPoint;
            this.Mac = Mac;
        }

        public string SSID
        {
            get
            {
                return accessPoint.Name.Equals("") ? null : accessPoint.Name;
            }
        }
        public int SignalStrength
        {
            get
            {
                return (int)accessPoint.SignalStrength;
            }
        }
        public bool IsSecured
        {
            get
            {
                return accessPoint.IsSecure;
            }
        }
        public bool HasProfile
        {
            get
            {
                return accessPoint.HasProfile;
            }
        }
        public bool IsConnected
        {
            get
            {
                return accessPoint.IsConnected;
            }
        }
        public string AuthType
        {
            get
            {
                var cipherAlgorithm = accessPoint.ToString().Split()[10];
                var authAlgorithm = accessPoint.ToString().Split()[6];
                switch (cipherAlgorithm)
                {
                    case "None":
                        return "Open";
                    case "Wep":
                        return "Wep";
                    case "CCMP":
                    case "TKIP":
                        return (authAlgorithm.Equals("RSNA") ? "WPA2-Enterprise-PEAP-MSCHAPv2" : "WPA2-PSK");
                    default:
                        return "Unknown";
                }
            }
        }

        public void ConnectAsync(AuthRequest authRequest, Action<bool> onConnectComplete, bool remember)
        {
            accessPoint.ConnectAsync(authRequest, !remember, onConnectComplete);
        }
    }
}
