using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace test3.Test
{
    public partial class AppL
    {
        #region Methods

        #region InfoX

        #endregion

        #region Fav
        [TestAttr("帳戶", "我的最愛", "刪除最愛")]
        private void DeleteFav() { LoginG(); PilotToInfo(); DeleteFavX(); }

        private void DeleteFavX()
        {
            _driver!.FindElement(By.Id("btn_Fav")).Click();

            Thread.Sleep(1000);

            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("(//app-btn[@data-method='deleteFav']//button)[1]")));

            tBtn.Click();

            Thread.Sleep(1000);

            Confirm(true);

            Thread.Sleep(1000);
        }
        #endregion

        #region Rsv
        [TestAttr("帳戶", "預約紀錄", "取消預約")]
        private void UpdateRsv() { LoginG(); PilotToInfo(); UpdateRsvX(); }

        private void UpdateRsvX()
        {
            _driver!.FindElement(By.Id("btn_Rsv")).Click();

            Thread.Sleep(1000);

            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("(//app-btn[@data-method='updateRsv']//button)[1]")));

            tBtn.Click();

            Thread.Sleep(1000);

            Confirm(true);

            Thread.Sleep(1000);
        }
        #endregion

        #region Hx
        [TestAttr("帳戶", "借閱紀錄", "新增最愛")]
        private void CreateFav() { LoginG(); PilotToInfo(); CreateFavX(); }

        private void CreateFavX()
        {
            _driver!.FindElement(By.Id("btn_Hx")).Click();

            Thread.Sleep(1000);

            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("(//app-btn[@data-method='createFav']//button)[1]")));

            tBtn.Click();

            Thread.Sleep(1000);
        }

        [TestAttr("帳戶", "借閱紀錄", "修改評論")]
        private void UpdateHx() { LoginG(); PilotToInfo(); UpdateHxX(); }

        private void UpdateHxX()
        {
            _driver!.FindElement(By.Id("btn_Hx")).Click();

            Thread.Sleep(1000);

            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("(//app-btn[@data-method='popup']//button)[1]")));

            tBtn.Click();

            Thread.Sleep(1000);

            _driver!.FindElement(By.Id("ipt_Feedback")).SendKeys(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            Thread.Sleep(1000);

            _driver!.FindElement(By.Id("btnS")).Click();

            Thread.Sleep(1000);
        }
        #endregion

        #region Msg

        #endregion

        #endregion

        #region Aux Methods
        private void PilotToInfo()
        {
            _driver!.FindElement(By.Id("li_Info")).Click();

            Thread.Sleep(1000);

            _driver!.FindElement(By.Id("btn_Info")).Click();

            Thread.Sleep(1000);
        }
        #endregion
    }
}