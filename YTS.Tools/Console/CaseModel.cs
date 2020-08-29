using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace YTS.Tools
{
    /// <summary>
    /// 测试实例模型
    /// </summary>
    public class CaseModel : AbsBasicDataModel
    {
        public CaseModel() { }

        /// <summary>
        /// 名称标志
        /// </summary>
        public string NameSign = @"实例模型初始名称";

        /// <summary>
        /// 执行事件
        /// </summary>
        public Func<bool> ExeEvent = null;

        /// <summary>
        /// 子类实例
        /// </summary>
        public List<CaseModel> SonCases = new List<CaseModel> { };

        /// <summary>
        /// 延时初始化方法, 会在 ExeEvent 执行前执行
        /// </summary>
        public virtual void onInit() { }

        /// <summary>
        /// 清除并写入文件内容, 并在控制台报告文件路径
        /// </summary>
        /// <param name="abs_file_path">文件绝对路径</param>
        /// <param name="content">写入内容</param>
        public void ClearAndWriteFile(string abs_file_path, string content)
        {
            abs_file_path = ConvertTool.ToString(abs_file_path);
            if (!PathHelp.IsAbsolute(abs_file_path))
            {
                Console.WriteLine("需要写入的文件名称错误: {0}", abs_file_path);
                return;
            }
            content = ConvertTool.ToString(content);
            File.Delete(abs_file_path);
            File.AppendAllText(abs_file_path, content);
            Console.WriteLine("清空并写入文件: {0}", abs_file_path);
        }

        /// <summary>
        /// 中断测试实例
        /// </summary>
        public CaseModel BreakCase()
        {
            return new CaseModel()
            {
                NameSign = @"中断测试",
                ExeEvent = () => false,
            };
        }

        /// <summary>
        /// 获取最小时间值, 用于随机取值
        /// </summary>
        public virtual DateTime GetMinDateTime()
        {
            return DateTime.Now.AddMonths(-12);
        }

        /// <summary>
        /// 获取最大时间值, 用于随机取值
        /// </summary>
        public virtual DateTime GetMaxDateTime()
        {
            return DateTime.Now;
        }
    }
    /*
     * 正则:
     * \s*public\s?(virtual|abstract)? (.*) (\w+)\(.*\) \{\s*.*\s*\}
     * 替换:
     *  public CaseModel Func_$3() {
            return new CaseModel() {
                NameSign = @"",
                ExeEvent = () => {
                    return true;
                },
            };
        }
     */
}
