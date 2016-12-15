using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using myExcel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Mail;
using FirebirdSql.Data.FirebirdClient;
using System.Diagnostics;
using OpenQA.Selenium.Interactions;

namespace SunPharma
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source.IndexOf(toCheck, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }

    public class Methods
    {
        private readonly FirefoxDriver _firefox;
        private List<string> messageContent = new List<string>();
        private RowDataList _dashboardData = new RowDataList();
        private RowDataList _morionData = new RowDataList();
        private int DivNumber = 7; //номер div, с которого начинаются данные по продажам
        private int DivRange = 7; //период div, через которые нужно отбирать продажи



        public Methods(FirefoxDriver firefox)
        {
            _firefox = firefox;
        }

        public string MessageContent(List<string> list)
        {
            var sb = new StringBuilder();
            foreach (var str in list)
            {
                sb.AppendLine(str);
                sb.AppendLine("<br>");
            }
            return sb.ToString();
        }

        public void email_send(string subject)
        {
            var mail = new MailMessage();
            mail.IsBodyHtml = true;
            var smtpServer = new SmtpClient("post.morion.ua");
            mail.From = new MailAddress("snizhana.nomirovska@proximaresearch.com");
            mail.To.Add("snizhana.nomirovska@proximaresearch.com");
            //mail.To.Add("nataly.tenkova@proximaresearch.com");
            mail.Subject = subject;
            mail.Body = MessageContent(messageContent);
            smtpServer.Send(mail);
        }

        public void WaitForAjax()
        {
            while (true)
            {
                var ajaxIsComplete = (bool)(_firefox as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                if (ajaxIsComplete)
                {
                    Thread.Sleep(500);
                    break;
                }
                Thread.Sleep(500);
            }
        }

        public void TryToLoadPage(string url, string waitPresenceAllElementsByXPath)
        {
            WebDriverWait wait = new WebDriverWait(_firefox, TimeSpan.FromSeconds(60));
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    _firefox.Navigate().GoToUrl(url);
                    wait.Until(
                        ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath(waitPresenceAllElementsByXPath)));
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine("Load page. Attempt №" + i);
                    i++;
                }
            }

        }

        public void TryToClick(string locator)
        {
            var maxElementRetries = 10;
            var retries = 0;
            while (true)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(_firefox, TimeSpan.FromSeconds(120));
                    wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                    _firefox.FindElement(By.XPath(locator)).Click();
                    return;
                }
                catch (Exception e)
                {
                    if (retries < maxElementRetries)
                    {
                        retries++;
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        public void WaitForTextPatternInAttribute(string locator, string attr, string text)
        {
            WebDriverWait wait = new WebDriverWait(_firefox, TimeSpan.FromSeconds(120));
            const int waitRetryDelayMs = 500; //шаг итерации (задержка)
            const int timeOut = 500; //время тайм маута 
            bool first = true;

            for (int milliSecond = 0; ; milliSecond += waitRetryDelayMs)
            {
                if (milliSecond > timeOut * 10000)
                {
                    //Debug.WriteLine("Timeout: Text " + text + " is not found ");
                    break; //если время ожидания закончилось (элемент за выделенное время не был найден)
                }

                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(locator)));
                if (_firefox.FindElement(By.XPath(locator)).GetAttribute(attr).ContainsIgnoreCase(text))
                {
                    //if (!first) Debug.WriteLine("Text is found: " + text);
                    break; //если элемент найден
                }

                //if (first) Debug.WriteLine("Waiting for text is present: " + text);

                first = false;
                Thread.Sleep(waitRetryDelayMs);
            }

        }


        public void LoginSunPharma(string url, string login, string password)
        {
            WebDriverWait wait = new WebDriverWait(_firefox, TimeSpan.FromSeconds(120));
            PageElements pageElements = new PageElements(_firefox);

            _firefox.Navigate().GoToUrl(url);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.IframeLoginDialogXpath)));
            _firefox.SwitchTo().Frame(pageElements.IframeLoginDialog);
            pageElements.Login.SendKeys(login);
            pageElements.Password.SendKeys(password);
            TryToClick(PageElements.OkButtonXpath);
            Thread.Sleep(3000);
            _firefox.SwitchTo().DefaultContent();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.ContinueButtonXpath)));
            TryToClick(PageElements.ContinueButtonXpath);
        }

        public void SetUpFilters()
        {
            WebDriverWait wait = new WebDriverWait(_firefox, TimeSpan.FromSeconds(120));
            PageElements pageElements = new PageElements(_firefox);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.RatingTabXpath)));
            Thread.Sleep(1000);
            if (pageElements.RatingTab.GetAttribute("class") != "selectedtab")
            {
                TryToClick(PageElements.RatingTabXpath);
                WaitForTextPatternInAttribute(PageElements.RatingTabXpath, "class", "selectedtab");
                Thread.Sleep(200);
            }
            if (pageElements.Pcs.GetAttribute("class") == "QvExcluded")
            {
                TryToClick(PageElements.PcsXpath);
                WaitForTextPatternInAttribute(PageElements.PcsXpath, "class", "QvSelected");
                Thread.Sleep(200);
            }
            if (pageElements.All.GetAttribute("class") == "QvExcluded")
            {
                TryToClick(PageElements.AllXpath);
                WaitForTextPatternInAttribute(PageElements.AllXpath, "class", "QvSelected");
                Thread.Sleep(200);

            }
            if (pageElements.TopAll.GetAttribute("class") == "QvExcluded")
            {
                TryToClick(PageElements.TopAllXpath);
                WaitForTextPatternInAttribute(PageElements.TopAllXpath, "class", "QvSelected");
                Thread.Sleep(200);

            }
            if (pageElements.SortBy.Text != "Distributor")
            {
                TryToClick(PageElements.SortByXpath);
                WaitForAjax();
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.DisributorOptionXpath)));
                TryToClick(PageElements.DisributorOptionXpath);
                wait.Until(ExpectedConditions.TextToBePresentInElement(pageElements.SortBy, "by Distributor"));
                Thread.Sleep(200);
            }
        }

        public void StoreDashBoardData(DateTime dateFrom, DateTime dateTo)
        {
            WebDriverWait wait = new WebDriverWait(_firefox, TimeSpan.FromSeconds(120));
            Actions action = new Actions(_firefox);
            PageElements pageElements = new PageElements(_firefox);

            for (DateTime i = dateFrom; i <= dateTo; i = i.AddMonths(1))
            {
                var currentDate = i.ToString("yyyy MM");
                TryToClick(PageElements.ChoosenDateXpath);
                Thread.Sleep(2000);
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.DropDownPeriodMenuXpath)));
                action.ContextClick(pageElements.DropDownPeriodMenu).Perform();
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.SearchOptionXpath)));
                TryToClick(PageElements.SearchOptionXpath);
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(PageElements.InputFieldXpath)));
                Thread.Sleep(200);
                pageElements.InputField.SendKeys(currentDate + Keys.Enter);
                Thread.Sleep(500);
                WaitForTextPatternInAttribute(PageElements.ChoosenDateXpath, "title", currentDate);
                Thread.Sleep(2000);
                var distributorsList =
                    _firefox.FindElementsByXPath(
                        ".//*[@class='QvFrame Document_CH1324']/div[3]/div[1]/div[1]/div[2]/div/div");
                var salesPcsList =
                    _firefox.FindElementsByXPath(
                        ".//*[@class='QvFrame Document_CH1324']/div[3]/div[1]/div[1]/div[5]/div/div");

                List<string> distributorArray = new List<string>();
                List<decimal> salesArray = new List<decimal>();

                for (int n = 2; n < distributorsList.Count; n++) // включая Others
                {
                    if(distributorsList[n].GetAttribute("title") == "Others")continue;
                    var name = distributorsList[n].GetAttribute("title");
                    if (name.IndexOf('.') != -1)
                    {
                        name = name.Substring(name.IndexOf('.') + 2);
                    }
                    distributorArray.Add(name);
                }

                for (int n = DivNumber; n <= distributorArray.Count * DivRange; n = n + DivRange)
                // считываем цифры исходя из кол-ва имён дистрибьюторов
                {
                    var sales = Math.Round(Convert.ToDecimal(salesPcsList[n].GetAttribute("title")), 1);
                    salesArray.Add(sales);
                }

                for (int x = 0; x < distributorArray.Count; x++)
                {
                    var rowData = new RowData()
                    {
                        Distributor = distributorArray[x],
                        SalesMonthPcs = salesArray[x],
                        Year = i.Year,
                        Month = i.Month
                    };
                    _dashboardData.Add(rowData);
                }
            }
            Console.WriteLine("Data have been stored from dashboard ");
        }

        public void StoreMorionData(DateTime dateFrom, DateTime dateTo)
        {
            for (DateTime i = dateFrom; i <= dateTo; i = i.AddMonths(1))
            {
                RowDataList morionData;
                var currentMonth = i.Month > 9 ? i.Month.ToString() : "0" + i.Month;
                var currentYear = i.Year.ToString();

                var query =
                    "select cast(right(RR.USER_NUMBER, 4) ||left(RR.USER_NUMBER, 2) || '001' as integer) as PERIOD_ID, " +
                    "OO.NAME_BASIC_RUS, " +
                    "sum(UU.FIELD_M_6317) as Q, " +
                    "sum(UU.FIELD_M_6317 * coalesce(D.NUMBER, 1) / coalesce(DL.NUMBER, D.NUMBER, 1)) as Q_RECALC " +
                    "from USER_DATA_REESTR RR " +
                    "left join USER_DATA_M_1009 UU on UU.REESTR_ID = RR.ID " +
                    "left join O_ADDRESS OA on OA.ID = UU.FIELD_OBJECT_M_6308 " +
                    "left join O_ORGANIZATION OO on OO.ID = OA.ORGANIZATION_ID " +
                    "left join M_DRUGS D on D.ID = UU.FIELD_OBJECT_M_6311 " +
                    "left join M_DRUGS_LINK LL on LL.DRUGS_ID = UU.FIELD_OBJECT_M_6311 " +
                    "left join M_DRUGS DL on DL.ID = LL.LINK_DRUGS_ID " +
                    "where RR.USER_DATA_ID = -1009 and " +
                    "RR.USER_NUMBER = '" + currentMonth + "-" + currentYear + "' " +
                    "group by 1, 2 " +
                    "order by 4 desc";

                WorkWithDataBase.StoreDataFromDB(out morionData, query);
                _morionData.AddRange(morionData);
            }

            Thread.Sleep(2000);


            /*foreach (var data in _morionData)
           {
               Console.WriteLine(data.Year + " " + data.Month + " " + data.Distibutor + " " + data.SalesMonthPcs);
           }*/
        }

        public void CompareData()
        {
            Console.WriteLine("Есть в базе, но не совпадает с дашбордом:");
            var diff1 = RowDataList.ComparePcsByDistributorByDate(_morionData, _dashboardData);
            foreach (var d in diff1)
            {
                if (d.SalesMonthPcs != 0)
                    Console.WriteLine(d.Year + " " + d.Month + " " + d.Distributor + " " + d.SalesMonthPcs);
            }
            
            Console.WriteLine("Есть в дашборде, но не совпадают с базой:");
            var diff2 = RowDataList.ComparePcsByDistributorByDate(_dashboardData, _morionData);
            foreach (var d in diff2)
            {
                if (d.SalesMonthPcs != 0)
                    Console.WriteLine(d.Year + " " + d.Month + " " + d.Distributor + " " + d.SalesMonthPcs);
            }
        }
    }
}
