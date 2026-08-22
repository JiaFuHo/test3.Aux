using Microsoft.Extensions.Logging;

namespace test3.Test
{
    public class appC
    {
        #region Fields
        private readonly appL _logic;
        private readonly ILogger<appC> _logO;
        #endregion

        #region Constructor
        public appC(appL logic, ILogger<appC> log)
        {
            _logic = logic;
            _logO = log;
        }
        #endregion

        public async Task Run()
        {
            _logic.SetTitle();


        }
    }
}