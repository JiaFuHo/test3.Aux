namespace test3.Test
{
    public class AppO
    {
        #region Fields
        private String? _testURL = "http://localhost:4200";
        #endregion

        #region Properties
        public String? TestURL
        {
            get => _testURL;
            set { if (!String.IsNullOrWhiteSpace(value)) { _testURL = value; } }
        }
        public String? TestAcc { get; set; }
        public String? TestPwd { get; set; }
        #endregion
    }
}