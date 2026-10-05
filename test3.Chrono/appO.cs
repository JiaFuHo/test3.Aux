namespace test3.Chrono
{
    public class AppO
    {
        #region Fields
        private String? _exeMode = "A";
        private Int32? _interval = 14;
        #endregion

        #region Properties
        public String? ExeMode
        {
            get => _exeMode;
            set { if (value == "A" || value == "M") { _exeMode = value; } }
        }
        public Int32? Interval
        {
            get => _interval;
            set { if (value != null && value >= 14) { _interval = value; } }
        }
        #endregion
    }
}