using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facts
{
    static class AppHelper
    {
        public static int MAXSIZEBASEDB = 200;
        private static int _pageIndex;

        static AppHelper()
        {
            Storage = IsolatedStorageSettings.ApplicationSettings;
        }

        public static IsolatedStorageSettings Storage { get; set; }

        // Flags
        public static bool IsRate { get; set; }
        public static bool AppBar { get; set; }
        public static bool IsTrial { get; set; }
        public static bool TextStyle { get; set; }

        // Page index
        public static int PageIndex
        {
            set { if (value <= MAXSIZEBASEDB && value > -1) _pageIndex = value; }
            get { return _pageIndex; }
        }

    }
}
