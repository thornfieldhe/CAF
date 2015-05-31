using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAF.Core
{
    /// <summary>
    /// 抽象字典存储访问对象接口
    /// </summary>
    public interface IDictionaryStore
    {
        /// <summary>
        /// 基于配置、数据库等信息的Store及访问方向提取信息
        /// 该方法重新加载相应的缓冲信息
        /// </summary>
        void Refersh();

        /// <summary>
        /// 根据Contex定义的Key/Value及访问方向提取信息
        /// </summary>
        /// <param name="contex"></param>
        void Find(DictionaryContext contex);
    }
}
