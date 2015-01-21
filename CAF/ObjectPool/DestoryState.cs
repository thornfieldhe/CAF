using System;

namespace CAF.ObjectPool
{
    /// <summary>
    /// 构造完成但没有被激活状态
    /// </summary>
    internal sealed class DestoryState : StateBase
    {
        public DestoryState() { }

        public override bool Executable
        {
            get { return false; }
        }

        public override bool Unoccupied
        {
            get { return false; }
        }

        public override void Activate(IPoolable item)
        {
            throw new NotSupportedException();
        }

        public override void Deactivate(IPoolable item)
        {
            throw new NotSupportedException();
        }
    }
}
