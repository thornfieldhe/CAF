using System.Collections.Generic;

namespace CAF
{
    /// <summary>
    /// 正则上下文
    /// </summary>
    public class RegexContex
    {
        public RegexContex(string content) : this(content, RegexOperator.Matches) { this.Content = content; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content">待匹配字符串</param>
        /// <param name="operater">正则操作</param>
        public RegexContex(string content, RegexOperator operater)
        {
            this.Content = content;
            this.Operator = operater;
            this.Matches = new List<string>();
            this.Groups = new Dictionary<int, List<string>>();
        }

        private RegexContex() { }

        public string Content { get; set; }
        public RegexOperator Operator { get; private set; }
        public List<string> Matches { get; set; }
        public Dictionary<int, List<string>> Groups { get; set; }

        public string Replacement;
    }

    public class RegexGroup
    {
    }
}