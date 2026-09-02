using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using test3.DAL.test3.Context;
using test3.DAL.test3.Models;

namespace test3.Chrono
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
        public async Task<(Boolean status, DateTime SDate)> ChkStatus()
        {
            var SDate = await _db.SystemTimes.OrderByDescending(x => x.Cdate).Select(x => x.Cdate).FirstOrDefaultAsync();
            var CDate = DateTime.Today;

            var status = !(SDate == CDate);

            return (status, SDate);
        }

        public async Task<Boolean> UpdateBorrow(DateTime SDate)
        {
            var (Status, StatusCode, Message) = (false, "", "");

            var CDate = DateTime.Today;

            var Interval = (_opt.Mode == "A") ? (CDate - SDate).Days : (Int32)_opt.Interval!;

            try
            {
                var updateList = await _db.Borrows.ToListAsync();

                foreach (var item in updateList)
                {
                    item.BorrowDate = item.BorrowDate.AddDays(Interval);
                    item.DueDateB = item.DueDateB.AddDays(Interval);
                    item.ReturnDate = (item.ReturnDate.HasValue) ? item.ReturnDate.Value.AddDays(Interval) : null;
                }

                await _db.SaveChangesAsync();

                (Status, StatusCode, Message) = (true, "2000", "");
            }
            catch (Exception ex)
            {
                (Status, StatusCode, Message) = (false, "5102", $"System Error: {ex.Message}");

                _logO.LogError(ex, $"UpdateBorrow錯誤 - StatusCode = {StatusCode}, Message = {Message}, ex = ");
            }

            return Status;
        }

        public async Task<Boolean> UpdateNotification(DateTime SDate)
        {
            var (Status, StatusCode, Message) = (false, "", "");

            var CDate = DateTime.Today;

            var Interval = (_opt.Mode == "A") ? (CDate - SDate).Days : (Int32)_opt.Interval!;

            try
            {
                var updateList = await _db.Notifications.ToListAsync();

                foreach (var item in updateList)
                {
                    item.NotificationDate = item.NotificationDate.AddDays(Interval);
                }

                await _db.SaveChangesAsync();

                (Status, StatusCode, Message) = (true, "2000", "");
            }
            catch (Exception ex)
            {
                (Status, StatusCode, Message) = (false, "5102", $"System Error: {ex.Message}");

                _logO.LogError(ex, $"UpdateNotification錯誤 - StatusCode = {StatusCode}, Message = {Message}, ex = ");
            }

            return Status;
        }

        public async Task<Boolean> UpdateReservation(DateTime SDate)
        {
            var (Status, StatusCode, Message) = (false, "", "");

            var CDate = DateTime.Today;

            var Interval = (_opt.Mode == "A") ? (CDate - SDate).Days : (Int32)_opt.Interval!;

            try
            {
                var updateList = await _db.Reservations.ToListAsync();

                foreach (var item in updateList)
                {
                    item.ReservateDate = item.ReservateDate.AddDays(Interval);
                    item.DueDateR = (item.DueDateR.HasValue) ? item.DueDateR.Value.AddDays(Interval) : null;
                }

                await _db.SaveChangesAsync();

                (Status, StatusCode, Message) = (true, "2000", "");
            }
            catch (Exception ex)
            {
                (Status, StatusCode, Message) = (false, "5102", $"System Error: {ex.Message}");

                _logO.LogError(ex, $"UpdateReservation錯誤 - StatusCode = {StatusCode}, Message = {Message}, ex = ");
            }

            return Status;
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