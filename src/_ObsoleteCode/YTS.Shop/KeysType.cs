using YTS.Tools;

namespace YTS.Shop
{
    public class KeysType
    {
        /// <summary>
        /// 用户金额变动记录操作类型
        /// </summary>
        [Explain("用户金额变动记录操作类型")]
        public enum UserMoneyRecordOperateType
        {
            /// <summary>
            /// 充值
            /// </summary>
            [Explain("充值")]
            Recharge = 1,

            /// <summary>
            /// 消费
            /// </summary>
            [Explain("消费")]
            Consumption = 2,

            /// <summary>
            /// 充值退款
            /// </summary>
            [Explain("充值退款")]
            RechargeRefund = 3,

            /// <summary>
            /// 产品退货
            /// </summary>
            [Explain("产品退货")]
            ProductReturnGoods = 4,
        }

        /// <summary>
        /// 产品数量修改记录操作类型
        /// </summary>
        [Explain("产品数量修改记录操作类型")]
        public enum ProductNumberRecordOperateType
        {
            /// <summary>
            /// 入库
            /// </summary>
            [Explain("入库")]
            EnterWarehouse = 1,

            /// <summary>
            /// 售出
            /// </summary>
            [Explain("售出")]
            Sold = 2,

            /// <summary>
            /// 报损
            /// </summary>
            [Explain("报损")]
            Damaged = 3,
        }
    }
}
