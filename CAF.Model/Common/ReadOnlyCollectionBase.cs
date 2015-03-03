
namespace CAF
{

    using CAF.Data;
    using CAF.Model;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public static class ReadOnlyCollectionBase<K> where K : ReadOnlyBase
    {
        static readonly string QUERY =
           "  SELECT * FROM {0} WHERE {1} ORDER BY {2} OFFSET {3} ROWS FETCH NEXT {4} ROWS only ";
        //sqlserver 2012及以下版本使用
        //         readonly string QUERY =
        //            "SELECT * FROM(  SELECT ROW_NUMBER() OVER ( ORDER BY {2} ) AS rownum ,* FROM {0} WHERE {1} )t WHERE rownum BETWEEN {3} AND ({3}+{4})";

        static readonly string COUNT = "  SELECT COUNT(*) FROM {0} WHERE {1}";

        static readonly string COMPUTE = "  SELECT {2} FROM {0} WHERE {1}";

        static string _tableName;

        static string _queryWhere;

        static string _orderBy;

        static object _dynamicObj;

        static string _sum;
        static string _average;



        public static ReadOnlyCollectionQueryResult<K> Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderBy">排序字段，可以是多字段用","隔开</param>
        /// <param name="pageSize">分页条数</param>
        /// <param name="readonlyList">查询对象，如：new{Id=Guid.NewId(),Name="xxx"}</param>
        /// <param name="queryWhere">查询条件，如：" Id=@Id And Name=@Name"</param>
        /// <param name="pageIndex">当前页，从第0页开始</param>
        /// <param name="sum">求和字段，可以是多字段用","隔开</param>
        /// <param name="average">求平均字段，可以是多字段用","隔开</param>
        /// <returns></returns>
        public static ReadOnlyCollectionQueryResult<K> Query(string orderBy, int pageSize, K readonlyList, string queryWhere = " 1=1", int pageIndex = 1, string sum = "", string average = "")
        {
            _queryWhere = queryWhere;
            _orderBy = orderBy;
            _tableName = readonlyList.TableName;

            _dynamicObj = readonlyList;
            _average = average;
            _sum = sum;

            Result = new ReadOnlyCollectionQueryResult<K> { PageSize = pageSize, PageIndex = pageIndex };
            ExtcuteQuery();
            return Result;
        }

        private static void ExtcuteQuery()
        {
            using (IDbConnection conn = SqlService.Instance.Connection)
            {
                Result.Result =
                    conn.Query<K>(
                        string.Format(QUERY, _tableName, _queryWhere, _orderBy, Result.PageSize * (Result.PageIndex - 1), Result.PageSize),
                        _dynamicObj).ToList();
                Result.TotalCount = conn.Query<int>(string.Format(COUNT, _tableName, _queryWhere), _dynamicObj).Single();
                Result.Sum = Compute(conn, _sum, "SUM");
                Result.Average = Compute(conn, _average, "AVG");
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="fileds"></param>
        /// <param name="methodType"></param>
        /// <returns></returns>
        private static Dictionary<string, object> Compute(IDbConnection conn, string fileds, string methodType)
        {
            var method = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(fileds))
            {
                var paras = fileds.Trim().Split(',');
                for (var i = 0; i < paras.Length; i++)
                {
                    paras[i] = string.Format(" {1} ({0}) AS {0},", paras[i], methodType);
                }
                var result = conn.Query<FastExpando>(string.Format(COMPUTE, _tableName, _queryWhere, string.Join("", paras).Trim(',')), _dynamicObj).Single();
                foreach (var item in result)
                {
                    method.Add(item.Key, item.Value);
                }
            }
            return method;
        }
    }
}
