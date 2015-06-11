using CAF.FS.Core.Data;
using CAF.FS.Core.Infrastructure;
using System;
using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CAF.FS.Core.Client.Common
{
    public class ExpressionBool : DbExpressionBoolProvider
    {
        public ExpressionBool(BaseQueueManger queueManger, Queue queue) : base(queueManger, queue) { }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            base.VisitMethodCall(m);
            if (this.ClearCallSql()) { return m; }
            var methodName = m.Method.Name;
            if (this.IsIgnoreMethod(methodName)) { return m; }

            #region 字段、参数、值类型
            Type fieldType = null;
            Type paramType = null;
            string fieldName = null;
            string paramName = null;

            if (m.Arguments.Count > 0)
            {
                // 静态方法 Object = null
                if (m.Object == null)
                {
                    if (!m.Arguments[0].Type.IsGenericType || m.Arguments[0].Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        fieldType = m.Arguments[0].Type; fieldName = this.SqlList.Pop();
                        if (m.Arguments.Count > 1) { paramType = m.Arguments[1].Type; paramName = this.SqlList.Pop(); }
                    }
                    else { paramType = m.Arguments[0].Type; paramName = this.SqlList.Pop(); fieldType = m.Arguments[1].Type; fieldName = this.SqlList.Pop(); }
                }
                else
                {
                    // 非List类型
                    if (!m.Object.Type.IsGenericType || m.Object.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        fieldType = m.Object.Type;
                        if (m.Arguments.Count > 0)
                        {
                            paramType = m.Arguments[0].Type;
                            paramName = this.SqlList.Pop();
                        }
                        fieldName = this.SqlList.Pop();
                    }
                    else { paramType = m.Object.Type; fieldType = m.Arguments[0].Type; paramName = this.SqlList.Pop(); fieldName = this.SqlList.Pop(); }
                }
            }
            #endregion

            switch (methodName)
            {
                case "Contains":
                    this.VisitMethodContains(fieldType, fieldName, paramType, paramName); break;
                case "StartsWith":
                    this.VisitMethodStartswith(fieldType, fieldName, paramType, paramName); break;
                case "EndsWith":
                    this.VisitMethodEndswith(fieldType, fieldName, paramType, paramName); break;
                case "IsEquals":
                    this.VisitMethodIsEquals(fieldType, fieldName, paramType, paramName); break;
                case "Equals":
                    this.VisitMethodEquals(fieldType, fieldName, paramType, paramName); break;
                case "ToShortDate":
                    this.VisitMethodToShortDate(fieldType, fieldName); break;
                default:
                    {
                        if (m.Arguments.Count == 0 && m.Object != null) { return m; }
                        throw new Exception(string.Format("暂不支持该方法的SQL转换：" + m.Method.Name.ToUpper()));
                    }
            }
            this.IsNot = false;
            return m;
        }

        /// <summary>
        /// 忽略字段的方法
        /// </summary>
        /// <param name="methodName">方法名称（大写）</param>
        protected virtual bool IsIgnoreMethod(string methodName)
        {
            switch (methodName)
            {
                case "ToDateTime": return true;
                default: return false;
            }
        }

        /// <summary>
        /// Contains方法解析
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramName"></param>
        protected virtual void VisitMethodContains(Type fieldType, string fieldName, Type paramType, string paramName)
        {

            // 非List<>形式
            if (paramType != null && (!paramType.IsGenericType || paramType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                #region 搜索值串的处理
                var param = this.Queue.Param.Find(o => o.ParameterName == paramName);
                if (param != null && Regex.IsMatch(param.Value.ToString(), @"[\d]+") && (Type.GetTypeCode(fieldType) == TypeCode.Int16 || Type.GetTypeCode(fieldType) == TypeCode.Int32 || Type.GetTypeCode(fieldType) == TypeCode.Decimal || Type.GetTypeCode(fieldType) == TypeCode.Double || Type.GetTypeCode(fieldType) == TypeCode.Int64 || Type.GetTypeCode(fieldType) == TypeCode.UInt16 || Type.GetTypeCode(fieldType) == TypeCode.UInt32 || Type.GetTypeCode(fieldType) == TypeCode.UInt64))
                {
                    param.Value = "," + param.Value + ",";
                    param.DbType = DbType.String;
                    if (this.QueueManger.DbProvider.KeywordAegis("").Length > 0) { fieldName = "','+" + fieldName.Substring(1, fieldName.Length - 2) + "+','"; }
                    else { fieldName = "','+" + fieldName + "+','"; }
                }
                #endregion

                this.SqlList.Push(String.Format("CHARINDEX({0},{1}) {2} 0", paramName, fieldName, this.IsNot ? "<=" : ">"));
            }
            else
            {

                // 不使用参数化形式，同时移除参数
                var paramValue = this.CurrentDbParameter.Value;
                this.Queue.Param.RemoveAt(this.Queue.Param.Count - 1);

                // 字段是字符类型的，需要加入''符号
                if (Type.GetTypeCode(fieldType) == TypeCode.String) { paramValue = "'" + paramValue.ToString().Replace(",", "','") + "'"; }

                this.SqlList.Push(String.Format("{0} {1} IN ({2})", fieldName, this.IsNot ? "Not" : "", paramValue));
            }
        }

        /// <summary>
        /// StartSwith方法解析
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramName"></param>
        protected virtual void VisitMethodStartswith(Type fieldType, string fieldName, Type paramType, string paramName)
        {
            this.SqlList.Push(String.Format("CHARINDEX({0},{1}) {2} 1", paramName, fieldName, this.IsNot ? ">" : "="));
        }

        /// <summary>
        /// EndSwith方法解析
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramName"></param>
        protected virtual void VisitMethodEndswith(Type fieldType, string fieldName, Type paramType, string paramName)
        {
            this.SqlList.Push(String.Format("{0} {1} LIKE {2}", fieldName, this.IsNot ? "Not" : "", paramName));
            this.CurrentDbParameter.Value = string.Format("%{0}", this.CurrentDbParameter.Value);
        }

        /// <summary>
        /// IsEquals方法解析
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramName"></param>
        protected virtual void VisitMethodIsEquals(Type fieldType, string fieldName, Type paramType, string paramName)
        {
            this.SqlList.Push(String.Format("{0} {1} {2}", fieldName, this.IsNot ? "<>" : "=", paramName));
        }

        /// <summary>
        /// IsEquals方法解析
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramName"></param>
        protected virtual void VisitMethodEquals(Type fieldType, string fieldName, Type paramType, string paramName)
        {
            this.SqlList.Push(String.Format("{0} {1} {2}", fieldName, this.IsNot ? "<>" : "=", paramName));
        }

        /// <summary>
        /// ToShortDate方法解析
        /// </summary>
        /// <param name="fieldType"></param>
        /// <param name="fieldName"></param>
        protected virtual void VisitMethodToShortDate(Type fieldType, string fieldName)
        {
            this.SqlList.Push(String.Format("CONVERT(varchar(100), {0}, 23)", fieldName));
        }
    }
}
