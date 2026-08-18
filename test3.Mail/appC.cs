using Microsoft.Extensions.Logging;

namespace test3.Mail
{
    public class appC
    {
        #region Fields
        private readonly appL _logic;
        private readonly ILogger<appL> _logO;
        #endregion

        #region Constructor
        public appC(appL logic, ILogger<appL> log)
        {
            _logic = logic;
            _logO = log;
        }
        #endregion

        public async Task Run()
        {

        }
    }
}