namespace test3.Test
{
    /// <summary> App邏輯 </summary>
    internal static class AppRunner
    {
        #region Fields
        private static AppSettings? _config = null;
        public static ILogger? _loggerO = null;
        public static ILogger? _loggerX = null;
        #endregion

        #region Methods


        public static void SetTitle()
        {
            String title = "test3.Test";

            Int32 widthTotal = 75;
            Int32 widthTitle = 0;

            foreach (var x in title) { widthTitle += (x > 127) ? 2 : 1; }

            Int32 padT = widthTotal - 2 - widthTitle;

            if (padT < 0) { padT = 0; }

            Int32 padL = padT / 2;
            Int32 padR = padT - padL;

            Console.WriteLine("╔" + new String('═', widthTotal - 2) + "╗");
            Console.WriteLine("║" + new String(' ', padL) + title + new String(' ', padR) + "║");
            Console.WriteLine("╚" + new String('═', widthTotal - 2) + "╝");
        }
        #endregion
    }
}