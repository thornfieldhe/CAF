using System;
using System.Reflection;

namespace CAF.Core
{
    /// <summary>
    /// 创建步骤属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BuildStepAttribute : Attribute, IComparable
    {
        private MethodInfo m_handler;

        public BuildStepAttribute() { }

        /// <summary>
        /// 通过配置文件获取该方法的执行次数和顺序
        /// </summary>
        /// <param name="name"></param>
        public BuildStepAttribute(string name) { this.Name = name; }

        /// <summary>
        /// 设置方法执行次数和顺序
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="times"></param>
        private BuildStepAttribute(int sequence, int times,string name)
        {
            this.Sequence = sequence;
            this.Times = times;
            this.Name = name;
        }

        /// <summary>
        /// 设置方法执行顺序，默认次数1次
        /// </summary>
        /// <param name="sequence"></param>
        public BuildStepAttribute(int sequence) : this(sequence, 1, Guid.NewGuid().ToString()) { }

        /// <summary>
        /// 设置方法执行顺序，默认名称为新Guid
        /// </summary>
        /// <param name="sequence"></param>
        public BuildStepAttribute(int sequence, int times) : this(sequence, times, Guid.NewGuid().ToString()) { }

        /// <summary>
        /// 方法Handler
        /// </summary>
        public MethodInfo Handler
        {
            get { return m_handler; }
            set { this.m_handler = value; }
        }

        /// <summary>
        /// 执行顺序
        /// </summary>
        public int Sequence { get; internal set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int Times { get; internal set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; internal set; }

        public int CompareTo(object target)
        {
            if ((target == null || target.GetType() != typeof(BuildStepAttribute)))
            {
                throw new ArgumentException("target");
            }
            return this.Sequence - ((BuildStepAttribute)target).Sequence;
        }

    }
}
