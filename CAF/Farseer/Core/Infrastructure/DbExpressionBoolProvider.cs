using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Queue = CAF.FS.Core.Data.Queue;

namespace CAF.FS.Core.Infrastructure
{
    /// <summary>
    /// 提供ExpressionBinary表达式树的解析
    /// </summary>
    public abstract class DbExpressionBoolProvider
    {
        /// <summary>
        ///  条件堆栈
        /// </summary>
        public readonly Stack<string> SqlList = new Stack<string>();
        /// <summary>
        ///  参数个数（标识）
        /// </summary>
        private int _paramsCount;
        /// <summary>
        /// 当前字段名称
        /// </summary>
        private string _currentFieldName;
        /// <summary>
        /// 当前值参数
        /// </summary>
        protected DbParameter CurrentDbParameter;
        /// <summary>
        /// 队列管理模块
        /// </summary>
        protected readonly BaseQueueManger QueueManger;
        /// <summary>
        /// 包含数据库SQL操作的队列
        /// </summary>
        protected readonly Queue Queue;

        /// <summary>
        /// 是否包括Not条件
        /// </summary>
        protected bool IsNot;

        /// <summary>
        /// 默认构造器
        /// </summary>
        /// <param name="queueManger">队列管理模块</param>
        /// <param name="queue">包含数据库SQL操作的队列</param>
        public DbExpressionBoolProvider(BaseQueueManger queueManger, Queue queue)
        {
            this.QueueManger = queueManger;
            this.Queue = queue;
            if (this.Queue.Param == null) { this.Queue.Param = new List<DbParameter>(); }
        }

        /// <summary>
        /// 清除当前所有数据
        /// </summary>
        public void Clear()
        {
            this.CurrentDbParameter = null;
            this._currentFieldName = null;
            this._paramsCount = 0;
            this.SqlList.Clear();
        }

        public Expression Visit(Expression exp)
        {
            if (exp == null) { return null; }
            switch (exp.NodeType)
            {
                case ExpressionType.ListInit:
                case ExpressionType.Call:
                case ExpressionType.Constant:
                case ExpressionType.Convert:
                case ExpressionType.MemberAccess: exp = this.VisitConvertExp(exp); break;
            }

            switch (exp.NodeType)
            {
                case ExpressionType.Lambda: return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.SubtractChecked: return this.CreateBinary((BinaryExpression)exp);
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.RightShift:
                case ExpressionType.Subtract: return this.CreateBinary((BinaryExpression)exp);
                case ExpressionType.ArrayLength:
                case ExpressionType.Convert: return this.Visit(((UnaryExpression)exp).Operand);
                case ExpressionType.Call: return this.VisitMethodCall((MethodCallExpression)exp);
                case ExpressionType.Constant: return this.CreateDbParam((ConstantExpression)exp);
                case ExpressionType.MemberAccess: return this.CreateFieldName((MemberExpression)exp);
                case ExpressionType.ConvertChecked:
                case ExpressionType.Negate:
                case ExpressionType.UnaryPlus:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs: return this.VisitUnary((UnaryExpression)exp);
                case ExpressionType.Invoke: return this.VisitInvocation((InvocationExpression)exp);
                case ExpressionType.Parameter: return this.VisitParameter((ParameterExpression)exp);
                case ExpressionType.New: return this.VisitNew((NewExpression)exp);
            }
            throw new Exception(string.Format("类型：(ExpressionType){0}，不存在。", exp.NodeType));
        }

