namespace CAF
{

    public partial class Extensions
    {

        /// <summary>
        /// 是否在范围之间
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="allowEqual">是否包含等于</param>
        /// <returns></returns>
        public static bool IsBetween(this int obj, int max, int min, bool allowEqual)
        {
            if (allowEqual)
            {
                return obj >= min && obj <= max;
            }
            return obj > min && obj < max;
        }
    }
}