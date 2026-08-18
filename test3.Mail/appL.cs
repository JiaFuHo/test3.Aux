using Microsoft.Extensions.Logging;
using test3.DAL.test3.Context;

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
        public appL(appO opt, ILogger<appL> log, test3Context db)
        {
            _opt = opt;
            _logO = log;
            _db = db;
        }
        #endregion

        #region Methods

        #endregion
    }
}