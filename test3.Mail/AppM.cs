namespace test3.Mail
{
    public class ErrInfo
    {
        public DateTime ErrTime { get; set; }
        public String? ErrSrc { get; set; }
        public String? StatusCode { get; set; }
        public String? Message { get; set; }
    }

    public class MailInfo
    {
        public String? Name { get; set; }
        public String? Email { get; set; }
    }
}