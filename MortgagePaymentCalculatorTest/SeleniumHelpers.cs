using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MortgagePaymentCalculatorTest
{
    public abstract class SeleniumHelpers
    {
        protected readonly IWebDriver driver;

        protected SeleniumHelpers(IWebDriver driver)
        {
            this.driver = driver;
        }

        protected IWebElement Find(By by)
        {
            return driver.FindElement(by);
        }

        public bool Enabled(IWebElement element)
        {
            return element.Enabled;
        }

        private bool VisibleAndEnabled(IWebElement element)
        {
            return element.Displayed && Enabled(element);
        }
        
        protected IWebElement WaitUntilAvailable(By by)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            //wait.Until(x => VisibleAndEnabled(x.FindElement(by)));

            return Find(by);
        }
    }
}
