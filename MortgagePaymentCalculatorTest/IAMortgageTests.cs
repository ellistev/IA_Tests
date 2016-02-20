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
    public class IAMortgageTests
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

        [TestCase("15 years", "weekly", "$ 836.75", "$ 732.70")]
        [TestCase("15 years", "Biweekly", "$ 1,674.30", "$ 1,465.83")]
        [TestCase("15 years", "Biweekly +", "$ 1,815.84", "$ 1,589.04")]
        [TestCase("15 years", "weekly +", "$ 907.92", "$ 794.52")]
        [TestCase("15 years", "Monthly", "$ 3,631.67", "$ 3,178.08")]
        [TestCase("20 years", "weekly", "$ 697.67", "$ 588.20")]
        [TestCase("20 years", "Biweekly", "$ 1,396.00", "$ 1,176.74")]
        [TestCase("20 years", "Biweekly +", "$ 1,514.02", "$ 1,275.65")]
        [TestCase("20 years", "weekly +", "$ 757.01", "$ 637.82")]
        [TestCase("20 years", "Monthly", "$ 3,028.03", "$ 2,551.30")]
        [TestCase("25 years", "weekly", "$ 617.49", "$ 502.76")]
        [TestCase("25 years", "Biweekly", "$ 1,235.57", "$ 1,005.81")]
        [TestCase("25 years", "Biweekly +", "$ 1,340.02", "$ 1,090.36")]
        [TestCase("25 years", "weekly +", "$ 670.01", "$ 545.18")]
        [TestCase("25 years", "Monthly", "$ 2,680.04", "$ 2,180.72")]
        [TestCase("30 years", "weekly", "$ 566.62", "$ 446.84")]
        [TestCase("30 years", "Biweekly", "$ 1,133.78", "$ 893.93")]
        [TestCase("30 years", "Biweekly +", "$ 1,229.62", "$ 969.07")]
        [TestCase("30 years", "weekly +", "$ 614.81", "$ 484.54")]
        [TestCase("30 years", "Monthly", "$ 2,459.25", "$ 1,938.14")]

        public void ShouldClickOnLoans(string amortization, string frequency, string paymentAt5, string paymentAt3)
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
            iaMortgagePaymentCalculatorPage.SelectDropDownValue(iaMortgagePaymentCalculatorPage.AmortizationDropDown, amortization);

            // Change the "Payment frequency" to weekly
            iaMortgagePaymentCalculatorPage.SelectDropDownValue(iaMortgagePaymentCalculatorPage.FrequencyDropDown, frequency);

            // Change the "Interest rate" to 5%
            iaMortgagePaymentCalculatorPage.SetFieldValue(iaMortgagePaymentCalculatorPage.InterestRateField, "5.00");

            // Click the "Calculate" button
            iaMortgagePaymentCalculatorPage.SubmitItem(iaMortgagePaymentCalculatorPage.CalculateButton);
            
            //Wait that payment is visible
            iaMortgagePaymentCalculatorPage.WaitUntilElementTextHasChanged(iaMortgagePaymentCalculatorPage.EstimatedPayment, iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);

            //Verify that the weekly payments on the right hand side of the screen shows "$ 836.75"
            Assert.AreEqual(paymentAt5, iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);

            // Change the "Interest rate" to 3%
            iaMortgagePaymentCalculatorPage.SetFieldValue(iaMortgagePaymentCalculatorPage.InterestRateField, "3.00");

            // Verify that the weekly payments on the right hand side of the screen shows "$ 732.70"
            iaMortgagePaymentCalculatorPage.SubmitItem(iaMortgagePaymentCalculatorPage.CalculateButton);

            //Wait that payment is visible
            iaMortgagePaymentCalculatorPage.WaitUntilElementTextHasChanged(iaMortgagePaymentCalculatorPage.EstimatedPayment, iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);

            //Assert.IsTrue(weeklyPayment.Contains("$ 742.70"));
            Assert.AreEqual(paymentAt3, iaMortgagePaymentCalculatorPage.EstimatedPaymentAmount);
           
        }


    }

}
