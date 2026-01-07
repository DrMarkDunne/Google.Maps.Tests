using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Allure.Attributes;

namespace Google.Maps.Tests.Pages
{
    public class GoogleMapsPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public GoogleMapsPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [AllureStep("Go to Google Maps")]
        public void GoTo()
        {
            _driver.Navigate().GoToUrl("https://www.google.com/maps");
            _driver.Manage().Window.Maximize();
        }

        [AllureStep("Accept Cookies")]
        public void AcceptCookies()
        {
            // Accept the 'I Agree' to cookies iframe
            CloseIframe("//*[@id=\"introAgreeButton\"]/span/span");
            // Select 'No Thanks' to ad spam iframe
            CloseIframe("//*[@id=\"yDmH0d\"]/c-wiz/div/div/c-wiz/div/div/div/div[2]/div[2]/button");
        }

        private void CloseIframe(string xpath, string tagName = "iframe")
        {
            var iframeElements = _driver.FindElements(By.TagName(tagName));
            if (iframeElements.Count <= 0) return;

            // Clear 1st pop-up form if it appears as it is required for cookies
            _driver.SwitchTo().Frame(0);
            try
            {
                var formElementButton = _driver.FindElement(By.XPath(xpath));
                formElementButton?.Click();
            }
            catch (NoSuchElementException)
            {
                // Ignore if button not found in this frame
            }
            finally
            {
                _driver.SwitchTo().DefaultContent();
            }
        }

        [AllureStep("Enter '{0}' in the search box")]
        public void SearchCity(string city)
        {
            const string searchBoxInputId = "searchboxinput";
            var searchBoxInputElement = _driver.FindElement(By.Id(searchBoxInputId));
            if (searchBoxInputElement == null) throw new Exception($"Element with ID: {searchBoxInputId} not found");

            searchBoxInputElement.SendKeys(city);

            // Verification can be here or separate. Test used to assert value.
            var currentValue = searchBoxInputElement.GetAttribute("value");
            if (currentValue != city) throw new Exception($"Expected search value '{city}' but got '{currentValue}'");
        }

        [AllureStep("Click Search")]
        public void ClickSearch()
        {
            const string searchBoxSearchButtonId = "searchbox-searchbutton";
            var searchBoxSearchButtonElement = _driver.FindElement(By.Id(searchBoxSearchButtonId));
            if (searchBoxSearchButtonElement == null) throw new Exception($"Element with ID: {searchBoxSearchButtonId} not found");

            searchBoxSearchButtonElement.Click();
        }

        [AllureStep("Get Headline Text")]
        public string GetHeadlineText()
        {
            const string headlineXpath = "//*[@id=\"pane\"]/div/div[1]/div/div/div[2]/div[1]/div[1]/div[1]/h1/span[1]";
            var headlineElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(headlineXpath)));

            // Original used GetInnerHtml helper.
            // Assuming innerHTML is needed, using IJavaScriptExecutor.
            var javaScriptExecutor = (IJavaScriptExecutor)_driver;
            var innerHtml = javaScriptExecutor.ExecuteScript("return arguments[0].innerHTML;", headlineElement).ToString();
            return innerHtml;
        }

        [AllureStep("Click Directions icon")]
        public void ClickDirections()
        {
            const string directionsButtonXpath = "//button[@class ='iRxY3GoUYUY__button gm2-hairline-border section-action-chip-button']";
            var directionsButton = _driver.FindElement(By.XPath(directionsButtonXpath));
            if (directionsButton == null) throw new Exception($"Element with XPath: {directionsButtonXpath} not found");

            directionsButton.Click();
        }

        [AllureStep("Get Destination Field Value")]
        public string GetDestinationValue()
        {
            const string destinationFieldXpath = "//div[@id=\"sb_ifc52\"]//input[@class=\"tactile-searchbox-input\"]";
            var destinationFieldElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(destinationFieldXpath)));
            return destinationFieldElement.GetAttribute("aria-label");
        }
    }
}
