using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ISN_Forecast.Win7
{
    internal class FutureForecasting
    {
        public void StartFunction()
        {
            string[] Days = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
        }

        public void DownloadFile()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringAsync(new Uri("https://api.openweathermap.org/data/2.5/forecast?lat="+ Configs.Latitude + "&lon=" + Configs.Longitude + "&appid=3a527f6b49287d153185316c83efdc9a"));
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(ProcessData);
        }

        private void ProcessData(object sender, DownloadStringCompletedEventArgs e)
        {

        }
    }
}
