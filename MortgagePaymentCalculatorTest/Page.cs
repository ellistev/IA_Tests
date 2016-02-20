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

        public Page(IWebDriver driver)
        {
            this.Driver = driver;
            PageFactory.InitElements(Driver, this);
        }

        public void Close() {
            Driver.Quit();
        }

        public void ClickItem(By byItem)
        {
            Driver.FindElement(byItem).Click();
        }

        public void SetSliderPercentage(By sliderHandleXpath, By sliderTrackXpath, int percentage)
        {
            var sliderHandle = Driver.FindElement(sliderHandleXpath);
            var sliderTrack = Driver.FindElement(sliderTrackXpath);
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
    }
}
