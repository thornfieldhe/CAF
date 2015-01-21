using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CAF
{
    public static class ArrayExt
    {
        /// <summary>
        /// 快速反转数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagetArray"></param>
        public static void Reversal<T>(this T[] tagetArray)
        {
            T tempHolder = default(T);
            if (tagetArray == null)
            {
                throw new ArgumentNullException("tagetArray");
            }
            if (tagetArray.Length > 0)
            {
                for (int counter = 0; counter < (tagetArray.Length / 2); counter++)
                {
                    tempHolder = tagetArray[counter];
                    tagetArray[counter] = tagetArray[tagetArray.Length - counter - 1];
                    tagetArray[tagetArray.Length - counter - 1] = tempHolder;
                }
            }
            else
            {
                Trace.WriteLine("Nothing to reverse");
            }
        }

        /// <summary>
        /// 交换数组中两个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tagetArray"></param>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        public static void Swap<T>(this T[] tagetArray, int indexA, int indexB)
        {
            T tempHolder = tagetArray[indexA];
            tagetArray[indexA] = tagetArray[indexB];
            tagetArray[indexB] = tempHolder;
        }
    }
}
