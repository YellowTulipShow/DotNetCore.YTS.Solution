using System;
using System.Data.SqlTypes;

namespace YTS.Tools
{
    /// <summary>
    /// 基础数据模型
    /// </summary>
    [Serializable]
    public abstract class AbsBasicDataModel
    {
        public AbsBasicDataModel() { }

        /// <summary>
        /// 错误的不正常的DateTime类型值
        /// </summary>
        public readonly static DateTime ERROR_DATETIME_VALUE = SqlDateTime.MinValue.Value;

        /// <summary>
        /// 深度克隆一个数据模型对象
        /// </summary>
        public AbsBasicDataModel CloneModelData()
        {
            return ReflexHelper.CloneAllAttribute<AbsBasicDataModel>(this);
        }
    }
}
