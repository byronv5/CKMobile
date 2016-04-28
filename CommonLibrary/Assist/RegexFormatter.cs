using System.Text.RegularExpressions;

namespace CommonLibrary.Assist
{
    public sealed class RegexFormatter
    {
        /// <summary>
        /// 校验用户输入的是否为电信手机号码
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public static bool CheckTeleNo(string no)
        {
            return Regex.IsMatch(no, @"(^133\d{8}$)|(^153\d{8}$)|(^189\d{8}$)|(^180\d{8}$)|(^181\d{8}$)|(^177\d{8}$)");
        }
    }
}
