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

            //Driver.Navigate().GoToUrl("http://ia.ca/individuals");
            Driver.Navigate().GoToUrl("http://ia.ca/");
            Driver.Manage().Window.Maximize();

            //var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.ElementIsVisible((By.CssSelector("#nav-secondaire > div.navbar-sub-main > ul > li.dropdown.Pret.three-items > a > span"))));
            //var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.TextToBePresentInElementValue(By.CssSelector("#topLangMenuItem > span"),"FR"));

            //var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.TextToBePresentInElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/a/span"), "Loans"));
           
            
            //timeout variable in seconds
            int timeOut;
            timeOut = 10;
            // find topLangMenuItem using Driver.FindElement and wait 5 seconds until value of this element is FR
            var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut)).Until(ExpectedConditions.TextToBePresentInElement(Driver.FindElement(By.CssSelector("#topLangMenuItem > span")), "FR"));
            //Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            // Click the "Loans" drop down menu 
            Driver.FindElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/a/span")).Click();
            // Click the "Mortgages" button
            Driver.FindElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/ul/li[1]/section/ul/li[1]/a")).Click();
            // Click the "Calculate your payments" button
            Driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/div[4]/div[1]/div[2]/a")).Click();
            // -- TO DO Drag the "Purchase Price" slider to verify that it is working. Include an assertion

            // Set the “Purchase Price” to 500,000 using the ‘+’ icon
            // Click button "+" to times
            for (int i=1; i<=2; i++)
            {
                Driver.FindElement(By.Id("PrixProprietePlus")).Click();    
            }
            // Check that purchase price is 500,000
            IWebElement purshasePrice = Driver.FindElement(By.Id("PrixPropriete"));
            Assert.AreEqual(purshasePrice.GetAttribute("value"),"500000");
           
            // Set the “Down payment” to 50,000 using the ‘+’ icon
            Driver.FindElement(By.Id("MiseDeFondPlus")).Click();
            // Check that Down payment is 50,000
            IWebElement downPayment = Driver.FindElement(By.Id("MiseDeFond"));
            Assert.AreEqual(downPayment.GetAttribute("value"),"50000");

            //Change the "Amortization" term drop down to 15 years
            IWebElement comboBoxAmortization = (Driver.FindElement(By.Id("Amortissement")));
            SelectElement selectedValueAmortization = new SelectElement(comboBoxAmortization);
            selectedValueAmortization.SelectByText("15 years");

            // Change the "Payment frequency" to weekly
            IWebElement comboBoxPaymentFrequency = (Driver.FindElement(By.Id("FrequenceVersement")));
            SelectElement selectedValuePaymentFrequency= new SelectElement(comboBoxPaymentFrequency);
            selectedValuePaymentFrequency.SelectByText("weekly");

            // Change the "Interest rate" to 5%
            interestRateChange("5.00");

            // TO DELETE
            // string interestRateValue;
            //interestRateValue = "5.00";
            //IWebElement interestRate = Driver.FindElement(By.Id("TauxInteret"));
           // interestRate.Clear();
           // interestRate.SendKeys(interestRateValue);

            // Click the "Calculate" button
            Driver.FindElement(By.Id("btn_calculer")).Click();

            
            //Wait that payment is visible
            var waitForCalculation = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut)).Until(ExpectedConditions.ElementExists((By.Id("paiement-resultats"))));

            // --TO DO Verify that the weekly payments on the right hand side of the screen shows "$ 836.75"
            //string weeklyPayment = Driver.FindElement(By.Id("paiement-resultats")).GetCssValue("class");
            //Assert.AreEqual(weeklyPayment.Equals("$ 836.75"));
            

           //Assert.IsTrue(weeklyPayment.Contains("$ 836.75"));


            // Change the "Interest rate" to 3%
            interestRateChange("3.00");

            // --TO DO Verify that the weekly payments on the right hand side of the screen shows "$ 732.70"
           
            Thread.Sleep(5000);

        }

        // this function changes interest rate 
        private void interestRateChange(string interestRateValue)
        {
            IWebElement interestRate = Driver.FindElement(By.Id("TauxInteret"));
            interestRate.Clear();
            interestRate.SendKeys(interestRateValue);
        }


        public static void Main(string[] args)
        {

        }

    }

}
