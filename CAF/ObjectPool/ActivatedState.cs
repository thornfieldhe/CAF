using System;

namespace CAF.ObjectPool
{
    /// <summary>
    /// 被激活状态
    /// </summary>
    internal sealed class ActivatedState : StateBase
    {
        public ActivatedState() { }

        public override bool Executable
        {
            get { return true; }
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
            item.ChangeState(SingletonBase<DeactivatedState>.Instance);
        }
    }
}
