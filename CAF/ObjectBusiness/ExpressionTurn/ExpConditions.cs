using System;
/* 作者：道法自然  
 * 个人邮件：myyangbin@sina.cn
 * 2014-10-1
 */
using System.Linq;
using System.Linq.Expressions;

namespace CAF
{
    using CAF.CAFExpression;

    internal enum ExpUnion : byte
    {
        And,
        Or,
        OrderBy
    }

    /// <summary>
    /// Lambda转SQL语句
    /// </summary>
    /// <typeparam name="T">查询类</typeparam>
    public sealed class ExpConditions<T>
    {

        #region 外部访问方法
        /// <summary>
        /// 获取 Where 条件语句
        /// </summary>
        /// <param name="AddCinditionKey">是否加Where词</param>
        /// <returns>Where条件语句</returns>
        public string Where(bool AddCinditionKey = true)
        {
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("exec", "");
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("delete", "");
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("master", "");
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("truncate", "");
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("declare", "");
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("create", "");
            this._aiWhereStr = this._aiWhereStr.ToLower().Replace("xp_", "no");
            if (string.IsNullOrWhiteSpace(this._aiWhereStr)) return string.Empty;

            if (AddCinditionKey)
            {
                return "AND " + this._aiWhereStr;
            }
            else
            {
                return this._aiWhereStr;
            }
        }

        /// <summary>
        /// 获取 OrderBy 条件语句
        /// </summary>
        /// <param name="AddCinditionKey">是否加Order By词</param>
        /// <returns>OrderBy 条件语句</returns>
        public string OrderBy(bool AddCinditionKey = true)
        {
            if (string.IsNullOrWhiteSpace(this._aiOrderByStr)) return string.Empty;

            if (AddCinditionKey)
            {
                return " Order By " + this._aiOrderByStr;
            }
            else
            {
                return this._aiOrderByStr;
            }

        }

        #endregion

        #region 混合语句增加操作
        /// <summary>
        /// 添加一个条件语句（Where/OrderBy）
        /// </summary>
        /// <param name="aiExp">条件表达示</param>
        public void Add(Expression<Func<IQueryable<T>, IQueryable<T>>> aiExp)
        {
            this.SetConditionStr(aiExp, ExpUnion.And);
        }

        /// <summary>
        /// 当给定条件满足时,添加一个条件语句（Where/OrderBy）
        /// </summary>
        /// <param name="aiCondition">当给定条件满足时</param>
        /// <param name="aiExp">条件表达示</param>
        public void Add(bool aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExp)
        {
            if (aiCondition)
            {
                this.SetConditionStr(aiExp, ExpUnion.And);
            }
        }

        /// <summary>
        /// 当给定lambda表达式条件满足时,添加一个条件语句（Where/OrderBy）
        /// </summary>
        /// <param name="aiCondition">给定lambda表达式条件</param>
        /// <param name="aiExp">条件表达示</param>
        public void Add(Func<bool> aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExp)
        {
            this.Add(aiCondition(), aiExp);
        }

        /// <summary>
        /// 如果条件满足时,将添加前一个条件语句（Where/OrderBy），否则添加后一个
        /// </summary>
        /// <param name="aiCondition">条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void Add(bool aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenTrue, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenFalse)
        {
            if (aiCondition)
            {
                this.SetConditionStr(aiExpWhenTrue, ExpUnion.And);
            }
            else
            {
                this.SetConditionStr(aiExpWhenFalse, ExpUnion.And);
            }
        }

