#region using
using CAF;
#endregion
namespace TestCAF
{
    /// <summary>
    /// ��΢���ӵĵĿɳػ�������󣬽�֧�� *
    /// </summary>
    public class AdvancedCalculator : PoolableBase
    {
        public int Multiple(int x, int y)
        {
            PreProcess();
            return x * y;
        }
    }
}
