//-----------------------------------------------------------------------
// <copyright file="Helpers.cs" name="Mark Dunne">
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

using Allure.Commons;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Google.Maps.Tests
{
    public class Helpers : TestBase
    {
        private static readonly string Url = "https://api.powerbi.com/beta/86eb5cdc-05d5-4952-b36d-bc110af1e2e5/datasets/80d1bd69-bd6f-4fe0-b3ad-97510f25853e/rows?key=deRV0n%2BiZamKcOBKKyPVu1IIipicqeFUgfj2rWXNOR7OkCTgHjnZLO0tguDD7dXkrGw%2BSELFfyU4ALQm5YwyAA%3D%3D";
        private static readonly ConcurrentDictionary<string, ConcurrentBag<object>> AutomationResults = new ConcurrentDictionary<string, ConcurrentBag<object>>();

        public static void LogAutomationResult(bool testRunComplete, TestContext testContext, string startDateTime = null, string endDateTime = null, double duration = 0.0, int testRunCount = 0)
        {
            var result = new AutomationResult()
            {
                TestRunDateTime = TestRunStartTime.ToString("yyyy-MM-ddTHH:mm:ss.000Z"),
                TestRunName = TestRunName,
            };
            if (testRunComplete)
            {
                result.TestRunPassCount = testContext.Result.PassCount;
                result.TestRunFailCount = testContext.Result.FailCount;
                result.TestRunCount = testRunCount;
                result.TestRunStartTime = startDateTime;
                result.TestRunEndTime = endDateTime;
                result.TestRunDuration = duration;
                result.TestRunSkipCount = testContext.Result.SkipCount;
                result.TestRunWarningCount = testContext.Result.WarningCount;
                result.TestRunInconclusiveCount = testContext.Result.InconclusiveCount;
                result.TestCaseClassName = testContext.Test.ClassName;

            }
            else
            {
                result.TestCaseName = testContext.Test.Name;
                result.TestCaseOutcome = testContext.Result.Outcome.Status.ToString();
                if (testContext.Result.Message != null)
                {
                    if (testContext.Result.Message.Length > 4000)
                        result.TestCaseMessage = testContext.Result.Message.Substring(0, 4000);
                    else
                        result.TestCaseMessage = testContext.Result.Message;
                }
                result.TestCaseStackTrace = testContext.Result.StackTrace;
                result.TestCaseStartTime = startDateTime;
                result.TestCaseEndTime = endDateTime;
                result.TestCaseDuration = duration;
                result.TestCaseClassName = testContext.Test.ClassName;
                result.TestCaseFullName = testContext.Test.FullName;
                result.TestCaseId = testContext.Test.ID;
                result.TestCaseMethodName = testContext.Test.MethodName;
                result.TestCaseAssertCount = testContext.AssertCount;
            }
            AddResultToCollection(testContext.Test.ClassName, result);
        }

        private static void AddResultToCollection(string key, object value)
        {
            _ = AutomationResults.TryRemove(key, out var childComponentBag);
            if (childComponentBag is null)
                childComponentBag = new ConcurrentBag<object>();
            childComponentBag.Add(value);
            _ = AutomationResults.TryAdd(key, childComponentBag);
        }

        public static async Task<HttpResponseMessage> PostDataToPowerBi()
        {
            if (AutomationResults is null || AutomationResults.Count <= 0)
                return null;

            var list = new List<object>();
            foreach (var item in AutomationResults)
            {
                foreach (var value in item.Value)
                {
                    list.Add(value);
                }
                AutomationResults.TryRemove(item.Key, out _);
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(Url, list.ToArray());
            response.EnsureSuccessStatusCode();

            return response;
        }

        /// <summary>
        /// Close iframe to progress tests
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="xpath"></param>
        /// <param name="tagName"></param>
        public static void CloseIframe(ChromeDriver driver, string xpath, string tagName = "iframe")
        {
            try
            {
                var iframeElements = driver.FindElements(By.TagName(tagName));
                if (iframeElements.Count > 0)
                {
                    // Clear 1st pop-up form if it appears as it is required for cookies
                    driver.SwitchTo().Frame(0);
                    var formElementButton = driver.FindElement(By.XPath(xpath));
                    if (formElementButton != null)
                        formElementButton.Click();
                    driver.SwitchTo().DefaultContent();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Takes the IWebElement and execute JS to return text from inner HTML
        /// </summary>
        /// <param name="element"></param>
        /// <returns>string</returns>
        public static string GetInnerHtml(IWebElement element)
        {
            var remoteWebDriver = (RemoteWebElement)element;
            var javaScriptExecutor = (IJavaScriptExecutor)remoteWebDriver.WrappedDriver;
            var innerHtml = javaScriptExecutor.ExecuteScript("return arguments[0].innerHTML;", element).ToString();

            return innerHtml;
        }

        /// <summary>
        /// Report a failed test case and attached screenshot
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="serchCityName"></param>
        /// <param name="e"></param>
        /// <returns>test status</returns>
        public static Status ReportFailedStepWithScreenshot(IWebDriver driver, string serchCityName, Exception e)
        {
            ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
            Screenshot screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(@$"{serchCityName}-failed.png");
            AllureLifecycle.Instance.AddAttachment
            (
                @$"{serchCityName}-failed.png"
            );
            AllureLifecycle.Instance.ReportIssueStep(e.Message, status: Status.failed);

            return Status.failed;
        }
    }
}
