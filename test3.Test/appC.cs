using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

            if (String.IsNullOrWhiteSpace(_opt.TestTypeId)) { throw new Exception("System Para Error: TestTypeId"); }

            var testPlan = _logic.GetPlan();

            while (true)
            {
                var testSuite = _logic.GetSuite(testPlan);

                if (testSuite == null) { continue; }

                if (testSuite.Any(x => !String.IsNullOrWhiteSpace(x.Attr!.Suite)))
                {
                    if (testSuite.Any(x => !String.IsNullOrWhiteSpace(x.Attr!.Feature)))
                    {
                        while (true)
                        {
                            var testFeature = _logic.GetFeature(testSuite);

                            if (testFeature == null) { break; }

                            while (true)
                            {
                                var testCase = _logic.GetCase(testFeature);

                                if (testCase == null) { break; }

                                _logic.Test(testCase);
                            }
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            var testCase = _logic.GetCase(testSuite);

                            if (testCase == null) { break; }

                            _logic.Test(testCase);
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        var testScenario = _logic.GetScenario(testSuite);

                        if (testScenario == null) { break; }

                        _logic.Test(testScenario);
                    }
                }
            }
        }
    }
}