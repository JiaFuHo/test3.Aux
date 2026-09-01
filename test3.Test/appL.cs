using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using test3.DAL.test3.Context;

namespace test3.Test
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
            String title = "test3.Test";

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