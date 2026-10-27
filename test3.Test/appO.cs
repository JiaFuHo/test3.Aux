namespace test3.Test
{
    public class AppO
    {
        #region Fields
        private String? _testURL = "http://localhost:4200";
        private String? _testTypeId = "2";
        #endregion

        #region Properties
        public String? TestURL
        {
            get => _testURL;
            set { if (!String.IsNullOrWhiteSpace(value)) { _testURL = value; } }
        }
        public String? TestAcc { get; set; }
        public String? TestPwd { get; set; }

        public String? TestTypeId
        {
            get => _testTypeId;
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && Int32.TryParse(value, out Int32 valueX))
                {
                    if (valueX >= 1 && valueX <= 7) { _testTypeId = value; }
                }
            }
        }
        #endregion
    }
}