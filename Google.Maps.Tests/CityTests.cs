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

using NUnit.Allure.Attributes;
using NUnit.Framework;
using Google.Maps.Tests.Pages;

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
            var googleMapsPage = new GoogleMapsPage(Driver);

            // 1. Go to https://www.google.com/maps
            googleMapsPage.GoTo();
            Assert.AreEqual("Google Maps", Driver.Title);
            googleMapsPage.AcceptCookies();

            // 2. Enter city in the search box
            googleMapsPage.SearchCity(searchCityName);

            // 3. Search
            googleMapsPage.ClickSearch();

            // 4. Verify left panel has city as a headline text
            var headlineElementValue = googleMapsPage.GetHeadlineText();
            Assert.AreEqual(searchCityName, headlineElementValue);

            // 5. Click Directions icon
            googleMapsPage.ClickDirections();

            // 6. Verify destination field is city
            var destinationFieldCurrentValue = googleMapsPage.GetDestinationValue();
            Assert.IsTrue(destinationFieldCurrentValue.Contains(searchCityName));
        }
    }
}