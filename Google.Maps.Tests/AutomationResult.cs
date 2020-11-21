//-----------------------------------------------------------------------
// <copyright file="AutomationResult.cs" name="Mark Dunne">
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

using Newtonsoft.Json;

namespace Google.Maps.Tests
{
    /*
    [
        {
            "testRunDateTime" :"2020-11-02T09:08:55.341Z",
            "testRunName" :"AAAAA555555",
            "testRunPassCount" :98.6,
            "testRunFailCount" :98.6,
            "testRunSkipCount" :98.6,
            "testRunWarningCount" :98.6,
            "testRunInconclusiveCount" :98.6,
            "testRunCount" :98.6,
            "testRunStartTime" :"2020-11-02T09:08:55.341Z",
            "testRunEndTime" :"2020-11-02T09:08:55.341Z",
            "testRunDuration" :98.6,
            "testCaseName" :"AAAAA555555",
            "testCaseOutcome" :"AAAAA555555",
            "testCaseAssertCount" :98.6,
            "testCaseStartTime" :"2020-11-02T09:08:55.341Z",
            "testCaseEndTime" :"2020-11-02T09:08:55.341Z",
            "testCaseDuration" :98.6,
            "testCaseClassName" :"AAAAA555555",
            "testCaseFullName" :"AAAAA555555",
            "testCaseId" :"AAAAA555555",
            "testCaseMethodName" :"AAAAA555555",
            "testCaseMessage" :"AAAAA555555",
            "testCaseStackTrace" :"AAAAA555555"
        }
    ]
    */

    public class AutomationResult
    {
        [JsonProperty("testRunDateTime", Required = Required.Always)]
        public string TestRunDateTime { get; set; }

        [JsonProperty("testRunName", Required = Required.Always)]
        public string TestRunName { get; set; }

        [JsonProperty("testRunPassCount")]
        public double TestRunPassCount { get; set; }

        [JsonProperty("testRunFailCount")]
        public double TestRunFailCount { get; set; }

        [JsonProperty("testRunSkipCount")]
        public double TestRunSkipCount { get; set; }

        [JsonProperty("testRunWarningCount")]
        public double TestRunWarningCount { get; set; }

        [JsonProperty("testRunInconclusiveCount")]
        public double TestRunInconclusiveCount { get; set; }

        [JsonProperty("testRunCount")]
        public double TestRunCount { get; set; }

        [JsonProperty("testRunStartTime")]
        public string TestRunStartTime { get; set; }

        [JsonProperty("testRunEndTime")]
        public string TestRunEndTime { get; set; }

        [JsonProperty("testRunDuration")]
        public double TestRunDuration { get; set; }

        [JsonProperty("testCaseName")]
        public string TestCaseName { get; set; }

        [JsonProperty("testCaseOutcome")]
        public string TestCaseOutcome { get; set; }

        [JsonProperty("testCaseAssertCount")]
        public double TestCaseAssertCount { get; set; }

        [JsonProperty("testCaseStartTime")]
        public string TestCaseStartTime { get; set; }

        [JsonProperty("testCaseEndTime")]
        public string TestCaseEndTime { get; set; }

        [JsonProperty("testCaseDuration")]
        public double TestCaseDuration { get; set; }

        [JsonProperty("testCaseClassName")]
        public string TestCaseClassName { get; set; }

        [JsonProperty("testCaseFullName")]
        public string TestCaseFullName { get; set; }

        [JsonProperty("testCaseId")]
        public string TestCaseId { get; set; }

        [JsonProperty("testCaseMethodName")]
        public string TestCaseMethodName { get; set; }

        [JsonProperty("testCaseMessage")]
        public string TestCaseMessage { get; set; }

        [JsonProperty("testCaseStackTrace")]
        public string TestCaseStackTrace { get; set; }
    }
}
