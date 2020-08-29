using System;
using System.Linq;
using System.ComponentModel;
using YTS.Tools;

namespace YTS.Test
{
    public abstract class AbsTestFramework<TItem> : ITestFramework<TItem> where TItem : ITestItem
    {
        private ITestOutput output;

        public long testItemCount = 0;
        public long testItemSuccessCount = 0;
        public long testItemErrorCount = 0;

        public AbsTestFramework(ITestOutput output)
        {
            this.output = output;
        }

        public virtual void OnInit()
        {
            testItemCount = 0;
            testItemSuccessCount = 0;
            testItemErrorCount = 0;
        }

        public virtual void OnExecute(ITestTemplate<TItem> temp, ITestFactory<TItem> factory)
        {
            OnExecute((ITestTemplate)temp, (ITestItem)factory);
        }

        public virtual void OnExecute(ITestTemplate<TItem> temp, TItem item)
        {
            OnExecute((ITestTemplate)temp, (TItem)item);
        }

        public virtual void OnExecute(ITestTemplate temp, ITestFactory factory)
        {
            var items = factory.GetItems();
            if (items.Count <= 0)
            {
                output.WriteLineError(@"(→_→) => 并没有需要测试实例子弹, 怎么打仗? 快跑吧~ running~ running~ running~ ");
                return;
            }
            foreach (var item in items)
            {
                OnExecute(temp, item);
            }
        }

        public virtual void OnExecute(ITestTemplate temp, ITestItem item)
        {
            Type item_type = item.GetType();
            string description = item_type.GetCustomAttributeValue<DescriptionAttribute, string>(m => m.Description);
            if (string.IsNullOrWhiteSpace(description))
            {
                description = item_type.FullName;
            }

            OnExecute(temp, item, description);
        }

        public virtual void OnExecute(ITestTemplate temp, ITestItem item, string description)
        {
            testItemCount += 1;

            output.Write($"[{description}]: ");

            try
            {
                decimal exeTime = temp.ExecuteTest(item);
                output.WriteInfo($"测试通过。");
                output.Write($"执行时间: ");
                output.WriteInfo(exeTime.ToString());
                output.WriteLine(string.Empty);
                testItemSuccessCount += 1;
            }
            catch (TestFailException ex)
            {
                testItemErrorCount += 1;

                output.WriteError($"测试失败!");
                output.WriteLine(string.Empty);
                output.WriteLine(ex.Message);
                var info = ex.Info;
                if (info != null)
                {
                    output.WriteLine($"测试数据还原:");

                    output.Write($"输入参数: ");
                    string args = string.Join(", ", info.Args.Select(b => b.ToJSONString()).ToArray());
                    output.WriteWarning($"{args}");
                    output.WriteLine(string.Empty);

                    output.Write($"预期结果: ");
                    string answers = string.Join(", ", info.Answer.Select(b => b.ToJSONString()).ToArray());
                    output.WriteWarning($"{answers}");
                    output.WriteLine(string.Empty);

                    output.Write($"计算结果: ");
                    output.WriteError($"{info.Result.ToJSONString()}");
                    output.WriteLine(string.Empty);
                }
            }
            catch (Exception ex)
            {
                testItemErrorCount += 1;

                output.WriteError($"测试异常!");
                output.WriteLineWarning($"异常内容: {ex.Message}");
                output.WriteLine($"堆栈异常: {ex.StackTrace}");

                throw ex;
            }

            temp.ExecuteEnd(item);
        }

        public virtual void OutputEndResult()
        {
            output.WriteLine(string.Empty);

            output.Write($"\t成功测试实例数: ");
            if (testItemCount == testItemSuccessCount)
            {
                output.WriteInfo($"{testItemSuccessCount}");
            }
            else
            {
                output.WriteWarning($"{testItemSuccessCount}");
            }

            output.WriteLine(string.Empty);

            output.Write($"\t失败测试实例数: ");
            if (0 == testItemErrorCount)
            {
                output.Write($"{testItemErrorCount}");
            }
            else
            {
                output.WriteError($"{testItemErrorCount}");
            }

            output.WriteLine(string.Empty);
            output.WriteLine(string.Empty);
        }
    }
}
