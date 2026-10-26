using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));

            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(_opt.TestURL!);
        }

        public void Teardown()
        {
            try { _driver!.Quit(); _driver!.Dispose(); }
            finally { _driver = null; _wait = null; }
        }
        #endregion

        #region Login
        public void Login()
        {
            _driver!.FindElement(By.Id("btnLGX")).Click();

            Thread.Sleep(1000);

            _driver!.FindElement(By.Id("ipt_Acc")).SendKeys(_opt.TestAcc!);
            _driver!.FindElement(By.Id("ipt_Pwd")).SendKeys(_opt.TestPwd!);
            _driver!.FindElement(By.Id("btnLG")).Click();

            Thread.Sleep(1000);
        }
        #endregion

        #region Misc
        public void Confirm(Boolean status)
        {
            var alert = _wait!.Until(ExpectedConditions.AlertIsPresent());

            if (status) { alert.Accept(); }
            else { alert.Dismiss(); }
        }
        #endregion

        #endregion

        #region Aux Methods
        public String IptChk(String prompt)
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
                else if (!String.IsNullOrWhiteSpace(input) && Byte.TryParse(input, out _)) { return input; }
            }
        }

        public void SetTitle()
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
    }
}