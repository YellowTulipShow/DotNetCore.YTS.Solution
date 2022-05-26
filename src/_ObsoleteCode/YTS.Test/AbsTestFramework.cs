using System;
using System.Linq;
using System.ComponentModel;
using YTS.Tools;

namespace YTS.Test
{
    /// <summary>
    /// 抽象类: 泛型测试框架, 为多种测试实例预留的接口
    /// </summary>
    /// <typeparam name="TItem">接口: 测试实例</typeparam>
    public abstract class AbsTestFramework<TItem> : ITestFramework<TItem> where TItem : ITestItem
    {
        private ITestOutput output;

        private long testItemCount = 0;
        private long testItemSuccessCount = 0;
        private long testItemErrorCount = 0;

        /// <summary>
        /// 初始化抽象类: 泛型测试框架
        /// </summary>
        /// <param name="output">实例化测试输出接口</param>
        public AbsTestFramework(ITestOutput output)
        {
            this.output = output;
        }

        /// <summary>
        /// 初始化事件方法
        /// </summary>
        public virtual void OnInit()
        {
            testItemCount = 0;
            testItemSuccessCount = 0;
            testItemErrorCount = 0;
        }

        /// <summary>
        /// 根据测试模板处理相应的测试工厂
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="factory">测试工厂</param>
        public virtual void OnExecute(ITestTemplate<TItem> temp, ITestFactory<TItem> factory)
        {
            OnExecute((ITestTemplate)temp, (ITestItem)factory);
        }

        /// <summary>
        /// 根据测试模板处理相应的测试实例
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="item">测试实例</param>
        public virtual void OnExecute(ITestTemplate<TItem> temp, TItem item)
        {
            OnExecute((ITestTemplate)temp, (TItem)item);
        }

        /// <summary>
        /// 根据测试模板处理测试实例工厂提供的测试实例
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="factory">测试实例供应工厂</param>
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

        /// <summary>
        /// 根据测试模板处理测试实例
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="item">测试实例</param>
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

        /// <summary>
        /// 根据测试模板处理相应的测试实例, 额外的测试描述
        /// </summary>
        /// <param name="temp">测试模板</param>
        /// <param name="item">测试实例</param>
        /// <param name="description">测试描述</param>
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

        /// <summary>
        /// 输出测试结果
        /// </summary>
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
