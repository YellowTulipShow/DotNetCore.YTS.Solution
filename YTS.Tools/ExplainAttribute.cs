using System;
using System.Reflection;

namespace YTS.Tools
{
    /// <summary>
    /// 解释特性 同一程序不能多个解释。
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class ExplainAttribute : System.Attribute
    {
        /// <summary>
        /// 错误注释文本
        /// </summary>
        public const string ERROR_EXPLAIN_TEXT = @"未知元素";

        public ExplainAttribute(string explaninStr)
        {
            if (!CheckData.IsStringNull(explaninStr))
            {
                this.Text = explaninStr;
            }
        }

        /// <summary>
        /// 文本注释
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 获得解释特性信息
        /// </summary>
        public static ExplainAttribute Extract(MemberInfo memberInfo)
        {
            ExplainAttribute explainAttr = memberInfo.AttributeFindOnly<ExplainAttribute>();
            if (CheckData.IsObjectNull(explainAttr))
                explainAttr = new ExplainAttribute(ERROR_EXPLAIN_TEXT);
            return explainAttr;
        }
    }
}
