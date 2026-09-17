namespace test3.Chrono
{
    public class AppO
    {
        #region Fields
        private String? _mode = "A";
        private Int32? _interval = 14;
        #endregion

        #region Properties
        public String? Mode
        {
            get => _mode;
            set { if (value == "A" || value == "M") { _mode = value; } }
        }
        public Int32? Interval
        {
            get => _interval;
            set { if (value != null && value >= 14) { _interval = value; } }
        }
        #endregion
    }
}