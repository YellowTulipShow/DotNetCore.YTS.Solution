using System;
using System.Text.RegularExpressions;

using Xunit;

namespace YTS.Tools.Test.Time
{
    public class TestRegex
    {
        [Theory]
        [InlineData(true, "2021-03-15 12:15:44", new string[] { "2021", "03", "15", "12", "15", "44" })]
        [InlineData(false, "2021-031-15 12:15:44", new string[] { "2021", "03", "15", "12", "15", "44" })]
        public void TestToResultList(bool isSuccess, string timeInput, string[] rlist)
        {
            Regex re = new Regex(@"^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})$",
                RegexOptions.ECMAScript | RegexOptions.IgnoreCase);
            Match match = re.Match(timeInput);
            MatchCollection matchCollection = re.Matches(timeInput);

            Assert.True(match.Success == isSuccess);
            if (!isSuccess)
            {
                return;
            }
            Assert.True(match.Groups[0].Value == timeInput);
            Assert.True(match.Groups[0].Value == matchCollection[0].Groups[0].Value);
            for (int i = 0; i < rlist.Length; i++)
            {
                Assert.True(match.Groups[i + 1].Value == rlist[i]);
            }
        }
    }
}
