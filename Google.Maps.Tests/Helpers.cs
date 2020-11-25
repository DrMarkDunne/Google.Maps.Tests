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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Allure.Commons;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Google.Maps.Tests
{
    public class Helpers : TestBase
    {
        private const string Url = "https://api.powerbi.com/beta/86eb5cdc-05d5-4952-b36d-bc110af1e2e5/datasets/80d1bd69-bd6f-4fe0-b3ad-97510f25853e/rows?key=deRV0n%2BiZamKcOBKKyPVu1IIipicqeFUgfj2rWXNOR7OkCTgHjnZLO0tguDD7dXkrGw%2BSELFfyU4ALQm5YwyAA%3D%3D";
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
                    result.TestCaseMessage = testContext.Result.Message.Length > 4000 ? testContext.Result.Message.Substring(0, 4000) : testContext.Result.Message;
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
            childComponentBag ??= new ConcurrentBag<object>();
            childComponentBag.Add(value);
            _ = AutomationResults.TryAdd(key, childComponentBag);
        }

        public static async Task<HttpResponseMessage> PostDataToPowerBi()
        {
            if (AutomationResults is null || AutomationResults.Count <= 0)
                return null;

            var list = new List<object>();
            foreach (var (key, value) in AutomationResults)
            {
                list.AddRange(value);
                AutomationResults.TryRemove(key, out _);
            }
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(Url, list.ToArray());
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
            var iframeElements = driver.FindElements(By.TagName(tagName));
            if (iframeElements.Count <= 0) return;
            // Clear 1st pop-up form if it appears as it is required for cookies
            driver.SwitchTo().Frame(0);
            var formElementButton = driver.FindElement(By.XPath(xpath));
            formElementButton?.Click();
            driver.SwitchTo().DefaultContent();
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
        /// <param name="searchCityName"></param>
        /// <param name="e"></param>
        /// <returns>test status</returns>
        public static Status ReportFailedStepWithScreenshot(IWebDriver driver, string searchCityName, Exception e)
        {
            var screenshotDriver = driver as ITakesScreenshot;
            var screenshot = screenshotDriver.GetScreenshot();
            screenshot.SaveAsFile(@$"{searchCityName}-failed.png");
            AllureLifecycle.Instance.AddAttachment
            (
                @$"{searchCityName}-failed.png"
            );
            AllureLifecycle.Instance.ReportIssueStep(e.Message, status: Status.failed);

            return Status.failed;
        }
    }
}
