using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.ComponentModel;
using System.Windows;
using System.Device.Location;

namespace ISN_Forecast.Win7.API
{
    internal class ObtainGPS
    {
        public static void main()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            watcher.TryStart(false, TimeSpan.FromSeconds(10));
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                MessageBox.Show("Lat: " + coord.Latitude + ", Long: " + coord.Longitude);
            }
            else
            {
                MessageBox.Show("Was not able to locate you through GPS. Reverting to IP-Lookup...");
            }
        }

        public static void longitude()
        {
            GeoCoordinate coord = new GeoCoordinate();
        }
    }
}