        /// <summary>
        /// 如果条件满足时,将添加前一个条件语句（Where/OrderBy），否则添加后一个
        /// </summary>
        /// <param name="aiCondition">条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void Add(Func<bool> aiCondition, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenTrue, Expression<Func<IQueryable<T>, IQueryable<T>>> aiExpWhenFalse)
        {
            this.Add(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
        }

        #endregion

        #region 以 And 相联接 Where条件语句
        /// <summary>
        /// 添加一个Where条件语句，如果语句存在，则以 And 相联接
        /// </summary>
        /// <param name="aiExp">Where条件表达示</param>
        public void AndWhere(Expression<Func<T, bool>> aiExp)
        {
            this.SetOneConditionStr(aiExp, ExpUnion.And);
        }

        /// <summary>
        /// 当给定条件满足时,添加一个Where条件语句，如果语句存在，则以 And 相联接
        /// </summary>
        /// <param name="aiCondition">给定条件</param>
        /// <param name="aiExp">Where条件表达示</param>
        public void AndWhere(bool aiCondition, Expression<Func<T, bool>> aiExp)
        {
            if (aiCondition)
            {
                this.SetOneConditionStr(aiExp, ExpUnion.And);
            }

        }

        /// <summary>
        /// 当给定lambda表达式条件满足时,添加一个Where条件语句，如果语句存在，则以 And 相联接
        /// </summary>
        /// <param name="aiCondition">给定lambda表达式条件</param>
        /// <param name="aiExp"></param>
        public void AddAndWhere(Func<bool> aiCondition, Expression<Func<T, bool>> aiExp)
        {
            this.AndWhere(aiCondition(), aiExp);
        }

        /// <summary>
        /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 And 相联接
        /// </summary>
        /// <param name="aiCondition">条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void AndWhere(bool aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
        { this.SetOneConditionStr(aiCondition ? aiExpWhenTrue : aiExpWhenFalse, ExpUnion.And); }

        /// <summary>
        /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 And 相联接
        /// </summary>
        /// <param name="aiCondition">Lambda条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void AndWhere(Func<bool> aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
        {
            this.AndWhere(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
        }

        #endregion

        #region 以 Or 相联接 Where条件语句
        /// <summary>
        /// 添加一个Where条件语句，如果语句存在，则以 Or 相联接
        /// </summary>
        /// <param name="aiExp">Where条件表达示</param>
        public void OrWhere(Expression<Func<T, bool>> aiExp)
        {
            this.SetOneConditionStr(aiExp, ExpUnion.Or);
        }

        /// <summary>
        /// 当给定条件满足时,添加一个Where条件语句，如果语句存在，则以 Or 相联接
        /// </summary>
        /// <param name="aiCondition">给定条件</param>
        /// <param name="aiExp">Where条件表达示</param>
        public void OrWhere(bool aiCondition, Expression<Func<T, bool>> aiExp)
        {
            if (aiCondition)
            {
                this.SetOneConditionStr(aiExp, ExpUnion.Or);
            }

        }

        /// <summary>
        /// 当给定lambda表达式条件满足时,添加一个Where条件语句，如果语句存在，则以 Or 相联接
        /// </summary>
        /// <param name="aiCondition">给定lambda表达式条件</param>
        /// <param name="aiExp"></param>
        public void OrWhere(Func<bool> aiCondition, Expression<Func<T, bool>> aiExp)
        {
            this.OrWhere(aiCondition(), aiExp);
        }

        /// <summary>
        /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 Or 相联接
        /// </summary>
        /// <param name="aiCondition">条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void OrWhere(bool aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
        { this.SetOneConditionStr(aiCondition ? aiExpWhenTrue : aiExpWhenFalse, ExpUnion.Or); }

        /// <summary>
        /// 如果条件满足时,将添加前一个条件语句（Where），否则添加后一个,以 Or 相联接
        /// </summary>
        /// <param name="aiCondition">Lambda条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void OrWhere(Func<bool> aiCondition, Expression<Func<T, bool>> aiExpWhenTrue, Expression<Func<T, bool>> aiExpWhenFalse)
        {
            this.OrWhere(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
        }

        #endregion

        #region  OrderBy语句

        /// <summary>
        /// 添加一个OrderBy语句
        /// </summary>
        /// <typeparam name="D">OrderBy的字段数据类型</typeparam>
        /// <param name="aiExp">OrderBy条件表达示</param>
        public void AddOrderBy<D>(Expression<Func<T, D>> aiExp)
        {
            this.SetOneConditionStr(aiExp, ExpUnion.OrderBy);
        }

        /// <summary>
        /// 如果条件满足时,添加一个OrderBy语句
        /// </summary>
        /// <typeparam name="D">OrderBy的字段数据类型</typeparam>
        /// <param name="aiCondition">条件</param>
        /// <param name="aiExp">OrderBy条件表达示</param>
        public void AddOrderBy<D>(bool aiCondition, Expression<Func<T, D>> aiExp)
        {
            if (aiCondition)
            {
                this.SetOneConditionStr(aiExp, ExpUnion.OrderBy);
            }

        }

        /// <summary>
        /// 如果条件满足时,添加一个OrderBy语句
        /// </summary>
        /// <typeparam name="D">OrderBy的数据字段类型</typeparam>
        /// <param name="aiCondition">Lambda条件</param>
        /// <param name="aiExp">OrderBy条件表达示</param>
        public void ThenBy<D>(Func<bool> aiCondition, Expression<Func<T, D>> aiExp)
        {
            this.AddOrderBy<D>(aiCondition(), aiExp);
        }

        /// <summary>
        /// 如果条件满足时,将添加前一个OrderBy语句，否则添加后一个
        /// </summary>
        /// <typeparam name="D">OrderBy的数据字段类型</typeparam>
        /// <param name="aiCondition">条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void ThenBy<D>(bool aiCondition, Expression<Func<T, D>> aiExpWhenTrue, Expression<Func<T, D>> aiExpWhenFalse)
        { this.SetOneConditionStr(aiCondition ? aiExpWhenTrue : aiExpWhenFalse, ExpUnion.OrderBy); }

        /// <summary>
        /// 如果条件满足时,将添加前一个OrderBy语句，否则添加后一个
        /// </summary>
        /// <typeparam name="D">OrderBy的数据字段类型</typeparam>
        /// <param name="aiCondition">Lambda条件</param>
        /// <param name="aiExpWhenTrue">条件为真时</param>
        /// <param name="aiExpWhenFalse">条件为假时</param>
        public void ThenBy<D>(Func<bool> aiCondition, Expression<Func<T, D>> aiExpWhenTrue, Expression<Func<T, D>> aiExpWhenFalse)
        {
            this.ThenBy<D>(aiCondition(), aiExpWhenTrue, aiExpWhenFalse);
        }


        #endregion

        #region 内部操作

        private string _aiWhereStr = string.Empty;

        private string _aiOrderByStr = string.Empty;

        private void SetConditionStr(Expression aiExp, ExpUnion aiUion = ExpUnion.And)
        {
            this.SetWhere(aiExp);//Where条件句

            this.SetOrderBy(aiExp);//Order by 语句
        }

        private void SetOneConditionStr(Expression aiExp, ExpUnion bizUion = ExpUnion.And)
        {
            switch (bizUion)
            {
                case ExpUnion.And:
                case ExpUnion.Or:
                    this.SetWhere(aiExp);//Where条件句
                    break;
                case ExpUnion.OrderBy:
                    this.SetOrderBy(aiExp);//Order by 语句
                    break;
            }
        }

        private void SetOrderBy(Expression aiExp)
        {
            var itemstr = ExpressionWriterSql.BizWhereWriteToString(aiExp, ExpSqlType.aiOrder);
            if (string.IsNullOrWhiteSpace(this._aiOrderByStr))
            {
                this._aiOrderByStr = itemstr;
            }
            else
            {
                this._aiOrderByStr = this._aiOrderByStr + "," + itemstr;

            }
        }

        private void SetWhere(Expression aiExp, ExpUnion bizUion = ExpUnion.And)
        {
            var itemstr = ExpressionWriterSql.BizWhereWriteToString(aiExp, ExpSqlType.aiWhere);
            if (string.IsNullOrWhiteSpace(this._aiWhereStr))
            {
                this._aiWhereStr = itemstr;
            }
            else
            {
                if (bizUion == ExpUnion.Or)
                {
                    this._aiWhereStr = this._aiWhereStr + " Or " + itemstr;
                }
                else
                {
                    this._aiWhereStr = this._aiWhereStr + " And " + itemstr;
                }
            }
        }
        #endregion

    }
}
