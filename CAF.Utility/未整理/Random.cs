using System;

namespace CAF.Utility
{
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 使用Random类生成伪随机数
    /// </summary>
    public class Randoms
    {
        #region 生成一个指定范围的随机整数
        /// <summary>
        /// 生成一个指定范围的随机整数，该随机数范围包括最小值，但不包括最大值
        /// </summary>
        /// <param name="minNum">最小值</param>
        /// <param name="maxNum">最大值</param>
        public static int GetRandomInt(int minNum, int maxNum)
        {
            var random = new Random();
            return random.Next(minNum, maxNum);
        }
        #endregion

        #region 生成一个0.0到1.0的随机小数
        /// <summary>
        /// 生成一个0.0到1.0的随机小数
        /// </summary>
        public static double GetRandomDouble()
        {
            var random = new Random();
            return random.NextDouble();
        }
        #endregion

        #region 对一个数组进行随机排序
        /// <summary>
        /// 对一个数组进行随机排序
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="arr">需要随机排序的数组</param>
        public static void GetRandomArray<T>(T[] arr)
        {
            //对数组进行随机排序的算法:随机选择两个位置，将两个位置上的值交换

            //交换的次数,这里使用数组的长度作为交换次数
            var count = arr.Length;

            //开始交换
            for (var i = 0; i < count; i++)
            {
                //生成两个随机数位置
                var randomNum1 = GetRandomInt(0, arr.Length);
                var randomNum2 = GetRandomInt(0, arr.Length);

                //定义临时变量
                T temp;

                //交换两个随机数位置的值
                temp = arr[randomNum1];
                arr[randomNum1] = arr[randomNum2];
                arr[randomNum2] = temp;
            }
        }



        private static int rep = 0;
        /// <summary>
        /// 生成不重复数字字符串 
        /// </summary>
        public static string GenerateCheckCodeNum(int codeCount)
        {
            var rep = 0;
            var str = string.Empty;
            var num2 = DateTime.Now.Ticks + rep;
            rep++;
            var random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (var i = 0; i < codeCount; i++)
            {
                var num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        /// <summary>
        /// 随机生成字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string GenerateCheckCode(int codeCount)
        {
            var str = string.Empty;
            var num2 = DateTime.Now.Ticks + rep;
            rep++;
            var random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (var i = 0; i < codeCount; i++)
            {
                char ch;
                var num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 生成随机常用汉字
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        public  static string GenerateChinese(int maxLength)
        {
            return  GetRandomCode(maxLength, Const.SimplifiedChinese);
        }


        /// <summary>
        /// 生成随机字母，不出现汉字和数字
        /// </summary>
        /// <param name="maxLength">最大长度</param>
        public static string GenerateLetters(int maxLength)
        {
            return GetRandomCode(maxLength, Const.Letters);
        }


        /// <summary>
        /// 生成随机布尔值
        /// </summary>
        public static bool GenerateBool()
        {
            var random = GetRandomInt(1, 3);
            if (random == 1)
                return false;
            return true;
        }

        /// <summary>
        /// 生成随机日期
        /// </summary>
        /// <param name="beginYear">起始年份</param>
        /// <param name="endYear">结束年份</param>
        public static DateTime GenerateDate(int beginYear = 2000, int endYear = 2030)
        {
            var year = GetRandomInt(beginYear, endYear);
            var month = GetRandomInt(1, 13);
            var day = GetRandomInt(1, 29);
            var hour = GetRandomInt(1, 24);
            var minute = GetRandomInt(1, 60);
            var second = GetRandomInt(1, 60);
            return new DateTime(year, month, day, hour, minute, second);
        }

        /// <summary>
        /// 生成随机枚举
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        public static T GenerateEnum<T>()
        {
            var list = Enum.GetItems<T>();
            int index = GetRandomInt(0, list.Count);
            return Enum.GetInstance<T>(list[index].Value);
        }

        #endregion


        #region 从字符串里随机得到，规定个数的字符串.
        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar">字符规范，如果等于null时，默认值为："1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"</param>
        /// <param name="codeCount">需要生成的随机数个数</param>
        /// <returns></returns>
        public static string GetRandomCode(int codeCount, string allChar = null)
        {
            if (string.IsNullOrEmpty(allChar))
            {
                allChar = "123456789ABCDEFGHiJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            }
            var randomCode = "";
            var temp = -1;
            var rand = new Random(Guid.NewGuid().GetHashCode());
            for (var i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                var t = rand.Next(allChar.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allChar.Length - 1);
                }

                temp = t;
                randomCode += allChar[t];
            }
            return randomCode;
        }

        #endregion


        #region 随机数操作函数
        /// <summary>取得随机数(数字),用yyMMddhhmmss + (xxx),共15位数字</summary>
        /// <returns></returns>
        public static string GetDateRnd()
        {
            var dtTmp = DateTime.Now;
            return dtTmp.ToString("yyMMddhhmmss") + GetRndNum(3);
        }

        /// <summary> 取得随机数(字母+数字),用yyMMddhhmmss + (xxx),共15位字母或数字,</summary>
        /// <returns></returns>
        public static string GetRndKey()
        {
            var dtTmp = DateTime.Now;
            return dtTmp.ToString("yyMMddhhmmss") + GetRndNum(3, true);
        }

        /// <summary> 取得n位随机整数,:45546</summary>
        /// <param name="n">随机数长度</param>
        /// <param name="isStr">true=随机字母和整数，false=随机整数</param>
        /// <returns></returns>
        public static string GetRndNum(int n, bool isStr = false)
        {
            var cChar = "0123456789";
            if (isStr)
            {
                cChar = "abcdefghijklmnopqrstuvwxyz0123456789";
            }

            var cLen = cChar.Length;
            var sRet = "";
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            for (var i = 0; i < n; i++)
            {
                sRet += cChar[rnd.Next(0, cLen)].ToString();
            }
            return sRet;
        }

        /// <summary>取得区间中的随机数,例如:getRndNext(14,17),将返回14,15,16</summary>
        /// <param name="min">随机数的最小值</param> 
        /// <param name="max">随机数的最大值(结果小于该值)</param> 
        /// <returns></returns>
        public static int GetRndNext(int min, int max)
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            var t = 0;
            if (min > max)
            {
                t = min;
                min = max;
                max = t;
            }

            t = max - min;
            return rnd.Next(t) + min;
        }

        /// <summary>取得区间中的随机数,例如:getRndNext(14,17),将返回14,15,16</summary>
        /// <param name="min">随机数的最小值</param> 
        /// <param name="max">随机数的最大值(结果小于该值)</param> 
        /// <returns></returns>
        public static decimal GetRndNextDecimal(decimal min, decimal max)
        {
            var rnd = new Random(Guid.NewGuid().GetHashCode());
            decimal t = 0;
            if (min > max)
            {
                t = min;
                min = max;
                max = t;
            }

            t = max - min;
            return (decimal)rnd.NextDouble() * t + min;
        }
        #endregion

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        private async static Task<string> Generate(int maxLength, string text)
        {
            var length = GetRandomInt(1,maxLength);
            var result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                await Task.Run(() =>
                {
                    var index = GetRandomInt(1, text.Length);
                    result.Append(text[index].ToString());

                });

            }
            return result.ToString();
        }

    
    }
}
