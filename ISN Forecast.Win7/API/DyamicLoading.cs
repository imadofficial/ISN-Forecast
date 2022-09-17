using ISN_Forecast.Win7.FirstSetup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISN_Forecast.Win7
{
    internal class DyamicLoading
    {
        public static class GlobalStrings
        {
            public static String TempChar;
        }

        public static void Load()
        {
            GetTemp();
            FutureForecasting();
        }

        public static int ReturnTime(int time)
        {
            int resultTime = 0;
            int currTime = DateTime.Now.Hour;

            DateTime today = DateTime.Now;
            double remaining = (today.AddDays(1).Date - today).TotalHours;

            var times = new Dictionary<int, int>()
            {
                {24, 0 },
                {25, 1 },
                {26, 2 },
                {27, 3 },
                {28, 4 },
                {29, 5 },
                {30, 6 },
                {31, 7 },
                {32, 8 },
                {33, 9 },
                {34, 10 },
                {35, 11 },
                {36, 12 },
                {37, 13 },
                {38, 14 },
                {40, 15 },
                {41, 16},
                {42, 17},
                {43, 18},
                {44, 19 },
                {45, 20 },
                {46, 21 },
                {47, 22 },
                {48, 23 },
            };


            if (currTime + time > 23)
            {
                int val;

                times.TryGetValue(currTime + time, out val);
                resultTime = val;
            }
            else
            {
                resultTime = currTime + time;
            }
            return resultTime;
        }

        public static void FutureForecasting()
        {
            var Settings = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("Assets/Settings.json"));
            String Unit = Settings[0]["Unit"];

            if(Unit == "temp_c")
            {
                GlobalStrings.TempChar = "C";
            }
            if (Unit == "temp_f")
            {
                GlobalStrings.TempChar = "F";
            }

            //This is likely the most complicated feature yet. I will prob end up rewriting this feature soon.
            var FutureData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);

            int CurrentHour = DateTime.Now.Hour;
            String TextHour = DateTime.Now.ToString("HH");
            
            NewWeather.Instance.Temp1.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(0)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp2.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(1)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp3.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(2)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp4.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(3)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp5.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(4)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp6.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(5)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp7.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(6)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp8.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(7)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp9.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(8)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp10.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(9)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp11.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(10)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp12.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(11)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp13.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(12)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp14.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(13)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp15.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(14)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp16.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(15)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp17.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(16)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp18.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(17)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp19.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(18)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp20.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(19)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp21.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(20)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp22.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(21)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp23.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(22)][Unit] + "°" + GlobalStrings.TempChar;
            NewWeather.Instance.Temp24.Text = FutureData["forecast"]["forecastday"][0]["hour"][ReturnTime(23)][Unit] + "°" + GlobalStrings.TempChar;

            NewWeather.Instance.Hour1.Text = "Now";
            NewWeather.Instance.Hour2.Text = ReturnTime(1) + ":00";
            NewWeather.Instance.Hour3.Text = ReturnTime(2) + ":00";
            NewWeather.Instance.Hour4.Text = ReturnTime(3) + ":00";
            NewWeather.Instance.Hour5.Text = ReturnTime(4) + ":00";
            NewWeather.Instance.Hour6.Text = ReturnTime(5) + ":00";
            NewWeather.Instance.Hour7.Text = ReturnTime(6) + ":00";
            NewWeather.Instance.Hour8.Text = ReturnTime(7) + ":00";
            NewWeather.Instance.Hour9.Text = ReturnTime(8) + ":00";
            NewWeather.Instance.Hour10.Text = ReturnTime(9) + ":00";
            NewWeather.Instance.Hour11.Text = ReturnTime(10) + ":00";
            NewWeather.Instance.Hour12.Text = ReturnTime(11) + ":00";
            NewWeather.Instance.Hour13.Text = ReturnTime(12) + ":00";
            NewWeather.Instance.Hour14.Text = ReturnTime(13) + ":00";
            NewWeather.Instance.Hour15.Text = ReturnTime(14) + ":00";
            NewWeather.Instance.Hour16.Text = ReturnTime(15) + ":00";
            NewWeather.Instance.Hour17.Text = ReturnTime(16) + ":00";
            NewWeather.Instance.Hour18.Text = ReturnTime(17) + ":00";
            NewWeather.Instance.Hour19.Text = ReturnTime(18) + ":00";
            NewWeather.Instance.Hour20.Text = ReturnTime(19) + ":00";
            NewWeather.Instance.Hour21.Text = ReturnTime(20) + ":00";
            NewWeather.Instance.Hour22.Text = ReturnTime(21) + ":00";
            NewWeather.Instance.Hour23.Text = ReturnTime(23) + ":00";
            NewWeather.Instance.Hour24.Text = ReturnTime(24) + ":00";
        }

        static void GetTemp()
        {
            var WeatherData = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(Weather.FullText);
            var Settings = (dynamic)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("Assets/Settings.json"));

            if (Settings[0]["Unit"] == "temp_c")
            {
                NewWeather.Instance.Type.Text = "°C";
            }
            else
            {
                NewWeather.Instance.Type.Text = "°F";
            }

            String Unit = Settings[0]["Unit"];
            NewWeather.Instance.Temperature.Text = WeatherData["current"][Unit];
        }
    }
}
