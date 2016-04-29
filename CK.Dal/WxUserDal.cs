using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CK.Model;

namespace CK.Dal
{
    public class WxUserDal: MongodbHelper<WxUserInfo>
    {
        private static readonly string ConnectionStr = ConfigurationManager.AppSettings["ConnectionStr"];
        /// <summary>
        /// 构造函数初始化数据库
        /// </summary>
        /// <param name="clctName">集合名称</param>
        public WxUserDal(string clctName) : base(ConnectionStr, clctName) { }
    }
}
