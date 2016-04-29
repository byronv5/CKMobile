using System.Collections.Generic;
using CK.Dal;
using CK.Model;

namespace CK.Bll
{
    public class WxUserBll
    {
        private readonly WxUserDal _userDal = new WxUserDal("WxUserInfo");
        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="info"></param>
        public void InsertUser(WxUserInfo info)
        {
            _userDal.Insert(info);
        }
        /// <summary>
        /// 根据openid获取单条用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public List<WxUserInfo> SearchUserByOpenid(WxUserInfo info)
        {
            return _userDal.List(i => i.openid == info.openid);
        }
        /// <summary>
        /// 根据openid更新单条用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void UpdateUserInfo(WxUserInfo info)
        {
            _userDal.Update(info, i => i.openid == info.openid);
        }
    }
}
