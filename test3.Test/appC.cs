using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using test3.Common;

namespace test3.Test
{
    public class AppC
    {
        #region Fields
        private readonly AppL _logic;
        private readonly AppO _opt;
        private readonly ILogger<AppC> _logO;
        #endregion

        #region Constructor
        public AppC(AppL logic, IOptions<AppO> opt, ILogger<AppC> log)
        {
            _logic = logic;
            _opt = opt.Value;
            _logO = log;
        }
        #endregion

        public async Task Run()
        {
            if (String.IsNullOrWhiteSpace(_opt.TestAcc)) { throw new Exception("System Para Error: TestAcc"); }
            if (String.IsNullOrWhiteSpace(_opt.TestPwd)) { throw new Exception("System Para Error: TestPwd"); }

            _logic.SetTitle();

            while (true)
            {
                var input = _logic.IptChk("123");

                if (input == "Q") { break; }
                else
                {
                    try
                    {
                        _logic.Setup();
                        _logic.Login();

                        _logic.PilotToInfo();
                        //_logic.DeleteFav();
                        _logic.CreateFav();

                        Console.WriteLine("\n按任意鍵繼續...");
                        Console.ReadKey();

                        _logX.L1();
                    }
                    finally { _logic.Teardown(); }
                }
            }
        }
    }
}