        /// <summary>
        /// 操作符号
        /// </summary>
        /// <param name="nodeType">表达式树类型</param>
        /// <param name="left">操作符左边的SQL</param>
        /// <param name="right">操作符右边的SQL</param>
        protected virtual void CreateOperate(ExpressionType nodeType, string left, string right)
        {
            string oper;
            switch (nodeType)
            {
                case ExpressionType.Equal: oper = this.CurrentDbParameter != null && this.CurrentDbParameter.Value != null ? "=" : "IS"; break;
                case ExpressionType.NotEqual: oper = this.CurrentDbParameter != null && this.CurrentDbParameter.Value != null ? "<>" : "IS NOT"; break;
                case ExpressionType.GreaterThan: oper = ">"; break;
                case ExpressionType.GreaterThanOrEqual: oper = ">="; break;
                case ExpressionType.LessThan: oper = "<"; break;
                case ExpressionType.LessThanOrEqual: oper = "<="; break;
                case ExpressionType.AndAlso: oper = "AND"; break;
                case ExpressionType.OrElse: oper = "OR"; break;
                case ExpressionType.Add: oper = "+"; break;
                case ExpressionType.Subtract: oper = "-"; break;
                case ExpressionType.Multiply: oper = "*"; break;
                case ExpressionType.Divide: oper = "/"; break;
                case ExpressionType.And: oper = "&"; break;
                case ExpressionType.Or: oper = "|"; break;
                default: throw new NotSupportedException(nodeType + "的类型，未定义操作符号！");
            }

            if (this.CurrentDbParameter != null && this.CurrentDbParameter.Value == null) { right = "NULL"; }
            this.SqlList.Push(String.Format("({0} {1} {2})", left, oper, right));
        }

        /// <summary>
        ///     将二元符号转换成T-SQL可识别的操作符
        /// </summary>
        protected virtual Expression CreateBinary(BinaryExpression bexp)
        {
            if (bexp == null) { return null; }

            // 先解析字段
            if (bexp.Left.NodeType == ExpressionType.MemberAccess || (bexp.Left.NodeType != ExpressionType.MemberAccess && bexp.Right.NodeType != ExpressionType.MemberAccess))
            {
                this.Visit(bexp.Left);
                this.Visit(bexp.Right);
            }
            else
            {
                this.Visit(bexp.Right);
                this.Visit(bexp.Left);
            }

            var right = this.SqlList.Pop();
            var left = this.SqlList.Pop();

            if (bexp.NodeType == ExpressionType.AndAlso || bexp.NodeType == ExpressionType.OrElse) { right = this.SqlTrue(right); left = this.SqlTrue(left); }

            this.CreateOperate(bexp.NodeType, left, right);

            // 清除状态（与或状态，不清除）
            if (bexp.NodeType != ExpressionType.And && bexp.NodeType != ExpressionType.Or)
            {
                this._currentFieldName = null;
                this.CurrentDbParameter = null;
            }
            return bexp;
        }

        /// <summary>
        ///     将属性变量的右边值，转换成T-SQL的字段值
        /// </summary>
        protected virtual Expression CreateDbParam(ConstantExpression cexp)
        {
            if (cexp == null) return null;
            //if (string.IsNullOrWhiteSpace(_currentFieldName)) { throw new Exception("当前字段名称为空，无法生成字段参数"); }

            // 非字符串，不使用参数
            //if (!(cexp.Value is string))
            //{
            //    int len;
            //    var type = DbProvider.GetDbType(cexp.Value, out len);
            //    SqlList.Push(DbProvider.ParamConvertValue(cexp.Value, type).ToString());

            //}
            //else
            {
                this._paramsCount++;
                //  查找组中是否存在已有的参数，有则直接取出
                this.CurrentDbParameter = this.QueueManger.DbProvider.CreateDbParam(this.Queue.Index + "_" + this._paramsCount + "_" + this._currentFieldName, cexp.Value);
                this.Queue.Param.Add(this.CurrentDbParameter);
                this.SqlList.Push(this.CurrentDbParameter.ParameterName);
            }
            this._currentFieldName = null;
            return cexp;
        }

        /// <summary>
        ///     将属性变量转换成T-SQL字段名
        /// </summary>
        protected virtual Expression CreateFieldName(MemberExpression m)
        {
            if (m == null) return null;
            if (m.NodeType == ExpressionType.Constant) { return this.Visit(this.VisitConvertExp(m)); }

            var keyValue = this.Queue.FieldMap.GetState(m.Member.Name);
            // 解析带SQL函数的字段
            if (keyValue.Key == null) { return this.CreateFunctionFieldName(m); }

            // 加入Sql队列
            this._currentFieldName = keyValue.Value.FieldAtt.Name;
            var filedName = this.QueueManger.DbProvider.KeywordAegis(this._currentFieldName);
            this.SqlList.Push(filedName);
            return m;
        }

