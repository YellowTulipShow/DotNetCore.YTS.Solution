using System;
using YTS.Shop.Models;
using YTS.Tools;

namespace YTS.Shop.Tools
{
    public class UserOperate
    {
        protected YTSEntityContext db;
        protected Managers manager;
        public UserOperate(YTSEntityContext db, Managers manager)
        {
            this.db = db;
            this.manager = manager;
        }

        /// <summary>
        /// 添加用户充值记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="userRechargeSet">充值设置</param>
        /// <param name="model">用户充值记录</param>
        public void AddUserRechargeRecord(Users user, UserRechargeSet userRechargeSet, UserRechargeRecord model)
        {
            model.AddManagerID = manager.ID;
            model.AddTime = DateTime.Now;
            model.GiveAwayMoney = model.GiveAwayMoney ?? 0;
            model.ProjectName = model.ProjectName ?? "自定义用户充值";
            model.UserID = user.ID;
            model.UserName = user.Name ?? user.NickName;
            model.UserRechargeSetID = userRechargeSet?.ID;
            db.UserRechargeRecord.Add(model);

            double RechargeMoney = model.RechargeMoney + (model.GiveAwayMoney ?? 0);

            // 用户金额修改
            user.Money += RechargeMoney;

            // 增加用户金额变动记录
            db.UserMoneyRecord.Add(new UserMoneyRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                OperateType = (int)KeysType.UserMoneyRecordOperateType.Recharge,
                RelatedSign = $"充值配置:{userRechargeSet.ProjectName}({userRechargeSet.ID})",
                UserID = user.ID,
                UserName = user.Name ?? user.NickName,
                OperateMoney = RechargeMoney,
                Remark = $"用户({model.UserName})充值:{RechargeMoney}",
            });
        }

        /// <summary>
        /// 添加用户退款记录
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="model">用户退款记录</param>
        public void AddUserRefundMoneyRecord(Users user, UserRefundMoneyRecord model)
        {
            model.AddManagerID = manager.ID;
            model.AddTime = DateTime.Now;
            model.UserID = user.ID;
            model.UserName = user.Name ?? user.NickName;
            model.SumGiveAwayMoney = model.SumGiveAwayMoney ?? 0;
            model.SumRechargeMoney = model.SumRechargeMoney ?? 0;
            db.UserRefundMoneyRecord.Add(model);

            double RefundMoney = model.ActualRefundMoney * -1;

            // 用户金额修改
            user.Money += RefundMoney;

            // 增加用户金额变动记录
            db.UserMoneyRecord.Add(new UserMoneyRecord()
            {
                AddManagerID = manager.ID,
                AddTime = DateTime.Now,
                OperateType = (int)KeysType.UserMoneyRecordOperateType.RechargeRefund,
                RelatedSign = "",
                UserID = user.ID,
                UserName = user.Name ?? user.NickName,
                OperateMoney = RefundMoney,
                Remark = $"用户({model.UserName})退款:{RefundMoney}",
            });
        }
    }
}
