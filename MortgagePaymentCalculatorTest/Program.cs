using System;
using System.Drawing;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace MortgagePaymentCalculatorTest
{
    [TestFixture]
    public class MortgagePaymentCalculatorTests
    {
        public IWebDriver Driver { get; private set; }
        public const int timeOut = 10;
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

            Driver.Navigate().GoToUrl("http://ia.ca/");
            Driver.Manage().Window.Maximize();

            // find topLangMenuItem using Driver.FindElement and wait 5 seconds until value of this element is FR
            var waitForEnglish = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut)).Until(ExpectedConditions.TextToBePresentInElement(Driver.FindElement(By.CssSelector("#topLangMenuItem > span")), "FR"));
            //Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            // Click the "Loans" drop down menu 
            Driver.FindElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/a/span")).Click();
            // Click the "Mortgages" button
            Driver.FindElement(By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/ul/li[1]/section/ul/li[1]/a")).Click();
            // Click the "Calculate your payments" button
            Driver.FindElement(By.XPath("//*[@id=\"main\"]/div[2]/div[4]/div[1]/div[2]/a")).Click();

            // todo Drag the "Purchase Price" slider to verify that it is working. Include an assertion

            SetSliderPercentage("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]/div[9]",
                "//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]", 0);
            IWebElement mortgageSliderValue = Driver.FindElement(By.Id("PrixPropriete"));
            Assert.AreEqual("0", mortgageSliderValue.GetAttribute("value"));

            SetSliderPercentage("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]/div[9]", 
                "//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]", 25);
            mortgageSliderValue = Driver.FindElement(By.Id("PrixPropriete"));
            Assert.AreNotEqual("0", mortgageSliderValue.GetAttribute("value"));

            SetSliderPercentage("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]/div[9]", 
                "//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]", 0);
            mortgageSliderValue = Driver.FindElement(By.Id("PrixPropriete"));
            Assert.AreEqual("0", mortgageSliderValue.GetAttribute("value"));


            // Set the “Purchase Price” to 500,000 using the ‘+’ icon
            // Click button "+" two times
            for (int i=1; i<=2; i++)
            {
                Driver.FindElement(By.Id("PrixProprietePlus")).Click();    
            }
            // Check that purchase price is 500,000
            IWebElement purshasePrice = Driver.FindElement(By.Id("PrixPropriete"));
            Assert.AreEqual("500000", purshasePrice.GetAttribute("value"));
           
            // Set the “Down payment” to 50,000 using the ‘+’ icon
            Driver.FindElement(By.Id("MiseDeFondPlus")).Click();
            // Check that Down payment is 50,000
            IWebElement downPayment = Driver.FindElement(By.Id("MiseDeFond"));
            Assert.AreEqual("50000", downPayment.GetAttribute("value"));

            //Change the "Amortization" term drop down to 15 years
            IWebElement comboBoxAmortization = (Driver.FindElement(By.Id("Amortissement")));
            SelectElement selectedValueAmortization = new SelectElement(comboBoxAmortization);
            selectedValueAmortization.SelectByText("15 years");

            // Change the "Payment frequency" to weekly
            IWebElement comboBoxPaymentFrequency = (Driver.FindElement(By.Id("FrequenceVersement")));
            SelectElement selectedValuePaymentFrequency= new SelectElement(comboBoxPaymentFrequency);
            selectedValuePaymentFrequency.SelectByText("weekly");

            // Change the "Interest rate" to 5%
            InterestRateChange("5.00");

            string weeklyPayment = Driver.FindElement(By.Id("paiement-resultats")).Text;

            // Click the "Calculate" button
            Driver.FindElement(By.Id("btn_calculer")).Submit();

            
            //Wait that payment is visible
            WaitUntilElementTextHasChanged(By.Id("paiement-resultats"), weeklyPayment);

            //Verify that the weekly payments on the right hand side of the screen shows "$ 836.75"
            weeklyPayment = Driver.FindElement(By.Id("paiement-resultats")).Text;

            Assert.AreEqual("$ 836.75", weeklyPayment);


            // Change the "Interest rate" to 3%
            InterestRateChange("3.00");

            // --TO DO Verify that the weekly payments on the right hand side of the screen shows "$ 732.70"
            Driver.FindElement(By.Id("btn_calculer")).Submit();


            //Wait that payment is visible
            WaitUntilElementTextHasChanged(By.Id("paiement-resultats"), weeklyPayment);

             weeklyPayment = Driver.FindElement(By.Id("paiement-resultats")).Text;

            //Assert.IsTrue(weeklyPayment.Contains("$ 742.70"));
            Assert.AreEqual("$ 732.70", weeklyPayment);
           
            Thread.Sleep(5000);
        }

        private void WaitUntilElementTextHasChanged(By elementToWaitFor, string originalValue)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut));
            wait.Until<IWebElement>((d) =>
            {
                IWebElement element = Driver.FindElement(elementToWaitFor);
                if (element.Text != originalValue)
                {
                    return element;
                }

                return null;
            });
        }

        // this function changes interest rate 
        private void InterestRateChange(string interestRateValue)
        {
            IWebElement interestRate = Driver.FindElement(By.Id("TauxInteret"));
            interestRate.Clear();
            interestRate.SendKeys(interestRateValue);
        }

        public void SetSliderPercentage(string sliderHandleXpath, string sliderTrackXpath, int percentage)
        {
            var sliderHandle = Driver.FindElement(By.XPath(sliderHandleXpath));
            var sliderTrack = Driver.FindElement(By.XPath(sliderTrackXpath));
            var width = int.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
            int dx = 0;
            if (percentage == 0)
            {
                dx = -5000;
            }
            else
            {
                dx = (int)(percentage / 100.0 * width);
            }
            new Actions(Driver)
                        .DragAndDropToOffset(sliderHandle, dx, 0)
                        .Build()
                        .Perform();
        }


        public static void Main(string[] args)
        {

        }

    }

}
