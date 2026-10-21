using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using test3.Common;

namespace test3.Chrono
{
    public class AppC
    {
        #region Fields
        private readonly AppL _logic;
        private readonly AppO _opt;
        private readonly ILogger<AppC> _logO;
        #endregion

        #region Constructor
        public AppC(AppL logic, IOptions<AppO> opt, ILogger<AppC> log)
        {
            _logic = logic;
            _opt = opt.Value;
            _logO = log;
        }
        #endregion

        public async Task Run()
        {
            _logic.SetTitle();

            var (status, interval) = await _logic.ChkStatus();

            if (status)
            {
                var (statusR, statusB, statusN) = (false, false, false);

                if (_opt.ExeMode == "A")
                {
                    statusR = await _logic.UpdateReservation(interval);
                    statusB = await _logic.UpdateBorrow(interval);
                    statusN = await _logic.UpdateNotification(interval);
                }
                else
                {
                    statusR = await _logic.UpdateReservation((Int32)_opt.Interval!);
                    statusB = await _logic.UpdateBorrow((Int32)_opt.Interval!);
                    statusN = await _logic.UpdateNotification((Int32)_opt.Interval!);
                }

                if (statusR && statusB && statusN)
                {
                    await _logic.UpdateSystemTime();

                    _logO.LogInformation("Chrono更新成功");
                }
            }
            else { _logO.LogWarning("Chrono未更新，查有重複紀錄"); }
        }
    }
}