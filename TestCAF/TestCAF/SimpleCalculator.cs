#region using
using CAF;
#endregion
namespace TestCAF
{
    /// <summary>
    /// �򵥵Ŀɳػ�������󣬽�֧�� + \ -
    /// </summary>
    public class SimpleCalculator : PoolableBase
    {
        public int Add(int x, int y)
        {
            PreProcess();
            return x + y;
        }
        public int Substract(int x, int y)
        {
            PreProcess();
            return x - y;
        }
    }
}
