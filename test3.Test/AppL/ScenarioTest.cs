using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace test3.Test
{
    public partial class AppL
    {
        #region Methods
        [TestAttr("", "", "查詢館藏 → 預約取書 / 預約借閱 → 取消預約")]
        private void Scenario_Rsv()
        {
            LoginG(); PilotToCollection();

            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"(//app-btn[@data-method='select_T']//button)[{_opt.TestTypeId}]")));

            tBtn.Click();

            Thread.Sleep(1000);

            CreateRsvX(); PilotToInfo(); UpdateRsvX();
        }
        #endregion
    }
}