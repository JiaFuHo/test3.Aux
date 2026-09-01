using Microsoft.Extensions.Logging;
using test3.Common;

namespace test3.Chrono
{
    public class AppC
    {
        #region Fields
        private readonly AppL _logic;
        private readonly ILogger<AppC> _logO;
        #endregion

        #region Constructor
        public AppC(AppL logic, ILogger<AppC> log)
        {
            _logic = logic;
            _logO = log;
        }
        #endregion

        public async Task Run()
        {
            _logic.SetTitle();

            var (status, SDate) = await _logic.ChkStatus();

            if (status)
            {
                var statusB = await _logic.UpdateBorrow(SDate);
                var statusN = await _logic.UpdateNotification(SDate);
                var statusR = await _logic.UpdateReservation(SDate);

                if (statusB && statusN)
                {
                    await _logic.UpdateSystemTime();

                    _logO.LogInformation("Chrono更新成功");
                    _logX.L1();
                }
            }
            else
            {
                _logO.LogInformation("Chrono未更新，查有重複紀錄");
                _logX.L1();
            }
        }
    }
}