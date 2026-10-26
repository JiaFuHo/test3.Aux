using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace test3.Test
{
    public partial class AppL
    {
        #region Methods

        #region InfoBase
        public void PilotToInfo()
        {
            _driver!.FindElement(By.Id("li_Info")).Click();

            Thread.Sleep(1000);

            _driver!.FindElement(By.Id("btn_Info")).Click();

            Thread.Sleep(1000);
        }
        #endregion

        #region InfoX

        #endregion

        #region Fav
        public void DeleteFav()
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

        #endregion

        #region Hx
        public void CreateFav()
        {
            _driver!.FindElement(By.Id("btn_Hx")).Click();

            Thread.Sleep(1000);

            var tBtn = _wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("(//app-btn[@data-method='createFav']//button)[1]")));

            tBtn.Click();

            Thread.Sleep(1000);
        }
        #endregion

        #region Msg

        #endregion

        #endregion
    }
}