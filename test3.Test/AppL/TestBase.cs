using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Reflection;
using test3.DAL.test3.Context;

namespace test3.Test
{
    public partial class AppL
    {
        #region Fields
        private readonly test3Context _db;
        private readonly AppO _opt;
        private readonly ILogger<AppL> _logO;

        private IWebDriver? _driver;
        private WebDriverWait? _wait;
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

        #region TestBase
        private void Confirm(Boolean status)
        {
            var alert = _wait!.Until(ExpectedConditions.AlertIsPresent());

            if (status) { alert.Accept(); }
            else { alert.Dismiss(); }
        }

        private void LoginG()
        {
            _driver!.FindElement(By.Id("btnLGX")).Click();

            Thread.Sleep(1000);

            _driver!.FindElement(By.Id("ipt_Acc")).SendKeys(_opt.TestAcc!);
            _driver!.FindElement(By.Id("ipt_Pwd")).SendKeys(_opt.TestPwd!);
            _driver!.FindElement(By.Id("btnLG")).Click();

            Thread.Sleep(1000);
        }

        private void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_opt.TestURL!);
        }

        private void Teardown()
        {
            try { _driver!.Quit(); _driver!.Dispose(); }
            finally { _driver = null; _wait = null; }
        }
        #endregion

        #region TestRunner
        public List<TestInfo> GetPlan()
        {
            var testPlan = typeof(AppL).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                                         .Where(x => x.GetCustomAttribute<TestAttr>() != null)
                                                         .Select(x => new TestInfo
                                                         {
                                                             Attr = x.GetCustomAttribute<TestAttr>(),
                                                             Method = x
                                                         })
                                                         .OrderBy(x => x.Attr!.Suite).ThenBy(x => x.Attr!.Case).ThenBy(x => x.Attr!.Order)
                                                         .ToList();

            return testPlan;
        }

        public List<TestInfo>? GetSuite(List<TestInfo> testPlan)
        {
            var testScenario = testPlan.Where(x => String.IsNullOrWhiteSpace(x.Attr!.Suite)).ToList();
            var testSuite = testPlan.Where(x => !String.IsNullOrWhiteSpace(x.Attr!.Suite)).GroupBy(x => x.Attr!.Suite).ToList();

            Console.Clear();

            SetTitle();

            Console.WriteLine($"偵測到 {testSuite.Count} 個測試情境 (輸入情境編號 or \"Q\" 離開)\n");
            Console.WriteLine("0. [Scenario] (套裝情境)");

            for (var x = 0; x < testSuite.Count; x++) { Console.WriteLine($"{x + 1}. [{testSuite[x].Key}]"); }

            Console.WriteLine("---------------------------------------------------------------------------");

            var input = IptChk("情境編號: ", 0, testSuite.Count);

            Console.WriteLine("");

            if (input == "Q") { Environment.Exit(0); }

            if (Int32.TryParse(input, out Int32 inputX))
            {
                if (inputX == 0) { return testScenario; }
                else if (inputX > 0 && inputX <= testSuite.Count)
                {
                    var testSuiteX = testSuite[inputX - 1].ToList();

                    return testSuiteX;
                }
            }

            return null;
        }

        public List<TestInfo>? GetFeature(List<TestInfo> testSuite)
        {
            var testFeature = testSuite.GroupBy(x => x.Attr!.Feature).ToList();

            Console.Clear();

            SetTitle();

            Console.WriteLine($"偵測到 {testFeature.Count} 個測試功能 (輸入功能編號 or \"B\" 返回 or \"Q\" 離開)\n");

            for (var x = 0; x < testFeature.Count; x++) { Console.WriteLine($"{x + 1}. [{testFeature[x].Key}]"); }

            Console.WriteLine("---------------------------------------------------------------------------");

            var input = IptChk("功能編號: ", 1, testFeature.Count);

            Console.WriteLine("");

            if (input == "B") { return null; }
            if (input == "Q") { Environment.Exit(0); }

            if ((Int32.TryParse(input, out Int32 inputX)) && (inputX > 0 && inputX <= testFeature.Count))
            {
                var testFeatureX = testFeature[inputX - 1].ToList();

                return testFeatureX;
            }

            return null;
        }

        public TestInfo? GetCase(List<TestInfo> testFeature)
        {
            Console.Clear();

            SetTitle();

            Console.WriteLine($"偵測到 {testFeature.Count} 個測試流程 (輸入流程編號 or \"B\" 返回 or \"Q\" 離開)\n");
            // Console.WriteLine("0. [All]");

            for (var x = 0; x < testFeature.Count; x++) { Console.WriteLine($"{x + 1}. [{testFeature[x].Attr!.Case}]"); }

            Console.WriteLine("---------------------------------------------------------------------------");

            var input = IptChk("流程編號: ", 1, testFeature.Count);

            Console.WriteLine("");

            if (input == "B") { return null; }
            if (input == "Q") { Environment.Exit(0); }

            if ((Int32.TryParse(input, out Int32 inputX)) && (inputX > 0 && inputX <= testFeature.Count))
            {
                var testCaseX = testFeature[inputX - 1];

                return testCaseX;
            }

            return null;
        }

        public TestInfo? GetScenario(List<TestInfo> testScenario)
        {
            Console.Clear();

            SetTitle();

            Console.WriteLine($"偵測到 {testScenario.Count} 個套裝情境 (輸入套裝編號 or \"B\" 返回 or \"Q\" 離開)\n");

            for (var x = 0; x < testScenario.Count; x++) { Console.WriteLine($"{x + 1}. [{testScenario[x].Attr!.Case}]"); }

            Console.WriteLine("---------------------------------------------------------------------------");

            var input = IptChk("套裝編號: ", 1, testScenario.Count);

            Console.WriteLine("");

            if (input == "B") { return null; }
            if (input == "Q") { Environment.Exit(0); }

            if ((Int32.TryParse(input, out Int32 inputX)) && (inputX > 0 && inputX <= testScenario.Count))
            {
                var testScenarioX = testScenario[inputX - 1];

                return testScenarioX;
            }

            return null;
        }

        public void Test(TestInfo testCase)
        {
            var (suite, feature, @case) = (testCase.Attr!.Suite, testCase.Attr!.Feature, testCase.Attr!.Case);

            var title = (!String.IsNullOrWhiteSpace(suite)) ?
                            (!String.IsNullOrWhiteSpace(feature)) ? $"[{suite} - {feature} - {@case}]" :
                                                                                             $"[{suite} - {@case}]" :
                                                                                             $"[{@case}]";

            _logO.LogInformation($"{title} 開始測試");

            try
            {
                Setup();

                testCase.Method!.Invoke(this, null);

                _logO.LogInformation($"{title} 測試成功");
            }
            catch (Exception ex)
            {
                var message = ex.InnerException ?? ex;

                _logO.LogError($"{title} 測試失敗 - Message = {message}");
            }
            finally
            {
                Console.WriteLine("\n按任意鍵繼續...");
                Console.ReadKey();

                Teardown();
            }
        }
        #endregion

        #endregion

        #region Aux Methods
        private String IptChk(String prompt, Int32 min, Int32 max)
        {
            var position = Console.CursorTop;

            while (true)
            {
                Console.SetCursorPosition(0, position);
                Console.Write(prompt + "　　　　　");
                Console.SetCursorPosition(0, position);
                Console.Write(prompt);

                var input = Console.ReadLine();

                if ((input?.ToUpper() == "B") || (input?.ToUpper() == "Q")) { return input.ToUpper(); }
                else if (!String.IsNullOrWhiteSpace(input) && Byte.TryParse(input, out var inputX))
                {
                    if (inputX >= min && inputX <= max) { return input; }
                }
            }
        }

        private void SetTitle()
        {
            var title = "test3.Test";

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

        #region Nested Types
        [AttributeUsage(AttributeTargets.Method)]
        public class TestAttr : Attribute
        {
            #region Properties
            public String? Suite { get; }
            public String? Feature { get; }
            public String? Case { get; }
            public Int32 Order { get; set; }
            #endregion

            #region Constructor
            public TestAttr(String suite, String feature, String @case)
            {
                Suite = suite;
                Feature = feature;
                Case = @case;
            }
            #endregion
        }

        public class TestInfo
        {
            public AppL.TestAttr? Attr { get; set; }
            public MethodInfo? Method { get; set; }
        }
        #endregion
    }
}