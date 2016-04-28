using System.Text.RegularExpressions;

namespace CommonLibrary.Assist
{
    public class RegexClass
    {
        /// <summary>
        /// 校验用户输入的是否为电信手机号码
        /// </summary>
        /// <param name="userTelNum"></param>
        /// <returns></returns>
        public static bool IsCtccNo(string userTelNum)
        {
            return Regex.IsMatch(userTelNum, @"(^133\d{8}$)|(^153\d{8}$)|(^189\d{8}$)|(^180\d{8}$)|(^181\d{8}$)|(^177\d{8}$)");
        }

        /// <summary>
        /// 验证是否是手机号
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static bool IsMobileNo(string no)
        {
            return Regex.IsMatch(no, @"^1[3|5|7|8|]\d{9}$");
        }
    }
}
