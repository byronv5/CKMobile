
using System;

namespace CK.Model
{
    /// <summary>
    /// 支付信息实体
    /// </summary>
    public class PayLogInfo
    {
        /// <summary>
        /// 用户openid：微信唯一标识用户身份
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 订单流水号
        /// </summary>
        public string TradeNo { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string CustName { get; set; }
        /// <summary>
        /// 性别：1男；2女
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdentityNo { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelNo { get; set; }
        /// <summary>
        /// 支付金额（单位：元）
        /// </summary>
        public double PayMoney { get; set; }
        /// <summary>
        /// 支付状态：0初始；1成功；-1失败
        /// </summary>
        public int PayStatus { get; set; }
        /// <summary>
        /// 订单创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
