using System.Collections.Generic;
using CK.Dal;
using CK.Model;

namespace CK.Bll
{
    public class PayLogBll
    {
        private readonly PayLogDal _payDal = new PayLogDal("Paylog");
        /// <summary>
        /// 插入支付日志
        /// </summary>
        /// <param name="info"></param>
        public void InsertLog(PayLogInfo info)
        {
            _payDal.Insert(info);
        }
        /// <summary>
        /// 根据身份证号码获取单条支付记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<PayLogInfo> SearchLogById(PayLogInfo info)
        {
            return _payDal.List(i => i.IdentityNo == info.IdentityNo);
        }
        /// <summary>
        /// 根据订单编号获取单条支付记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public PayLogInfo SearchLogByOrderId(PayLogInfo info)
        {
            return _payDal.Single(i => i.TradeNo == info.TradeNo);
        }
        /// <summary>
        /// 根据订单编号更新单条支付记录
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void UpdaPayLogInfo(PayLogInfo info)
        {
            _payDal.Update(info, i => i.TradeNo == info.TradeNo);
        }
    }
}
