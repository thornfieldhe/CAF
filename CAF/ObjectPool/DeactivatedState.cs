
namespace CAF.ObjectPool
{
    /// <summary>
    /// 激活后被释放状态
    /// </summary>
    internal sealed class DeactivatedState : StateBase
    {
        public DeactivatedState() { }

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
        }
    }
}
