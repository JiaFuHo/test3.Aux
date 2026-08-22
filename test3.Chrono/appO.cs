namespace test3.Chrono
{
    public class appO
    {
        #region Fields
        private Int32? _interval = 14;
        #endregion

        #region Properties
        public String? Mode { get; set; } = "A";
        public Int32? Interval
        {
            get => _interval;
            set => _interval = (value == null || value < 14) ? 14 : value;
        }
        #endregion
    }
}