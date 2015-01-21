namespace CAF
{
    /// <summary>
    /// 正则表达式接口
    /// </summary>
    public interface IRegExpression : IExpression
    {
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="expression">待匹配表达式</param>
        /// <returns></returns>
        bool IsMatch(string expression);
    }
}