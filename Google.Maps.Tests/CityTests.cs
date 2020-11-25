//-----------------------------------------------------------------------
// <copyright file="CityTests.cs" name="Mark Dunne">
// Copyright (c) 2020 Mark Dunne. All rights reserved.
// Author: Mark Dunne
//
// You may use, distribute and modify this code under the
// terms of the MIT License.
//
// You should have received a copy of the MIT License with
// this file. If not, please write to: gmarkdunne@gmail.com
// </copyright>
//-----------------------------------------------------------------------

using System;

using Allure.Commons;

using NUnit.Allure.Attributes;
using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

namespace Google.Maps.Tests
{
    [TestFixture(Category = "CityTests")]
    [AllureSuite("CityTests")]
    [AllureDisplayIgnored]
    [Author("Mark Dunne", "gmarkdunne@gmail.com")]
    [Parallelizable(ParallelScope.Self)] // Set to 'All' to force fails
    public class CityTests : TestBase
    {
        [TestCase("Dublin", TestName = "1 - Google Maps - 'Dublin' Test"), Order(1)]
        [TestCase("London", TestName = "2 - Google Maps - 'London' Test")]
        [TestCase("Berlin", TestName = "3 - Google Maps - 'Berlin' Test")]
        [TestCase("Hong Kong", TestName = "4 - Google Maps - 'Hong Kong' Test")]
        [TestCase("Tokyo", TestName = "5 - Google Maps - 'Tokyo' Test")]
        [TestCase("San Francisco", TestName = "6 - Google Maps - 'San Francisco' Test")]
        [TestCase("New York", TestName = "7 - Google Maps - 'New York' Test")]
        public void GoogleMapsCityTest(string searchCityName = "Dublin")
        {
            #region 1. Go to https://www.google.com/maps
            using var driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var id = Guid.NewGuid().ToString();
            var stepResult = new StepResult { name = "1. Go to https://www.google.com/maps" };
            var status = Status.passed;
            AllureLifecycle.Instance.StartStep(id, stepResult);
            try
            {
                // Navigate to example page
                driver.Navigate().GoToUrl("https://www.google.com/maps");
                driver.Manage().Window.Maximize();
                // Assert the page title is what is expected
                Assert.AreEqual("Google Maps", driver.Title);
                // Accept the 'I Agree' to cookies iframe
                Helpers.CloseIframe(driver, "//*[@id=\"introAgreeButton\"]/span/span");
                // Select 'No Thanks' to ad spam iframe
                Helpers.CloseIframe(driver, "//*[@id=\"yDmH0d\"]/c-wiz/div/div/c-wiz/div/div/div/div[2]/div[2]/button");
            }
            catch (AssertionException e)
            {
                status = Helpers.ReportFailedStepWithScreenshot(driver, searchCityName, e);
            }
            AllureLifecycle.Instance.StopStep(step => stepResult.status = status);
            #endregion

            #region 2. Enter Dublin in the search box
            id = Guid.NewGuid().ToString();
            stepResult = new StepResult { name = $"2. Enter '{searchCityName}' in the search box" };
            status = Status.passed;
            AllureLifecycle.Instance.StartStep(id, stepResult);
            try
            {
                const string searchBoxInputId = "searchboxinput";
                var searchBoxInputElement = driver.FindElementById(searchBoxInputId);
                // Assert that the 'searchBoxInputElement' is not null
                Assert.IsNotNull(searchBoxInputElement, $"'{driver.Title}' has no element with ID: {searchBoxInputId}");
                searchBoxInputElement.SendKeys(searchCityName);
                var searchBoxInputElementCurrentValue = searchBoxInputElement.GetAttribute("value");
                // Assert that the current value in the 'searchBoxInputElementCurrentValue' is set to 'searchCityName'
                Assert.AreEqual(searchCityName, searchBoxInputElementCurrentValue);
            }
            catch (AssertionException e)
            {
                status = Helpers.ReportFailedStepWithScreenshot(driver, searchCityName, e);
            }
            AllureLifecycle.Instance.StopStep(step => stepResult.status = status);
            #endregion

            #region 3. Search
            id = Guid.NewGuid().ToString();
            stepResult = new StepResult { name = "3. Search" };
            status = Status.passed;
            AllureLifecycle.Instance.StartStep(id, stepResult);
            try
            {
                const string searchBoxSearchButtonId = "searchbox-searchbutton";
                var searchBoxSearchButtonElement = driver.FindElementById(searchBoxSearchButtonId);
                // Assert that the 'searchBoxSearchButtonElement' is not null
                Assert.IsNotNull(searchBoxSearchButtonElement, $"'{driver.Title}' has no element with ID: {searchBoxSearchButtonId}");
                searchBoxSearchButtonElement.Click();
            }
            catch (AssertionException e)
            {
                status = Helpers.ReportFailedStepWithScreenshot(driver, searchCityName, e);
            }
            AllureLifecycle.Instance.StopStep(step => stepResult.status = status);
            #endregion

            #region 4. Verify left panel has "Dublin" as a headline text
            id = Guid.NewGuid().ToString();
            stepResult = new StepResult { name = $"4. Verify left panel has '{searchCityName}' as a headline text" };
            status = Status.passed;
            AllureLifecycle.Instance.StartStep(id, stepResult);
            try
            {
                // Wait until element appears
                const string headlineXpath = "//*[@id=\"pane\"]/div/div[1]/div/div/div[2]/div[1]/div[1]/div[1]/h1/span[1]";
                var headlineElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(headlineXpath)));
                Assert.IsNotNull(headlineElement, $"'{driver.Title}' has no element with ID: {headlineXpath}");
                var headlineElementValue = Helpers.GetInnerHtml(headlineElement);
                // Assert that the current value in the 'headlineElementValue' is set to 'searchCityName'
                Assert.AreEqual(searchCityName, headlineElementValue);
            }
            catch (AssertionException e)
            {
                status = Helpers.ReportFailedStepWithScreenshot(driver, searchCityName, e);
            }
            AllureLifecycle.Instance.StopStep(step => stepResult.status = status);
            #endregion

