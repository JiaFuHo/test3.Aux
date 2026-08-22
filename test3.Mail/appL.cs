using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using test3.DAL.test3.Context;
using test3.DAL.test3.Models;

namespace test3.Mail
{
    public class appL
    {
        #region Fields
        private readonly appO _opt;
        private readonly ILogger<appL> _logO;
        private readonly test3Context _db;
        #endregion

        #region Constructor
        public appL(ILogger<appL> log, IOptions<appO> opt, test3Context db)
        {
            _logO = log;
            _opt = opt.Value;
            _db = db;
        }
        #endregion

        #region Methods

        #endregion

        #region Aux Methods
        public void SetTitle()
        {
            String title = "test3.Mail";

            Int32 widthTotal = 75;
            Int32 widthTitle = 0;

            foreach (var x in title) { widthTitle += (x > 127) ? 2 : 1; }

            Int32 padT = widthTotal - 2 - widthTitle;

            if (padT < 0) { padT = 0; }

            Int32 padL = padT / 2;
            Int32 padR = padT - padL;

            Console.WriteLine("╔" + new String('═', widthTotal - 2) + "╗");
            Console.WriteLine("║" + new String(' ', padL) + title + new String(' ', padR) + "║");
            Console.WriteLine("╚" + new String('═', widthTotal - 2) + "╝");
        }
        #endregion
    }
}