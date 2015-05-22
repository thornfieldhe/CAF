namespace CAF
{
    /// <summary>
    /// 正则匹配字符串
    /// </summary>
    public struct StringRegExpression
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public static string Email = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        /// <summary>
        /// 非负整数
        /// </summary>
        public static string SignlessInteger = @"[0-9]*[1-9][0-9]*";
        /// <summary>
        /// 正整数
        /// </summary>
        public static string NonnegativeInteger = @"\d+";
        /// <summary>
        /// 非正整数
        /// </summary>
        public static string NonPositiveInteger = @"((-\d+)|(0+))";
        /// <summary>
        /// 负整数
        /// </summary>
        public static string NonnegativeNumber = @"-[0-9]*[1-9][0-9]*";
        /// <summary>
        /// 整数
        /// </summary>
        public static string Integer = @"(-)?\d+";
        /// <summary>
        /// 非负浮点数
        /// </summary>
        public static string NonNegativeDecimal = @"\d+(\.\d+)?";
        /// <summary>
        /// 正浮点数
        /// </summary>
        public static string PositiveDecimal = @"(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))";
        /// <summary>
        /// 非正浮点数
        /// </summary>
        public static string NonPositiveDecimal = @"((-\d+(\.\d+)?)|(0+(\.0+)?))";
        /// <summary>
        /// 负浮点数
        /// </summary>
        public static string NegativeDecimal = @"(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))";
        /// <summary>
        /// 浮点数
        /// </summary>
        public static string Decimal = @"(-?\d+)(\.\d+)?";
        /// <summary>
        /// 字母
        /// </summary>
        public static string Letters = @"[A-Za-z]+";
        /// <summary>
        /// 大写字母
        /// </summary>
        public static string UpperLetters = @"[A-Z]+";
        /// <summary>
        /// 小写字母
        /// </summary>
        public static string LowLetters = @"[a-z]+";
        /// <summary>
        /// 数字和字母
        /// </summary>
        public static string LetterAndNumbers = @"[A-Za-z0-9]+";
        /// <summary>
        /// 数字和字母以及下划线
        /// </summary>
        public static string LetterAndNumbersAndUnderline = @"\w+";
        /// <summary>
        /// Url地址
        /// </summary>
        public static string UrlExpression = @"[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?";
        /// <summary>
        /// 日期格式1：年-月-日
        /// </summary>
        public static string DateFormate1 = @"(d{2}|d{4})-((0([1-9]{1}))|(1[1|2]))-(([0-2]([1-9]{1}))|(3[0|1]))";
        /// <summary>
        /// 日期格式2：月/日/年
        /// </summary>
        public static string DateFormate2 = @"((0([1-9]{1}))|(1[1|2]))/(([0-2]([1-9]{1}))|(3[0|1]))/(d{2}|d{4})";
        /// <summary>
        /// 日期格式3：YYYY-MM-DD(比较准确)
        /// </summary>
        public static string DateFormate3 = @"((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-
(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-
(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))";
        /// <summary>
        /// Ip地址
        /// </summary>
        public static string IpAddress = @"(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5]).
(d{1,2}|1dd|2[0-4]d|25[0-5]).(d{1,2}|1dd|2[0-4]d|25[0-5])";
        /// <summary>
        /// 中文
        /// </summary>
        public static string ChineseCharacter = @"([\u4e00-\u9fa5]+|[a-zA-Z0-9]+)";
        /// <summary>
        /// 身份证
        /// </summary>
        public static string IdentityCard = "d{15}|d{18}";
        /// <summary>
        /// HTML标签
        /// </summary>
        public static string HTMLTag = @"<(S*?)[^>]*>.*?|<.*?/>";
        /// <summary>
        /// 空白行
        /// </summary>
        public static string EmputyLine=@"\n\s*\r";
    }
}