        /// <summary>
        ///     将属性变量转换成T-SQL字段名（带SQL函数的字段）
        /// </summary>
        protected virtual Expression CreateFunctionFieldName(MemberExpression m)
        {
            switch (m.Member.Name)
            {
                case "Count":
                case "Length":
                    {
                        var exp = this.CreateFieldName((MemberExpression)m.Expression);
                        this.SqlList.Push(string.Format("LEN({0})", this.SqlList.Pop()));
                        return exp;
                    }
            }
            return this.CreateFieldName((MemberExpression)m.Expression);
        }

        /// <summary>
        ///     值类型的转换
        /// </summary>
        protected virtual Expression VisitUnary(UnaryExpression u)
        {
            if (u.NodeType == ExpressionType.Not) { this.IsNot = true; }
            return this.Visit((u).Operand);
        }

        /// <summary>
        ///     将变量转换成值
        /// </summary>
        protected Expression VisitConvertExp(Expression exp)
        {
            if (exp is BinaryExpression || !this.IsFieldValue(exp)) { return exp; }
            return Expression.Constant(Expression.Lambda(exp).Compile().DynamicInvoke(null), exp.Type);
        }

        /// <summary>
        ///     判断是字段，还是值类型
        /// </summary>
        protected bool IsFieldValue(Expression exp)
        {
            if (exp == null) { return false; }

            switch (exp.NodeType)
            {
                case ExpressionType.Lambda: return this.IsFieldValue(((LambdaExpression)exp).Body);
                case ExpressionType.Call:
                    {
                        var callExp = (MethodCallExpression)exp;
                        // oXXXX.Call(....)
                        if (callExp.Object != null && !this.IsFieldValue(callExp.Object)) { return false; }
                        foreach (var item in callExp.Arguments) { if (!this.IsFieldValue(item)) { return false; } }
                        return true;
                    }
                case ExpressionType.MemberAccess:
                    {
                        var memExp = (MemberExpression)exp;
                        // o.XXXX
                        return memExp.Expression == null || this.IsFieldValue(memExp.Expression);
                        //if (memExp.Expression.NodeType == ExpressionType.Constant) { return true; }
                        //if (memExp.Expression.NodeType == ExpressionType.MemberAccess) { return IsCanCompile(memExp.Expression); }
                        //break;
                    }
                case ExpressionType.Parameter: return !exp.Type.IsClass && !exp.Type.IsAbstract && !exp.Type.IsInterface;
                case ExpressionType.Convert: return this.IsFieldValue(((UnaryExpression)exp).Operand);
                case ExpressionType.Add:
                case ExpressionType.Subtract:
                case ExpressionType.Multiply:
                case ExpressionType.Divide: return this.IsFieldValue(((BinaryExpression)exp).Left) && this.IsFieldValue(((BinaryExpression)exp).Right);
                case ExpressionType.ArrayIndex:
                case ExpressionType.ListInit:
                case ExpressionType.Constant: { return true; }
            }
            return false;
        }

