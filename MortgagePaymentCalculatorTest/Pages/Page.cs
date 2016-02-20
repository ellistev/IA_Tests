using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace MortgagePaymentCalculatorTest
{
    public class Page
    {
        protected IWebDriver Driver;
        public const int TimeOut = 10;

        public Page(IWebDriver driver)
        {
            this.Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void Close() {
            Driver.Quit();
        }

        public void OpenPage()
        {
            Driver.Navigate().GoToUrl("http://");
            Driver.Manage().Window.Maximize();
        }

        public void ClickItem(By byItem)
        {
            Driver.FindElement(byItem).Click();
        }

        public void SubmitItem(By byItem)
        {
            Driver.FindElement(byItem).Submit();
        }

        public void SelectDropDownValue(By selectionElement, string selectionValue)
        {
            IWebElement comboBox = (Driver.FindElement(selectionElement));
            SelectElement selectionObject = new SelectElement(comboBox);
            selectionObject.SelectByText(selectionValue);
        }

        public void SetFieldValue(By field, string valueToSet)
        {
            IWebElement element = Driver.FindElement(field);
            element.Clear();
            element.SendKeys(valueToSet);
        }

        /// <summary>
        /// Sets the slider percentage.
        /// </summary>
        /// <param name="sliderHandleXpath">The slider handle xpath</param>
        /// <param name="sliderTrackXpath">The slider track xpath</param>
        /// <param name="percentage">The percentage</param>
        public void SetSliderPercentage(By sliderHandleXpath, By sliderTrackXpath, int percentage)
        {
            var sliderHandle = Driver.FindElement(sliderHandleXpath);
            var sliderTrack = Driver.FindElement(sliderTrackXpath);
            var width = Int32.Parse(sliderTrack.GetCssValue("width").Replace("px", ""));
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

        /// <summary>
        /// Waits until element text has changed.
        /// </summary>
        /// <param name="elementToWaitFor">The element to wait for</param>
        /// <param name="originalValue">The original value</param>
        public void WaitUntilElementTextHasChanged(By elementToWaitFor, string originalValue)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeOut));
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
    }
}
