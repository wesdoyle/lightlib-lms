using FluentAssertions;
using LibraryData.Models;
using LibraryService;
using NUnit.Framework;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class DataHelperMethodsShould
    {
        [Test]
        public void Return_Humanized_Business_Hrs_Given_BranchHours()
        {
            var branchHours = new List<BranchHours>
            {
                new BranchHours
                {
                    DayOfWeek = 1,
                    OpenTime = 8,
                    CloseTime = 12
                },

                new BranchHours
                {
                    DayOfWeek = 2,
                    OpenTime = 8,
                    CloseTime = 10
                }
            };

            var expected = new List<string>
            {
                "Monday 08:00 to 12:00",
                "Tuesday 08:00 to 10:00"
            };

            var humanizedHours = DataHelpers.HumanizeBusinessHours(branchHours);
            humanizedHours.Should().BeEquivalentTo(expected);
        }
    }
}