        /// <summary>
        ///     解析方法
        /// </summary>
        protected virtual Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Object == null)
            {
                for (var i = m.Arguments.Count - 1; i > 0; i--)
                {
                    var exp = m.Arguments[i];
                    //while (exp != null && exp.NodeType == ExpressionType.Call)
                    //{
                    //    exp = ((MethodCallExpression)exp).Object;
                    //}
                    this.Visit(exp);
                }
                this.Visit(m.Arguments[0]);
            }
            else
            {
                // 如果m.Object能压缩，证明不是字段（必须先解析字段，再解析值）
                var result = this.IsFieldValue(m.Object);

                if (!result) { this.Visit(m.Object); }
                for (var i = 0; i < m.Arguments.Count; i++) { this.Visit(m.Arguments[i]); }
                if (result) { this.Visit(m.Object); }
            }
            return m;
        }

        protected MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment: return this.VisitMemberAssignment((MemberAssignment)binding);

                case MemberBindingType.MemberBinding: return this.VisitMemberMemberBinding((MemberMemberBinding)binding);

                case MemberBindingType.ListBinding: return this.VisitMemberListBinding((MemberListBinding)binding);
            }
            throw new Exception(string.Format("类型：(MemberBindingType){0}，不存在。", binding.BindingType));
        }

        protected IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
        {
            List<MemberBinding> list = null;
            var num = 0;
            var count = original.Count;
            while (num < count)
            {
                var item = this.VisitBinding(original[num]);
                if (list != null)
                {
                    list.Add(item);
                }
                else if (item != original[num])
                {
                    list = new List<MemberBinding>(count);
                    for (var i = 0; i < num; i++)
                    {
                        list.Add(original[i]);
                    }
                    list.Add(item);
                }
                num++;
            }
            if (list != null)
            {
                return list;
            }
            return original;
        }

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            var arguments = this.VisitExpressionList(initializer.Arguments);
            return arguments != initializer.Arguments ? Expression.ElementInit(initializer.AddMethod, arguments) : initializer;
        }

        protected virtual IEnumerable<ElementInit> VisitElementInitializerList(ReadOnlyCollection<ElementInit> original)
        {
            List<ElementInit> list = null;
            var num = 0;
            var count = original.Count;
            while (num < count)
            {
                var item = this.VisitElementInitializer(original[num]);
                if (list != null)
                {
                    list.Add(item);
                }
                else if (item != original[num])
                {
                    list = new List<ElementInit>(count);
                    for (var i = 0; i < num; i++)
                    {
                        list.Add(original[i]);
                    }
                    list.Add(item);
                }
                num++;
            }
            if (list != null)
            {
                return list;
            }
            return original;
        }

        protected virtual IEnumerable<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
        {
            List<Expression> sequence = null;
            var num = 0;
            var count = original.Count;
            while (num < count)
            {
                var item = this.Visit(original[num]);
                if (sequence != null)
                {
                    sequence.Add(item);
                }
                else if (item != original[num])
                {
                    sequence = new List<Expression>(count);
                    for (var i = 0; i < num; i++)
                    {
                        sequence.Add(original[i]);
                    }
                    sequence.Add(item);
                }
                num++;
            }
            if (sequence != null)
            {
                return (ReadOnlyCollection<Expression>)(IEnumerable)sequence;
            }
            return original;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            this.Visit(lambda.Body);
            return lambda;
        }

        protected virtual MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
        {
            var expression = this.Visit(assignment.Expression);
            return expression != assignment.Expression ? Expression.Bind(assignment.Member, expression) : assignment;
        }

        protected virtual MemberListBinding VisitMemberListBinding(MemberListBinding binding)
        {
            var initializers = this.VisitElementInitializerList(binding.Initializers);
            return initializers != binding.Initializers ? Expression.ListBind(binding.Member, initializers) : binding;
        }

        protected virtual MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
        {
            var bindings = this.VisitBindingList(binding.Bindings);
            return bindings != binding.Bindings ? Expression.MemberBind(binding.Member, bindings) : binding;
        }
        protected virtual Expression VisitInvocation(InvocationExpression iv)
        {
            var arguments = this.VisitExpressionList(iv.Arguments);
            var expression = this.Visit(iv.Expression);
            if ((arguments == iv.Arguments) && (expression == iv.Expression))
            {
                return iv;
            }
            return Expression.Invoke(expression, arguments);
        }
        protected virtual Expression VisitParameter(ParameterExpression p)
        {
            return p;
        }

        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            if (nex.Arguments.Count == 0 && nex.Type.IsGenericType)
            {
                this.CreateDbParam(Expression.Constant(null));
            }
            this.VisitExpressionList(nex.Arguments);
            return nex;
        }

        /// <summary>
        ///     清除值为空的条件，并给与1!=1的SQL
        /// </summary>
        protected virtual bool ClearCallSql()
        {
            if (this.Queue.Param != null && this.Queue.Param.Count > 0 && (this.Queue.Param.Last().Value == null || string.IsNullOrWhiteSpace(this.Queue.Param.Last().Value.ToString())))
            {
                this.CurrentDbParameter = null;
                this.Queue.Param.RemoveAt(this.Queue.Param.Count - 1);
                this.SqlList.Pop();
                this.SqlList.Pop();
                this.SqlList.Push("1<>1");
                return true;
            }
            return false;
        }

        /// <summary>
        ///     当存在true 时，特殊处理
        /// </summary>
        protected virtual string SqlTrue(string sql)
        {
            var dbParam = this.Queue.Param.FirstOrDefault(o => o.ParameterName == sql);
            if (dbParam != null)
            {
                var result = dbParam.Value.ToString().Equals("true");
                this.Queue.Param.RemoveAll(o => o.ParameterName == sql);
                return result ? "1=1" : "1<>1";
            }
            return sql;
        }
    }
}
