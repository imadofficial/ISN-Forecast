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

        public static String IPInfoKey;
        public static String Weatherkey;
        public static String Meta; //HAHAHAHA MARK ZUCKERBURG
        public static String SetupProcess;
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

    internal class UpdatesFound
    {
        public static int Build;
        public static int SubBuild;

        public static String Name;
        public static String Size;
        public static String Description;
    }
}
