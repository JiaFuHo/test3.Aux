using test3.Dto.Common;

namespace test3.Mail
{
    public class ErrInfo
    {
        public DateTime ErrTime { get; set; }
        public String? ErrSrc { get; set; }
        public String? StatusCode { get; set; }
        public String? Message { get; set; }
    }

    public class MailInfoX
    {
        public String? Name { get; set; }
        public String? Email { get; set; }
    }

    #region Mail
    public class MailQueryRes : QueryResBase
    {
        public IEnumerable<MailInfoY>? MailList { get; set; }
    }

    public class MailInfoY : ErrInfo
    {
        public Int32? ErrId { get; set; }
    }

    public class MailRes : ResBase
    {
        public Int32? TotalCount { get; set; }
    }
    #endregion

    #region Ntfy
    public class NtfyQueryRes : QueryResBase
    {
        public IEnumerable<NtfyInfo>? NtfyList { get; set; }
    }

    public class NtfyInfo
    {
        public Int32? NotificationId { get; set; }
        public String? CName { get; set; }
        public String? CAcc { get; set; }
        public String? Message { get; set; }
    }

    public class NtfyRes : ResBase
    {
        public Int32? TotalCount { get; set; }
    }
    #endregion
}