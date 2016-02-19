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

            Driver.Navigate().GoToUrl("http://ia.ca");
            Driver.Manage().Window.Maximize();

            var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementExists((By.CssSelector(".test:contains('Loans')"))));


            Driver.FindElement(By.CssSelector(".test")).Click();
            Driver.FindElement(By.CssSelector("a[href*='/mortgages']")).Click();

            Thread.Sleep(5000);

        }

        public static void Main(string[] args)
        {

        }

    }

}
