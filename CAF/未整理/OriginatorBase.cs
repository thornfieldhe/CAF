using System.Collections.Generic;
namespace CAF
{
    /// <summary>
    /// 包括内部备忘录类型的原发器抽象定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OriginatorBase<T>
        where T : new()
    {
        /// <summary>
        /// 用于保存历次备忘信息的堆栈
        /// </summary>
        private Stack<T> stack = new Stack<T>();

        public OriginatorBase()
        {
            this.State = new T();
            SaveCheckpoint();
        }

        /// <summary>
        /// 原发器对象的状态
        /// </summary>
        protected T State { get; set; }

        protected virtual T CreateMemento()
        {
            return this.State.DeepCopy();
        }

        /// <summary>
        /// 把状态保存到备忘录
        /// </summary>
        public virtual void SaveCheckpoint()
        {
            stack.Push(CreateMemento());
        }
        /// <summary>
        /// 从备忘录恢复之前的状态
        /// </summary>
        public virtual void Undo()
        {
            if (stack.Count == 0) return;
            var m = stack.Pop();
            this.State = m;
        }
    }
}
