namespace test3.Mail
{
    public class AppO
    {
        #region Fields
        private String? _mailName = "test3 Mail";
        private Int32? _mailPort = 465;
        #endregion

        #region Properties
        public String? MailName
        {
            get => _mailName;
            set { if (!String.IsNullOrWhiteSpace(value)) { _mailName = value; } }
        }
        public String? MailHost { get; set; }
        public Int32? MailPort
        {
            get => _mailPort;
            set { if (value == 465 || value == 587) { _mailPort = value; } }
        }
        public String? MailAcc { get; set; }
        public String? MailPwd { get; set; }
        public List<MailInfoX> MailList { get; set; } = new();
        public String? LogPathS { get; set; }
        #endregion
    }
}