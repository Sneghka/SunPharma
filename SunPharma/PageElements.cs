using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SunPharma
{
    public class PageElements
    {
        private readonly FirefoxDriver _firefox;

        public PageElements(FirefoxDriver firefox)
        {
            _firefox = firefox;
        }

        #region Xpaths

        public const string IframeLoginDialogXpath = ".//*[@id='popupFrame1']";
        public const string LoginXpath = ".//*[@id='userid']";
        public const string PasswordXpath = ".//*[@id='password']";
        public const string OkButtonXpath = ".//*[@id='PageFooter']/td/button[1]";
        public const string ContinueButtonXpath = ".//*[@class='QvFrame Document_TX3438']/div[2]/table/tbody/tr/td";
        public const string RatingTabXpath = ".//*[@rel='DocumentSH127']";
        public const string PcsXpath = ".//*[@class='QvFrame Document_LB2190']/div[2]/div/div[1]/div[3]";
        public const string AllXpath = ".//*[@class='QvFrame Document_LB2282']/div[3]/div/div[1]/div[1]";
        public const string TopAllXpath = ".//*[@class='QvFrame Document_LB2189']/div[3]/div/div[1]/div[8]";
        public const string SortByXpath = ".//*[@class='QvFrame Document_TX3148']/div[2]/table/tbody/tr/td";
        public const string DisributorOptionXpath = ".//*[@class='QvFrame Document_LB2094']/div[3]/div/div[1]/div[10]";
        public const string ClearPeriodXpath = ".//*[@class='QvFrame Document_LB2289']/div[2]/div[1]/div[1]";
        public const string ChoosenDateXpath = ".//*[@class='QvFrame Document_MB450']/div[3]/div/div[1]/div[5]/div/div[3]";
        public const string DropDownPeriodMenuXpath = ".//*[@id='DS']/div/div/div[1]/div[1]";
        public const string SearchOptionXpath = "html/body/ul/li[1]/a";
        public const string InputFieldXpath = "html/body/*[@class='PopupSearch']/input";


        #endregion


        #region LoginElements

        public IWebElement IframeLoginDialog
        {
            get { return _firefox.FindElement(By.XPath(".//*[@id='popupFrame1']")); }
        }
        public IWebElement Login
        {
            get { return _firefox.FindElement(By.XPath(".//*[@id='userid']")); }
        }
        public IWebElement Password
        {
            get { return _firefox.FindElement(By.XPath(".//*[@id='password']")); }
        }
        public IWebElement OkButton
        {
            get { return _firefox.FindElement(By.XPath(".//*[@id='PageFooter']/td/button[1]")); }
        }

        #endregion


        #region PageElements

        public IWebElement ContinueButton
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_TX3438']/div[2]/table/tbody/tr/td")); }
        }
        public IWebElement RatingTab
        {
            get { return _firefox.FindElement(By.XPath(".//*[@rel='DocumentSH127']")); }
        }
        public IWebElement Pcs
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_LB2190']/div[2]/div/div[1]/div[3]")); }
        }
        public IWebElement All
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_LB2282']/div[3]/div/div[1]/div[1]")); }
        }
        public IWebElement TopAll
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_LB2189']/div[3]/div/div[1]/div[8]")); }
        }

        public IWebElement SortBy
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_TX3148']/div[2]/table/tbody/tr/td")); }
        }
        public IWebElement DisributorOption
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_LB2094']/div[3]/div/div[1]/div[10]")); }
        }
        public IWebElement ClearPeriod
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_LB2289']/div[2]/div[1]/div[1]")); }
        }
        public IWebElement ChoosenDate
        {
            get { return _firefox.FindElement(By.XPath(".//*[@class='QvFrame Document_MB450']/div[3]/div/div[1]/div[5]/div/div[3]")); }
        }
        public IWebElement DropDownPeriodMenu
        {
            get { return _firefox.FindElement(By.XPath(".//*[@id='DS']/div/div/div[1]/div[1]")); }
        }

        public IWebElement SearchOption
        {
            get { return _firefox.FindElement(By.XPath("html/body/ul/li[1]/a")); }
        }
        public IWebElement InputField
        {
            get { return _firefox.FindElement(By.XPath("html/body/*[@class='PopupSearch']/input")); }
        }

       #endregion
    }
}
