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

        public By PurchasePriceSliderHandle { get { return By.XPath("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]/div[9]"); } }

        public By PurchasePriceSliderTrack { get { return By.XPath("//*[@id=\"form_calculateur_versements\"]/div[2]/div/div[2]/div/div[1]"); } }

        public string PurchasePrice { get { return Driver.FindElement(By.Id("PrixPropriete")).GetAttribute("value"); } }

        public By PurchasePricePlusButton { get { return By.Id("PrixProprietePlus"); } }

        public By DownPaymentPlusButton { get { return By.Id("MiseDeFondPlus"); } }

        public string DownPaymentValue { get { return Driver.FindElement(By.Id("MiseDeFond")).GetAttribute("value"); } }

        public By AmortizationDropDown { get { return By.Id("Amortissement"); }  }

        public By FrequencyDropDown { get { return By.Id("FrequenceVersement"); } }

        public By InterestRateField { get { return By.Id("TauxInteret"); } }

        public string EstimatedPaymentAmount { get { return Driver.FindElement(EstimatedPayment).Text; } } 

        public By EstimatedPayment { get { return By.Id("paiement-resultats"); } }

        public By CalculateButton { get { return By.Id("btn_calculer");  } }

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

