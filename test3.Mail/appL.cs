using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using test3.Common;
using test3.DAL.test3.Context;
using test3.DAL.test3.Models;

namespace test3.Mail
{
    public class AppL
    {
        #region Fields
        private readonly test3Context _db;
        private readonly AppO _opt;
        private readonly ILogger<AppL> _logO;
        #endregion

        #region Constructor
        public AppL(test3Context db, IOptions<AppO> opt, ILogger<AppL> log)
        {
            _db = db;
            _opt = opt.Value;
            _logO = log;
        }
        #endregion

        #region Methods
        public async Task<List<ErrInfo>> GetErrList()
        {
            var fileName = $"Log_{DateTime.Today:yyyyMMdd}.txt";
            var filePath = Path.Combine(_opt.LogPathS!, fileName);

            var ErrList = new List<ErrInfo>();

            if (!File.Exists(filePath)) { return ErrList; }

            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);

            while (true)
            {
                var line = await reader.ReadLineAsync();

                if (line == null) { break; }

                if (line.Contains("[ERR]"))
                {
                    var Err = ParseErr(line);

                    if (Err != null) { ErrList.Add(Err); }
                }
            }

            if (ErrList.Count > 0)
            {
                _logO.LogInformation($"GetErrList成功 - TotalCount = {ErrList.Count}");
                _logX.L1();
            }
            else
            {
                _logO.LogInformation($"GetErrList完成 - Message = 查無相關紀錄");
                _logX.L1();
            }

            return ErrList;
        }

