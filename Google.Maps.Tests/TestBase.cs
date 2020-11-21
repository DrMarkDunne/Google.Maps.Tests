//-----------------------------------------------------------------------
// <copyright file="TestBase.cs" name="Mark Dunne">
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

using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Google.Maps.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureParentSuite("AllTests")]
    public class TestBase
    {
        private static readonly Stopwatch TestRunTimer = new Stopwatch();
        public static readonly DateTime TestRunStartTime = DateTime.Now;
        private static readonly Stopwatch TestCaseTimer = new Stopwatch();
        private static DateTime TestCaseStartTime { get; set; }
        private static readonly ConcurrentDictionary<string, object> TestCaseCount = new ConcurrentDictionary<string, object>();
        public static readonly string TestRunName = RandomString(10);

        public TestBase()
        {
        }

        [OneTimeSetUp]
        public void CleanupResultDirectory()
        {
            TestRunTimer.Start();
            AllureLifecycle.Instance.CleanupResultDirectory();
        }

        [SetUp]
        public void TestSetup()
        {
            try
            {
                TestCaseCount.TryAdd(TestContext.CurrentContext.Test.ID, null);
                TestCaseTimer.Start();
                TestCaseStartTime = DateTime.Now;
                TestContext.WriteLine($"{Environment.NewLine}Test {TestContext.CurrentContext.Test.Name} started execution at {TestCaseStartTime}.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nThere was an exception: {e.Message}");
                throw e;
            }
        }

        [TearDown]
        public static void TestCleanUp()
        {
            var endMessage = "Tear Down complete";
            NUnit.Allure.Core.AllureExtensions.WrapSetUpTearDownParams(() =>
            {
                try
                {
                    TestCaseTimer.Stop();
                    var TestCaseTime = TestCaseTimer.Elapsed;
                    DateTime TestCaseEndTime = TestCaseStartTime + TestCaseTime;

                    var status = TestContext.CurrentContext.Result.Outcome.Status;

                    var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                        ? ""
                        : $"<pre>{TestContext.CurrentContext.Result.StackTrace}</pre>";

                    TestContext.WriteLine(
                        $"Test {TestContext.CurrentContext.Test.Name} finished execution in {TestCaseTime.TotalMinutes} minuntes at {TestCaseEndTime} with result:{status}.");

                    // sending telemetry to Power BI
                    Helpers.LogAutomationResult(testRunComplete: false, testContext: TestContext.CurrentContext, startDateTime: TestCaseStartTime.ToString("yyyy-MM-ddTHH:mm:ss.000Z"), endDateTime: TestCaseEndTime.ToString("yyyy-MM-ddTHH:mm:ss.000Z"), duration: TestCaseTime.TotalMinutes);
                }
                catch (Exception e)
                {
                    endMessage = $"Exception caught during test clean up: {e.Message}";
                    AllureExtensions.ReportIssueStep(AllureLifecycle.Instance, endMessage);
                    throw e;
                }

            }, endMessage);
        }

        /// <summary>
        /// Completes test run
        /// </summary>
        [OneTimeTearDown]
        public static void CompleteTestRun()
        {
            TestRunTimer.Stop();
            var TestRunTime = TestCaseTimer.Elapsed;
            DateTime TestRunEndTime = TestRunStartTime + TestRunTime;
            // Store test run results
            Helpers.LogAutomationResult(testRunComplete: true, testContext: TestContext.CurrentContext, startDateTime: TestRunStartTime.ToString("yyyy-MM-ddTHH:mm:ss.000Z"), endDateTime: TestRunEndTime.ToString("yyyy-MM-ddTHH:mm:ss.000Z"), duration: TestRunTime.TotalMinutes, testRunCount: TestCaseCounted(TestCaseCount));
            // sending telemetry to Power BI
            _ = Helpers.PostDataToPowerBi().Result;
        }

        /// <summary>
        /// Returns the new of test cases run
        /// </summary>
        /// <param name="testCaseCount"></param>
        /// <returns></returns>
        private static int TestCaseCounted(ConcurrentDictionary<string, object> testCaseCount)
        {
            var result = new ConcurrentDictionary<string, object>();

            Parallel.ForEach(testCaseCount, item =>
            {
                result.TryRemove(item.Key, out var _);
                result.TryAdd(item.Key, null);
            });

            return result.Count;
        }

        /// <summary>
        /// Returns a random string for a passed in Length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[new Random().Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
