namespace CAF
{
    /// <summary>
    /// 正则表达式接口
    /// </summary>
    public interface IExpression
    {
        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="contex"></param>
        void Evaluate(RegexContex contex);
    }
}