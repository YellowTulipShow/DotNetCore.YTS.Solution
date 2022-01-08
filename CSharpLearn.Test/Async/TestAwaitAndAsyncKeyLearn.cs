using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using System.Threading.Tasks;
using System.Net.Http;

namespace CSharpLearn.Test.Async
{
    public class TestAwaitAndAsyncKeyLearn
    {
        private IList<string> logs;
        [Fact]
        public void Exe()
        {
            logs = new List<string>();
            Task<int> task = GetUrlContentLengthAsync();
            task.Wait();
            int index = Task.WaitAny(task);
            //Task.WaitAll(task);
            int len = task.Result;
            logs.Add(len.ToString());
            Assert.Equal("GetStringAsync,Working...,1", string.Join(",", logs));
        }

        public async Task<int> GetUrlContentLengthAsync()
        {
            var client = new HttpClient();
            logs.Add("GetStringAsync");
            Task<string> getStringTask = client.GetStringAsync("https://docs.microsoft.com/dotnet");
            logs.Add("Working...");
            string contents = await getStringTask;
            return contents.Length;
        }
    }
}
