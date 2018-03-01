using NUnit.Framework;

namespace Library.Tests
{
    [TestFixture]
    public class DataHelperMethodsShould
    {
        [Test]
        public void Should_Be_True()
        {
            Assert.Pass();
        }

        //[Test]
        //public void Return_Humanized_Business_Hrs_Given_BranchHours()
        //{
        //    var branchHours = new List<BranchHours>
        //    {
        //        new BranchHours
        //        {
        //            DayOfWeek = 1,
        //            OpenTime = 8,
        //            CloseTime = 12
        //        },

        //        new BranchHours
        //        {
        //            DayOfWeek = 2,
        //            OpenTime = 8,
        //            CloseTime = 10
        //        }
        //    };

        //    var expected = new List<string>();

        //    DataHelpers.HumanizeBusinessHours(branchHours)
        //        .Should().BeEquivalentTo(expected);
        //}
    }
}