        public async Task CreateLog(List<ErrInfo> ErrList)
        {
            var (statusCode, message, totalCount, skipCount) = ("", "", ErrList.Count, 0);

            try
            {
                foreach (var x in ErrList)
                {
                    var createChk = await _db.SystemLogs.AnyAsync(y => y.ErrTime == x.ErrTime && y.ErrSrc == x.ErrSrc);

                    if (createChk) { skipCount++; continue; }

                    var create = new SystemLog
                    {
                        ErrTime = x.ErrTime,
                        ErrSrc = x.ErrSrc!,
                        StatusCode = x.StatusCode!,
                        Message = x.Message!,
                        Ctime = DateTime.Now
                    };

                    _db.SystemLogs.Add(create);
                }

                if (totalCount - skipCount > 0)
                {
                    await _db.SaveChangesAsync();

                    (statusCode, message) = ("2000", "新增成功");

                    _logO.LogInformation($"CreateLog成功 - StatusCode = {statusCode}, TotalCount = {totalCount}, SkipCount = {skipCount}");
                    _logX.L1();
                }
                else
                {
                    (statusCode, message) = ("4004", "查無須新增紀錄");

                    _logO.LogInformation($"CreateLog完成 - StatusCode = {statusCode}, Message = {message}");
                    _logX.L1();
                }
            }
            catch (Exception ex)
            {
                (statusCode, message) = ("5102", "System Error");

                _logO.LogError(ex, $"CreateLog錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
                _logX.L1();
            }
        }

        public async Task<MailQueryRes> GetMailList()
        {
            var Res = new MailQueryRes();

            var querySrc = _db.SystemLogs.Where(x => x.IsMailed == false);

            if (!await querySrc.AnyAsync())
            {
                Res.Status = false;
                Res.StatusCode = "4004";
                Res.Message = "查無相關紀錄";

                _logO.LogInformation($"GetMailList完成 - StatusCode = {Res.StatusCode}, Message = {Res.Message}");
                _logX.L1();

                return Res;
            }

            try
            {
                var query = querySrc.Select(x => new MailInfoY
                {
                    ErrId = x.ErrId,
                    ErrTime = x.ErrTime,
                    ErrSrc = x.ErrSrc,
                    StatusCode = x.StatusCode,
                    Message = x.Message
                });

                var MailList = await query.ToListAsync();

                Res.Status = true;
                Res.StatusCode = "2000";
                Res.Message = "查詢成功";
                Res.TotalCount = MailList.Count;
                Res.MailList = MailList;

                _logO.LogInformation($"GetMailList成功 - StatusCode = {Res.StatusCode}, TotalCount = {Res.TotalCount}");
            }
            catch (Exception ex)
            {
                Res.Status = false;
                Res.StatusCode = "5102";
                Res.Message = "System Error";

                _logO.LogError(ex, $"GetMailList錯誤 - StatusCode = {Res.StatusCode}, Message = {Res.Message}: {ex.Message}, ex = ");
                _logX.L1();
            }

            return Res;
        }

        public async Task<NtfyQueryRes> GetNtfyList()
        {
            var Res = new NtfyQueryRes();

            var querySrc = _db.Notifications.Where(x => x.IsNotified == false);

            if (!await querySrc.AnyAsync())
            {
                Res.Status = false;
                Res.StatusCode = "4004";
                Res.Message = "查無相關紀錄";

                _logO.LogInformation($"GetNtfyList完成 - StatusCode = {Res.StatusCode}, Message = {Res.Message}");
                _logX.L1();

                return Res;
            }

            try
            {
                var query = querySrc.Select(x => new NtfyInfo
                {
                    NotificationId = x.NotificationId,
                    CName = x.CidNavigation.Cname,
                    CAcc = x.CidNavigation.Cacc,
                    Message = x.Message
                });

                var NtfyList = await query.ToListAsync();

                Res.Status = true;
                Res.StatusCode = "2000";
                Res.Message = "查詢成功";
                Res.TotalCount = NtfyList.Count;
                Res.NtfyList = NtfyList;

                _logO.LogInformation($"GetNtfyList成功 - StatusCode = {Res.StatusCode}, TotalCount = {Res.TotalCount}");
            }
            catch (Exception ex)
            {
                Res.Status = false;
                Res.StatusCode = "5102";
                Res.Message = "System Error";

                _logO.LogError(ex, $"GetNtfyList錯誤 - StatusCode = {Res.StatusCode}, Message = {Res.Message}: {ex.Message}, ex = ");
                _logX.L1();
            }

            return Res;
        }

        public async Task<Boolean> Mail(List<MailInfoY> MailList)
        {
            var (status, statusCode, message, totalCount) = (false, "", "", MailList.Count);

            try
            {
                using var smtp = new SmtpClient { CheckCertificateRevocation = false };

                await smtp.ConnectAsync(_opt.MailHost!, (Int32)_opt.MailPort!, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_opt.MailAcc!, _opt.MailPwd!);

                foreach (var x in MailList)
                {
                    var email = new MimeMessage
                    {
                        From = { new MailboxAddress(_opt.MailName!, _opt.MailAcc!) },
                        Subject = "【test3.Mail】系統信",
                    };

                    foreach (var y in _opt.MailList) { email.To.Add(new MailboxAddress(y.Name!, y.Email!)); }

                    email.Body = new BodyBuilder
                    {
                        HtmlBody = $@"<div style='text-align: center; margin-top: 1rem'>
                                                        <h1 style='margin: 0'>【test3.Service】</h1>
                                                    </div>

                                                    <div style='padding: 20px 0'>
                                                        <table style='border-collapse: collapse; width: 100%; max-width: 500px; margin: 0 auto'>
                                                        <tr>
                                                            <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>錯誤時間</th>
                                                            <td style='padding: 12px; border: 1px solid #d9d9d9'>{x.ErrTime:yyyy/MM/dd HH:mm:ss}</td>
                                                        </tr>
                                                        <tr>
                                                            <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>錯誤來源</th>
                                                            <td style='padding: 12px; border: 1px solid #d9d9d9'>{x.ErrSrc}</td>
                                                        </tr>
                                                        <tr>
                                                            <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>錯誤狀態</th>
                                                            <td style='padding: 12px; border: 1px solid #d9d9d9'>{x.StatusCode}</td>
                                                        </tr>
                                                        <tr>
                                                            <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>錯誤訊息</th>
                                                            <td style='padding: 12px; border: 1px solid #d9d9d9'>{x.Message}</td>
                                                        </tr>
                                                        <tr>
                                                            <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>通知時間</th>
                                                            <td style='padding: 12px; border: 1px solid #d9d9d9'>{DateTime.Now:yyyy/MM/dd HH:mm:ss}</td>
                                                        </tr>
                                                        </table>
                                                    </div>

                                                    <div style='text-align: center; color:#999'>
                                                        <p>本郵件由系統自動發送，請勿直接回覆。</p>
                                                    </div>"
                    }.ToMessageBody();

                    await smtp.SendAsync(email);
                }

                await smtp.DisconnectAsync(true);

                (status, statusCode, message) = (true, "2000", "發信成功");

                _logO.LogInformation($"Mail成功 - StatusCode = {statusCode}, TotalCount = {totalCount}");
            }
            catch (Exception ex)
            {
                (status, statusCode, message) = (false, "5001", "Network Error");

                _logO.LogError(ex, $"Mail錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
                _logX.L1();
            }

            return status;
        }

        public async Task<Boolean> Ntfy(List<NtfyInfo> NtfyList)
        {
            var (status, statusCode, message, totalCount) = (false, "", "", NtfyList.Count);

            try
            {
                using var smtp = new SmtpClient { CheckCertificateRevocation = false };

                await smtp.ConnectAsync(_opt.MailHost!, (Int32)_opt.MailPort!, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_opt.MailAcc!, _opt.MailPwd!);

                foreach (var x in NtfyList)
                {
                    var email = new MimeMessage
                    {
                        From = { new MailboxAddress(_opt.MailName!, _opt.MailAcc!) },
                        Subject = "【林間書語】系統信",
                        To = { new MailboxAddress(x.CName!, x.CAcc!) },
                        Body = new BodyBuilder
                        {
                            HtmlBody = $@"<div style='text-align: center; margin-top: 1rem'>
                                                            <h1 style='margin: 0'>【林間書語】</h1>
                                                        </div>

                                                        <div style='padding: 20px 0'>
                                                            <table style='border-collapse: collapse; width: 100%; max-width: 500px; margin: 0 auto'>
                                                            <tr>
                                                                <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>通知訊息</th>
                                                                <td style='padding: 12px; border: 1px solid #d9d9d9'>{x.Message}</td>
                                                            </tr>
                                                            <tr>
                                                                <th style='padding: 12px; border: 1px solid #d9d9d9; font-weight: bold; width: 30%'>通知時間</th>
                                                                <td style='padding: 12px; border: 1px solid #d9d9d9'>{DateTime.Now:yyyy/MM/dd HH:mm:ss}</td>
                                                            </tr>
                                                            </table>
                                                        </div>

                                                        <div style='text-align: center; color:#999'>
                                                            <p>本郵件由系統自動發送，請勿直接回覆。</p>
                                                        </div>"
                        }.ToMessageBody()
                    };

                    await smtp.SendAsync(email);
                }

                await smtp.DisconnectAsync(true);

                (status, statusCode, message) = (true, "2000", "通知成功");

                _logO.LogInformation($"Ntfy成功 - StatusCode = {statusCode}, TotalCount = {totalCount}");
            }
            catch (Exception ex)
            {
                (status, statusCode, message) = (false, "5001", "Network Error");

                _logO.LogError(ex, $"Ntfy錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
                _logX.L1();
            }

            return status;
        }

        public async Task UpdateLog(List<Int32> IdList)
        {
            var (statusCode, message) = ("", "");

            var updateSrc = _db.SystemLogs.Where(x => IdList.Contains(x.ErrId));

            if (!await updateSrc.AnyAsync())
            {
                (statusCode, message) = ("4004", "查無相關紀錄");

                _logO.LogWarning($"UpdateLog失敗 - StatusCode = {statusCode}, Message = {message}");
                _logX.L1();

                return;
            }

            try
            {
                var updateList = await updateSrc.ToListAsync();

                foreach (var x in updateList)
                {
                    x.IsMailed = true;
                    x.Mtime = DateTime.Now;
                }

                await _db.SaveChangesAsync();

                (statusCode, message) = ("2000", "更新成功");

                _logO.LogInformation($"UpdateLog成功 - StatusCode = {statusCode}");
                _logX.L1();
            }
            catch (Exception ex)
            {
                (statusCode, message) = ("5102", "System Error");

                _logO.LogError(ex, $"UpdateLog錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
                _logX.L1();
            }
        }

        public async Task UpdateMsg(List<Int32> IdList)
        {
            var (statusCode, message) = ("", "");

            var updateSrc = _db.Notifications.Where(x => IdList.Contains(x.NotificationId));

            if (!await updateSrc.AnyAsync())
            {
                (statusCode, message) = ("4004", "查無相關紀錄");

                _logO.LogWarning($"UpdateMsg失敗 - StatusCode = {statusCode}, Message = {message}");

                return;
            }

            try
            {
                var updateList = await updateSrc.ToListAsync();

                foreach (var x in updateList)
                {
                    x.IsNotified = true;
                    x.Ntime = DateTime.Now;
                }

                await _db.SaveChangesAsync();

                (statusCode, message) = ("2000", "更新成功");

                _logO.LogInformation($"UpdateMsg成功 - StatusCode = {statusCode}");
            }
            catch (Exception ex)
            {
                (statusCode, message) = ("5102", "System Error");

                _logO.LogError(ex, $"UpdateMsg錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
            }
        }
        #endregion

        #region Aux Methods
        public void SetTitle()
        {
            var title = "test3.Mail";

            var widthTotal = 75;
            var widthTitle = 0;

            foreach (var x in title) { widthTitle += (x > 127) ? 2 : 1; }

            var padT = widthTotal - 2 - widthTitle;

            if (padT < 0) { padT = 0; }

            var padL = padT / 2;
            var padR = padT - padL;

            Console.WriteLine("╔" + new String('═', widthTotal - 2) + "╗");
            Console.WriteLine("║" + new String(' ', padL) + title + new String(' ', padR) + "║");
            Console.WriteLine("╚" + new String('═', widthTotal - 2) + "╝");
        }

        private ErrInfo? ParseErr(String line)
        {
            var Err = new ErrInfo();

            var regex = @"^(?<ErrTime>\d{2}:\d{2}) \[ERR\] (?<ErrSrc>.+?)錯誤 - StatusCode = (?<StatusCode>\d+), Message = (?<Message>.*?)(?:, ex =)?$";

            var match = System.Text.RegularExpressions.Regex.Match(line, regex);

            if (match.Success)
            {
                Err.ErrTime = DateTime.Today.Add(TimeSpan.Parse(match.Groups["ErrTime"].Value));
                Err.ErrSrc = match.Groups["ErrSrc"].Value;
                Err.StatusCode = match.Groups["StatusCode"].Value;
                Err.Message = match.Groups["Message"].Value;
            }
            else { return null; }

            return Err;
        }
        #endregion
    }
}