using System;
using System.Collections.Generic;

namespace YTS.Tools
{
    /// <summary>
    /// 计算方法
    /// </summary>
    public enum CalcWayEnum
    {
        /// <summary>
        /// 双循环遍历, 对比答案和数据源每一项
        /// </summary>
        DoubleCycle,
        /// <summary>
        /// 单循环遍历, 对比答案和数据源 Index 相同项
        /// </summary>
        SingleCycle,
        /// <summary>
        /// 随机对比几个, 第一个和最后一个一定对比
        /// </summary>
        Random,
    }

    /// <summary>
    /// 验证IList泛型列表记录
    /// </summary>
    public class VerifyIList<TA, TS> : AbsBasicDataModel
    {
        #region === parameter 参数 ===
        /// <summary>
        /// 答案
        /// </summary>
        public IList<TA> Answer = null;

        /// <summary>
        /// 需验证数据源
        /// </summary>
        public IList<TS> Source = null;

        /// <summary>
        /// 方法: 单个选项是否相等
        /// </summary>
        public Func<TA, TS, bool> Func_isEquals
        {
            get
            {
                return !CheckData.IsObjectNull(_func_isEquals) ? _func_isEquals : (a, s) => a.Equals(s);
            }
            set { _func_isEquals = value; }
        }
        private Func<TA, TS, bool> _func_isEquals = null;

        /// <summary>
        /// 方法: 长度不相等时执行回调
        /// </summary>
        public Action<int, int> Func_lengthNotEquals
        {
            get
            {
                return !CheckData.IsObjectNull(_func_lengthNotEquals) ? _func_lengthNotEquals : (alen, slen) =>
                {
                    Console.WriteLine("answer_length: {0} source_length: {1} 不相等", alen, slen);
                };
            }
            set { _func_lengthNotEquals = value; }
        }
        private Action<int, int> _func_lengthNotEquals = null;

        /// <summary>
        /// 方法: 查找失败时回调
        /// </summary>
        public Action<TA> Func_notFind
        {
            get
            {
                if (!CheckData.IsObjectNull(_func_notFind))
                {
                    return _func_notFind;
                }
                else
                {
                    return a =>
                    {
                        // Console.WriteLine("答案选项: {0} 没找到", Json.ToJson(a));
                    };
                }
            }
            set { _func_notFind = value; }
        }
        private Action<TA> _func_notFind = null;

        /// <summary>
        /// 计算方式
        /// </summary>
        public CalcWayEnum ArgCalcWay = CalcWayEnum.DoubleCycle;
        #endregion

        public VerifyIList(CalcWayEnum calc_way)
        {
            this.ArgCalcWay = calc_way;
        }

        public VerifyIList(CalcWayEnum calc_way, IList<TA> answer, IList<TS> source)
        {
            this.ArgCalcWay = calc_way;
            this.Answer = answer;
            this.Source = source;
        }

        /// <summary>
        /// 批量比较计算 答案和数据源 选项是否都相等
        /// </summary>
        /// <returns>都相等: True, 有错误or其中有一个不相等: False</returns>
        public bool Calc()
        {
            if (CheckData.IsObjectNull(Answer))
            {
                Console.WriteLine("Answer 答案为空");
                return false;
            }
            if (CheckData.IsObjectNull(Source))
            {
                Console.WriteLine("Source 数据源为空");
                return false;
            }

            if (CheckData.IsSizeEmpty(Answer) && CheckData.IsSizeEmpty(Source))
            {
                return true;
            }

            if (Answer.Count != Source.Count)
            {
                Func_lengthNotEquals(Answer.Count, Source.Count);
                return false;
            }

            Func<bool> method = GetCalcWayExeMethod();
            return method();
        }

        public Func<bool> GetCalcWayExeMethod()
        {
            switch (ArgCalcWay)
            {
                case CalcWayEnum.DoubleCycle:
                    return () =>
                    {
                        foreach (TA a in Answer)
                        {
                            bool isfind = false;
                            foreach (TS s in Source)
                            {
                                if (Func_isEquals(a, s))
                                {
                                    isfind = true;
                                    break;
                                }
                            }
                            if (!isfind)
                            {
                                Func_notFind(a);
                                return false;
                            }
                        }
                        return true;
                    };
                case CalcWayEnum.SingleCycle:
                    return () =>
                    {
                        for (int i = 0; i < Answer.Count; i++)
                        {
                            if (!Func_isEquals(Answer[i], Source[i]))
                            {
                                Func_notFind(Answer[i]);
                                return false;
                            }
                        }
                        return true;
                    };
                case CalcWayEnum.Random:
                    return () =>
                    {
                        List<int> list = new List<int>();
                        list.Add(0);
                        list.Add(Answer.Count - 1);
                        for (int i = 0; i < RandomData.GetInt(3, 6); i++)
                        {
                            list.Add(RandomData.GetInt(0, Answer.Count));
                        }
                        foreach (int i in list)
                        {
                            if (!Func_isEquals(Answer[i], Source[i]))
                            {
                                Func_notFind(Answer[i]);
                                return false;
                            }
                        }
                        return true;
                    };
                default:
                    return () =>
                    {
                        throw new Exception("对比计算, 没有可执行方法");
                    };
            }
        }
    }
}
