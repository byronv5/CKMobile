using System.Configuration;
using CK.Model;

namespace CK.Dal
{
    public class PayLogDal : MongodbHelper<PayLogInfo>
    {
        private static readonly string ConnectionStr = ConfigurationManager.AppSettings["ConnectionStr"];
        /// <summary>
        /// 构造函数初始化数据库
        /// </summary>
        /// <param name="clctName">集合名称</param>
        public PayLogDal(string clctName) : base(ConnectionStr, clctName) { }
    }
}