            #region 5. Click Directions icon
            id = Guid.NewGuid().ToString();
            stepResult = new StepResult { name = "5. Click Directions icon" };
            status = Status.passed;
            AllureLifecycle.Instance.StartStep(id, stepResult);
            try
            {
                const string directionsButtonXpath = "//button[@class ='iRxY3GoUYUY__button gm2-hairline-border section-action-chip-button']";
                var directionsButton = driver.FindElement(By.XPath(directionsButtonXpath));
                Assert.IsNotNull(directionsButton, $"'{driver.Title}' has no element with ID: {directionsButtonXpath}");
                directionsButton.Click();
            }
            catch (AssertionException e)
            {
                status = Helpers.ReportFailedStepWithScreenshot(driver, searchCityName, e);
            }
            AllureLifecycle.Instance.StopStep(step => stepResult.status = status);
            #endregion

            #region 6. Verify destination field is "Dublin"
            id = Guid.NewGuid().ToString();
            stepResult = new StepResult { name = $"6. Verify destination field is '{searchCityName}'" };
            status = Status.passed;
            AllureLifecycle.Instance.StartStep(id, stepResult);
            try
            {
                const string destinationFieldXpath = "//div[@id=\"sb_ifc52\"]//input[@class=\"tactile-searchbox-input\"]";
                var destinationFieldElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(destinationFieldXpath)));
                var destinationFieldCurrentValue = destinationFieldElement.GetAttribute("aria-label");
                Assert.IsNotNull(destinationFieldCurrentValue, $"'{driver.Title}' has no element with ID: {destinationFieldXpath}");
                // Assert that the current value in the 'destinationFieldCurrentValue' contains 'searchCityName'
                Assert.IsTrue(destinationFieldCurrentValue.Contains(searchCityName));
            }
            catch (AssertionException e)
            {
                status = Helpers.ReportFailedStepWithScreenshot(driver, searchCityName, e);
            }
            AllureLifecycle.Instance.StopStep(step => stepResult.status = status);
            #endregion
        }
    }
}