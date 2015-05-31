using System;

namespace CAF.ObjectPool
{
    /// <summary>
    /// 构造完成但没有被激活状态
    /// </summary>
    internal sealed class ConstructedState : StateBase
    {
        public ConstructedState() { }

        public override bool Executable
        {
            get { return false; }
        }

        public override bool Unoccupied
        {
            get { return true; }
        }

        public override void Activate(IPoolable item)
        {
            item.ChangeState(SingletonBase<ActivatedState>.Instance);
        }

        public override void Deactivate(IPoolable item)
        {
            throw new NotSupportedException();
        }
    }
}
