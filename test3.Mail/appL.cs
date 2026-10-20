using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

                if (line.Contains("[ERR]")) { ErrList.Add(ParseErr(line)); }
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

                    (statusCode, message) = ("2000", "");

                    _logO.LogInformation($"CreateSystemLog成功 - StatusCode = {statusCode}, TotalCount = {totalCount}, SkipCount = {skipCount}");
                    _logX.L1();
                }
                else
                {
                    (statusCode, message) = ("2000", "查無須新增紀錄");

                    _logO.LogInformation($"CreateSystemLog完成 - StatusCode = {statusCode}, Message = {message}");
                    _logX.L1();
                }
            }
            catch (Exception ex)
            {
                (statusCode, message) = ("5102", "System Error");

                _logO.LogError(ex, $"CreateSystemLog錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
                _logX.L1();
            }
        }

        public async Task UpdateLog() { }

        public async Task UpdateMsg() { }
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

        private ErrInfo ParseErr(String line)
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

            return Err;
        }
        #endregion
    }
}