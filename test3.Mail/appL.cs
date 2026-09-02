using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        #endregion
    }
}