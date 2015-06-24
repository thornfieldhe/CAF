using System;
using System.Text.RegularExpressions;

namespace CAF
{
    using System.Linq;

    /// <summary>
    /// 正则匹配基类
    /// </summary>
    public class RegExpressionBase : IRegExpression
    {
        protected Regex regex;

        public RegExpressionBase(string expression)
        {
            this.regex = new Regex(expression, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool IsMatch(string content)
        {
            return this.regex.IsMatch(content);
        }

        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="contex"></param>
        public void Evaluate(RegexContex contex)
        {
            if (contex == null)
            {
                throw new ArgumentNullException("contex");
            }
            switch (contex.Operator)
            {
                case RegexOperator.Matches:
                    this.EvaluateMatch(contex);
                    break;
                case RegexOperator.Replace:
                    this.EvaluateReplace(contex);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 通过Match方式解析表达式
        /// </summary>
        /// <param name="context"></param>
        protected virtual void EvaluateMatch(RegexContex context)
        {
            context.Matches.Clear();
            context.Groups.Clear();
            var coll = this.regex.Matches(context.Content);
            if (coll.Count == 0)
            {
                return;
            }
            var groupCount = 0;
            foreach (Match match in coll)
            {
                context.Matches.Add(match.Value);
                this.GetMaxInt(match.Groups.Count, ref groupCount);
            }
            for (var i = 0; i < groupCount; i++)
            {
                var groupItems = (from Match match in coll where match.Groups[i] != null select match.Groups[i].Value).ToList();
                context.Groups.Add(i, groupItems);
            }
        }

        /// <summary>
        /// 通过Replace方式替换表达式内容
        /// </summary>
        /// <param name="contex"></param>
        protected virtual void EvaluateReplace(RegexContex contex)
        {
            contex.Content = this.regex.Replace(contex.Content, contex.Replacement);
        }

        private void GetMaxInt(int resoult, ref int source)
        {
            if (resoult > source)
            {
                source = resoult;
            }
        }
    }
}