using Microsoft.Extensions.Options;

namespace test3.Mail
{
    public class AppC
    {
        #region Fields
        private readonly AppL _logic;
        private readonly AppO _opt;
        #endregion

        #region Constructor
        public AppC(AppL logic, IOptions<AppO> opt)
        {
            _logic = logic;
            _opt = opt.Value;
        }
        #endregion

        public async Task Run()
        {
            if (String.IsNullOrWhiteSpace(_opt.MailAcc)) { throw new Exception("System Para Error: MailAcc"); }
            if (String.IsNullOrWhiteSpace(_opt.MailPwd)) { throw new Exception("System Para Error: MailPwd"); }
            if (String.IsNullOrWhiteSpace(_opt.MailHost)) { throw new Exception("System Para Error: MailHost"); }
            if (_opt.MailList is not { Count: > 0 }) { throw new Exception("System Para Error: MailList"); }
            if (String.IsNullOrWhiteSpace(_opt.LogPathS)) { throw new Exception("System Para Error: LogPathS"); }

            _logic.SetTitle();

            var ErrList = await _logic.GetErrList();

            if (ErrList.Count > 0) { await _logic.CreateLog(ErrList); }

            var MailRes = await _logic.GetMailList();
            var NtfyRes = await _logic.GetNtfyList();

            if (MailRes.Status)
            {
                var status = await _logic.Mail(MailRes.MailList!.ToList());

                if (status) { await _logic.UpdateLog(MailRes.MailList!.Select(x => (Int32)x.ErrId!).ToList()); }
            }

            if (NtfyRes.Status)
            {
                var status = await _logic.Ntfy(NtfyRes.NtfyList!.ToList());

                if (status) { await _logic.UpdateMsg(NtfyRes.NtfyList!.Select(x => (Int32)x.NotificationId!).ToList()); }
            }
        }
    }
}