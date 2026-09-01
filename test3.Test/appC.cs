using Microsoft.Extensions.Logging;

namespace test3.Test
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


        }
    }
}