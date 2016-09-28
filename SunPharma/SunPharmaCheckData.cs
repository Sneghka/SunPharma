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


namespace SunPharma
{
    [TestFixture]
    public class SunPharmaCheckData
    {
        [Test]
        public void CheckSalesPsc()
        {
            FirefoxDriver firefox = new FirefoxDriver();
            var methods = new Methods(firefox);
            
            DateTime dateFrom = new DateTime(2016,04,01);
            DateTime dateTo = new DateTime(2016,05,01);
            var url ="http://pharmxplorer.com.ua/QvAJAXZfc/opendoc.htm?document=TestDocs\\SunPharma\\Proxima_Distr_SunPharma.qvw&host=QVS@qlikview&anonymous=true";
            var login = "user_sp";
            var password = "34xcpqfo7y";

            methods.LoginSunPharma(url, login, password);
            methods.SetUpFilters();
            methods.StoreDashBoardData(dateFrom, dateTo);
            methods.StoreMorionData(dateFrom, dateTo);
            methods.CompareData();
            firefox.Quit();
        }

    }
}
