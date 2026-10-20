using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using test3.DAL.test3.Context;
using test3.DAL.test3.Models;

namespace test3.Chrono
{
    public class AppL
    {
        #region Fields
        private readonly test3Context _db;
        private readonly ILogger<AppL> _logO;
        #endregion

        #region Constructor
        public AppL(test3Context db, ILogger<AppL> log)
        {
            _db = db;
            _logO = log;
        }
        #endregion

        #region Methods
        public async Task<(Boolean status, Int32 interval)> ChkStatus()
        {
            var SDate = await _db.SystemTimes.OrderByDescending(x => x.Cdate).Select(x => x.Cdate).FirstOrDefaultAsync();
            var CDate = DateTime.Today;

            var (status, interval) = (!(SDate == CDate), (CDate - SDate.Date).Days);

            return (status, interval);
        }

        public async Task<Boolean> UpdateReservation(Int32 interval)
        {
            var (status, statusCode, message) = (false, "", "");

            try
            {
                var updateList = await _db.Reservations.ToListAsync();

                foreach (var item in updateList)
                {
                    item.ReservateDate = item.ReservateDate.AddDays(interval);
                    item.DueDateR = (item.DueDateR.HasValue) ? item.DueDateR.Value.AddDays(interval) : null;
                    item.Stime = DateTime.Now;
                }

                await _db.SaveChangesAsync();

                (status, statusCode, message) = (true, "2000", "");
            }
            catch (Exception ex)
            {
                (status, statusCode, message) = (false, "5102", "System Error");

                _logO.LogError(ex, $"UpdateReservation錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
            }

            return status;
        }

        public async Task<Boolean> UpdateBorrow(Int32 interval)
        {
            var (status, statusCode, message) = (false, "", "");

            try
            {
                var updateList = await _db.Borrows.ToListAsync();

                foreach (var item in updateList)
                {
                    item.BorrowDate = item.BorrowDate.AddDays(interval);
                    item.DueDateB = item.DueDateB.AddDays(interval);
                    item.ReturnDate = (item.ReturnDate.HasValue) ? item.ReturnDate.Value.AddDays(interval) : null;
                    item.Stime = DateTime.Now;
                }

                await _db.SaveChangesAsync();

                (status, statusCode, message) = (true, "2000", "");
            }
            catch (Exception ex)
            {
                (status, statusCode, message) = (false, "5102", "System Error");

                _logO.LogError(ex, $"UpdateBorrow錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
            }

            return status;
        }

        public async Task<Boolean> UpdateNotification(Int32 interval)
        {
            var (status, statusCode, message) = (false, "", "");

            try
            {
                var updateList = await _db.Notifications.ToListAsync();

                foreach (var item in updateList)
                {
                    item.NotificationDate = item.NotificationDate.AddDays(interval);
                    item.IsRead = false;
                    item.Stime = DateTime.Now;
                }

                await _db.SaveChangesAsync();

                (status, statusCode, message) = (true, "2000", "");
            }
            catch (Exception ex)
            {
                (status, statusCode, message) = (false, "5102", "System Error");

                _logO.LogError(ex, $"UpdateNotification錯誤 - StatusCode = {statusCode}, Message = {message}: {ex.Message}, ex = ");
            }

            return status;
        }

        public async Task UpdateSystemTime()
        {
            var create = new SystemTime { Cdate = DateTime.Today };

            await _db.SystemTimes.AddAsync(create);
            await _db.SaveChangesAsync();
        }
        #endregion

        #region Aux Methods
        public void SetTitle()
        {
            var title = "test3.Chrono";

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
        #endregion
    }
}