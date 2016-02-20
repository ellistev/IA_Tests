using System;
using System.Drawing;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
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
            IAHomePage iaHomePage = new IAHomePage(Driver);
            iaHomePage.OpenPage();

            // find topLangMenuItem using Driver.FindElement and wait 5 seconds until value of this element is FR
            iaHomePage.WaitUntilEnglish(Page.TimeOut);

            // Click the "Loans" drop down menu 
            iaHomePage.ClickItem(iaHomePage.LoansMenuItem);

            // Click the "Mortgages" button
            iaHomePage.ClickItem(iaHomePage.LoanSubmenuMortgagesItem);

            // Click the "Calculate your payments" button
            IAMortgagesPage iaMortgagesPage = new IAMortgagesPage(Driver);
            iaMortgagesPage.ClickItem(iaMortgagesPage.CalculatePaymentsButton);

            //Drag the "Purchase Price" slider to verify that it is working. Include an assertion
            IAMortgagePaymentCalculatorPage iaMortgagePaymentCalculatorPage = new IAMortgagePaymentCalculatorPage(Driver);

            iaMortgagePaymentCalculatorPage.SetSliderPercentage(iaMortgagePaymentCalculatorPage.PurchasePriceSliderHandle,
                iaMortgagePaymentCalculatorPage.PurchasePriceSliderTrack, 0);

            Assert.AreEqual("0", iaMortgagePaymentCalculatorPage.PurchasePrice);

            iaMortgagePaymentCalculatorPage.SetSliderPercentage(iaMortgagePaymentCalculatorPage.PurchasePriceSliderHandle,
                iaMortgagePaymentCalculatorPage.PurchasePriceSliderTrack, 25);

            Assert.AreNotEqual("0", iaMortgagePaymentCalculatorPage.PurchasePrice);

            iaMortgagePaymentCalculatorPage.SetSliderPercentage(iaMortgagePaymentCalculatorPage.PurchasePriceSliderHandle,
                iaMortgagePaymentCalculatorPage.PurchasePriceSliderTrack, 0);

            Assert.AreEqual("0", iaMortgagePaymentCalculatorPage.PurchasePrice);


            // Set the “Purchase Price” to 500,000 using the ‘+’ icon
            // Click button "+" two times
            for (int i=1; i<=2; i++)
            {
                iaMortgagePaymentCalculatorPage.ClickItem(iaMortgagePaymentCalculatorPage.PurchasePricePlusButton);
            }

            // Check that purchase price is 500,000
            Assert.AreEqual("500000", iaMortgagePaymentCalculatorPage.PurchasePrice);
           
            // Set the “Down payment” to 50,000 using the ‘+’ icon
            iaMortgagePaymentCalculatorPage.ClickItem(iaMortgagePaymentCalculatorPage.DownPaymentPlusButton);

            // Check that Down payment is 50,000
            Assert.AreEqual("50000", iaMortgagePaymentCalculatorPage.DownPaymentValue);

            //Change the "Amortization" term drop down to 15 years
            iaMortgagePaymentCalculatorPage.SelectDropDownValue(iaMortgagePaymentCalculatorPage.AmortizationDropDown, "15 years");

            // Change the "Payment frequency" to weekly
            iaMortgagePaymentCalculatorPage.SelectDropDownValue(iaMortgagePaymentCalculatorPage.FrequencyDropDown, "weekly");

            // Change the "Interest rate" to 5%
            iaMortgagePaymentCalculatorPage.SetFieldValue(iaMortgagePaymentCalculatorPage.InterestRateField, "5.00");

            // Click the "Calculate" button
            iaMortgagePaymentCalculatorPage.SubmitItem(iaMortgagePaymentCalculatorPage.CalculateButton);
            //Driver.FindElement(By.Id("btn_calculer")).Submit();
            
            //Wait that payment is visible
            iaMortgagePaymentCalculatorPage.WaitUntilElementTextHasChanged(iaMortgagePaymentCalculatorPage.EstimatedPayment, iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);

            //Verify that the weekly payments on the right hand side of the screen shows "$ 836.75"
            Assert.AreEqual("$ 836.75", iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);

            // Change the "Interest rate" to 3%
            iaMortgagePaymentCalculatorPage.SetFieldValue(iaMortgagePaymentCalculatorPage.InterestRateField, "3.00");

            // Verify that the weekly payments on the right hand side of the screen shows "$ 732.70"
            iaMortgagePaymentCalculatorPage.SubmitItem(iaMortgagePaymentCalculatorPage.CalculateButton);

            //Wait that payment is visible
            iaMortgagePaymentCalculatorPage.WaitUntilElementTextHasChanged(iaMortgagePaymentCalculatorPage.EstimatedPayment, iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);

            //Assert.IsTrue(weeklyPayment.Contains("$ 742.70"));
            Assert.AreEqual("$ 732.70", iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);
           
            Thread.Sleep(5000);
        }







        public static void Main(string[] args)
        {

        }

    }

}
