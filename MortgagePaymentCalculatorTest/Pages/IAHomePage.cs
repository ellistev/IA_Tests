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
    public class IAHomePage : Page{
 
        protected IWebDriver Driver;

        public By LoansMenuItem
        {
            get { return By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/a/span"); }
        }

        public By LoanSubmenuMortgagesItem
        {
            get { return By.XPath("//*[@id=\"nav-secondaire\"]/div[1]/ul/li[4]/ul/li[1]/section/ul/li[1]/a"); }
        }
 
        public IAHomePage(IWebDriver driver) : base(driver)
        {
            this.Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void OpenPage() {
            Driver.Navigate().GoToUrl("http://ia.ca/");
            Driver.Manage().Window.Maximize();
        }

        public void WaitUntilEnglish(int timeOut, string language)
        {
            // find topLangMenuItem using Driver.FindElement and wait 5 seconds until value of this element is FR
            new WebDriverWait(Driver, TimeSpan.FromSeconds(timeOut)).Until(ExpectedConditions.TextToBePresentInElement(Driver.FindElement(By.CssSelector("#topLangMenuItem > span")), language));
        }

        public void ChangeToLanguageSite(string language)
        {
            var currentLanguageButton = Driver.FindElement(By.CssSelector("#topLangMenuItem > span"));
            var currentLanguageButtonValue = currentLanguageButton.Text.Trim();

            if (language.ToLower() == currentLanguageButtonValue.ToLower())
            {
                currentLanguageButton.Click();
                new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeOut)).Until(ExpectedConditions.TextToBePresentInElement(Driver.FindElement(By.CssSelector("#topLangMenuItem > span")), (language.ToLower() == "en" ? "FR" : "EN")));

            }

        }
    }
}

