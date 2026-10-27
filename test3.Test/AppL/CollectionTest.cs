using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace test3.Test
{
    public partial class AppL
    {
        #region Methods
        [TestAttr("館藏", "", "預約取書 / 預約借閱")]
        private void CreateRsv() { LoginG(); PilotToCollection(); CreateRsvX(); }

        private void CreateRsvX()
        {
            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"(//app-btn[@data-method='createRsv']//button)[1]")));

            tBtn.Click();

            Thread.Sleep(1000);
        }
        #endregion

        #region Aux Methods
        private void PilotToCollection()
        {
            _driver!.FindElement(By.Id("btn_Collection")).Click();

            Thread.Sleep(1000);
        }
        #endregion
    }
}