using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace MortgagePaymentCalculatorTest
{
    public class IAMortgagePaymentCalculatorPage : Page {
 
        protected IWebDriver Driver;

        public By PurchasePriceSliderHandle
        {
            get { return By.XPath("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]/div[9]"); }
        }

        public By PurchasePriceSliderTrack
        {
            get { return By.XPath("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]"); }
        }

        public IAMortgagePaymentCalculatorPage(IWebDriver driver)
            : base(driver)
        {
            this.Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void OpenPage() {
            //not used in this code/qa test
            Driver.Navigate().GoToUrl("https://ia.ca/mortgage-payment-calculator");
            Driver.Manage().Window.Maximize();
        }
    }
}

