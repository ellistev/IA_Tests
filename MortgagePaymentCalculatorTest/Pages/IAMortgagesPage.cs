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
    public class IAMortgagesPage : Page {
 
        protected IWebDriver Driver;

        public By CalculatePaymentsButton
        {
            get { return By.XPath("//*[@id=\"main\"]/div[2]/div[4]/div[1]/div[2]/a"); }
        }

        public IAMortgagesPage(IWebDriver driver) : base(driver)
        {
            this.Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void OpenPage() {
            //not used in this code/qa test
            Driver.Navigate().GoToUrl("http://ia.ca/mortage");
            Driver.Manage().Window.Maximize();
        }
    }
}

