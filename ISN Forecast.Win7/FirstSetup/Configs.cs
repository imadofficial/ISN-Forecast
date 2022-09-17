using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISN_Forecast.Win7.FirstSetup
{
    internal class Configs
    {
        public static String Language;
        public static String Look;
        public static String Translations;
        public static String TimeFormat;
        public static String DateFormat;
        public static String Country;
        public static String AllowAlerts;
        public static String AllowAutoUpdate;
        public static String Unit;
        public static String GPS;
        public static String Percipitation;
        public static String Speed;
        public static String UnitChar;
        public static String AutoHideDate;

        public static String IPInfoKey;
        public static String Weatherkey;
        public static String Meta; //HAHAHAHA MARK ZUCKERBURG
        public static String SetupProcess;

        public static String Latitude;
        public static String Longitude;
    }
    
    internal class AppBehavior
    {
        public static String Hamburger;
        public static String Latitude; public static String Longitude;
    }

    internal class Weather
    {
        public static String FullText;
        public static String Search;
    }

        internal class Version
    {
        public static int Build;
        public static int SubBuild;
    }

    internal class BlackWhite
    {
        public static String BoxColorBlack = "#8A8A8A";
        public static String TextColorBlack = "#FFFFFF";

        public static String BoxColorWhite = "#FFFFFF";
        public static String TextColorWhite = "#000000";
    }
}
