using System;
using System.Drawing;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MortgagePaymentCalculatorTest
{
    [TestFixture]
    public class MortgagePaymentCalculatorTests
    {
        public IWebDriver Driver { get; private set; }

        [SetUp]
        public virtual void Setup()
        {

            var options = new ChromeOptions();
            options.AddArgument("--disable-extensions");
            Driver = new ChromeDriver(options);
           
        }

        [TearDown]
        public virtual void TearDown()
        {
            Driver.Quit();
        }

        [Test]
        public void ShouldClickOnLoans()
        {

            Driver.Navigate().GoToUrl("http://ia.ca/individuals");
            Driver.Manage().Window.Maximize();

            //var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementIsVisible((By.CssSelector("#nav-secondaire > div.navbar-sub-main > ul > li.dropdown.Pret.three-items > a > span"))));


            Driver.FindElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/a/span")).Click();
            Driver.FindElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/ul/li[1]/section/ul/li[1]/a")).Click();
            Driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/div[4]/div[1]/div[2]/a")).Click();
            Thread.Sleep(5000);

        }


        public static void Main(string[] args)
        {

        }

    }

}
