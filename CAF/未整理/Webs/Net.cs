namespace CAF.Webs
{
    public class Net
    {
        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            //获得IP地址
            string hostname;
            System.Net.IPHostEntry localhost;
            hostname = System.Net.Dns.GetHostName();
            localhost = System.Net.Dns.GetHostEntry(hostname);
            var ip = localhost.AddressList[0].ToString();
            var i = 1;
            while (ip.Contains(":"))
            {
                if (i == localhost.AddressList.Length)
                {
                    ip = "";
                    break;
                }
                ip = localhost.AddressList[i].ToString();
                if (ip.Contains(":"))
                {
                    i++;
                }
                else
                {
                    break;
                }
            }
            return ip;
        }
    }
}