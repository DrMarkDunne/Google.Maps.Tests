//-----------------------------------------------------------------------
// <copyright file="AllureExtensions.cs" name="Mark Dunne">
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Allure.Commons;

namespace Google.Maps.Tests
{
    /// <summary>
    /// Extensions for Allure report template
    /// </summary>
    public static class AllureExtensions
    {
        /// <summary>
        /// Reporting issue step
        /// </summary>
        /// <param name="lifecycle"></param>
        /// <param name="stepName"></param>
        /// <param name="callerName"></param>
        /// <param name="status"></param>
        public static void ReportIssueStep(this AllureLifecycle lifecycle, string stepName = "", [CallerMemberName] string callerName = "", Status status = Status.broken)
        {
            if (string.IsNullOrEmpty(stepName))
                stepName = callerName;
            var id = Guid.NewGuid().ToString();
            var stepResult = new StepResult { name = stepName };
            lifecycle.StartStep(id, stepResult);
            lifecycle.StopStep(step => stepResult.status = status);
        }

        /// <summary>
        /// Attach Link to report
        /// </summary>
        /// <param name="lifecycle"></param>
        /// <param name="url"></param>
        public static void AttachLinkToReport(this AllureLifecycle lifecycle, string url = "")
        {
            lifecycle.UpdateTestCase(tc =>
            {
                tc.links = new List<Link> { new Link { name = url, type = "Failure", url = url } };
                tc.uuid = lifecycle.ToString();
                tc.description = "URL to the page where the issue occurred is in the 'Links' section below." + Environment.NewLine;
            });
        }
    }
}
