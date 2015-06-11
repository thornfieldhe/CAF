﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Queue = CAF.FS.Core.Data.Queue;

namespace CAF.FS.Core.Infrastructure
{
    /// <summary>
    /// 提供ExpressionNew表达式树的解析
    /// </summary>
    public abstract class DbExpressionNewProvider
    {
        /// <summary>
        /// 条件堆栈
        /// </summary>
        public readonly Stack<string> SqlList = new Stack<string>();
        /// <summary>
        /// 队列管理模块
        /// </summary>
        protected readonly BaseQueueManger QueueManger;
        /// <summary>
        /// 包含数据库SQL操作的队列
        /// </summary>
        protected readonly Queue Queue;
        /// <summary>
        /// 是否是字段筛选
        /// </summary>
        protected bool IsSelect;

        /// <summary>
        /// 提供ExpressionNew表达式树的解析
        /// </summary>
        /// <param name="queueManger">队列管理模块</param>
        /// <param name="queue">包含数据库SQL操作的队列</param>
        public DbExpressionNewProvider(BaseQueueManger queueManger, Queue queue)
        {
            this.QueueManger = queueManger;
            this.Queue = queue;
        }

        /// <summary>
        /// 清除当前所有数据
        /// </summary>
        public void Clear()
        {
            this.SqlList.Clear();
        }
        public virtual Expression Visit(Expression exp, bool? isSelect = null)
        {
            if (exp == null) { return null; }
            if (isSelect != null) { this.IsSelect = isSelect.GetValueOrDefault(); }

            switch (exp.NodeType)
            {
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
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked: return this.CreateBinary((BinaryExpression)exp);
                case ExpressionType.Lambda: return this.VisitLambda((LambdaExpression)exp);
                case ExpressionType.New: return this.VisitNew((NewExpression)exp);
                case ExpressionType.MemberAccess: return this.CreateFieldName((MemberExpression)exp);
                case ExpressionType.Convert: return this.Visit(((UnaryExpression)exp).Operand);
                case ExpressionType.MemberInit: return this.VisitMemberInit((MemberInitExpression)exp);
                case ExpressionType.Call: return this.VisitMethodCall((MethodCallExpression)exp);
            }
            throw new Exception(string.Format("类型：(ExpressionType){0}，不存在。", exp.NodeType));
        }

        protected virtual Expression CreateFieldName(MemberExpression m)
        {
            if (m == null) return null;

            var keyValue = this.Queue.FieldMap.GetState(m.Member.Name);
            if (keyValue.Key == null) { return this.CreateFieldName((MemberExpression)m.Expression); }

            // 加入Sql队列
            string filedName;
            if (!DbProvider.IsField(keyValue.Value.FieldAtt.Name))
            {
                filedName = this.IsSelect ? keyValue.Value.FieldAtt.Name + " as " + keyValue.Key.Name : keyValue.Value.FieldAtt.Name;
            }
            else { filedName = this.QueueManger.DbProvider.KeywordAegis(keyValue.Value.FieldAtt.Name); }
            this.SqlList.Push(filedName);
            return m;
        }

        protected virtual ReadOnlyCollection<Expression> VisitExpressionList(ReadOnlyCollection<Expression> original)
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

        protected virtual NewExpression VisitNew(NewExpression nex)
        {
            this.VisitExpressionList(nex.Arguments);
            return nex;
        }

        protected virtual Expression VisitLambda(LambdaExpression lambda)
        {
            return this.Visit(lambda.Body);
        }

        protected virtual Expression VisitMemberInit(MemberInitExpression init)
        {
            var newExpression = this.VisitNew(init.NewExpression);
            var bindings = this.VisitBindingList(init.Bindings);
            if ((newExpression == init.NewExpression) && (bindings == init.Bindings))
            {
                return init;
            }
            return Expression.MemberInit(newExpression, bindings);
        }
        protected virtual IEnumerable<MemberBinding> VisitBindingList(ReadOnlyCollection<MemberBinding> original)
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

        protected virtual MemberBinding VisitBinding(MemberBinding binding)
        {
            switch (binding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return this.VisitMemberAssignment((MemberAssignment)binding);

                case MemberBindingType.MemberBinding:
                    return this.VisitMemberMemberBinding((MemberMemberBinding)binding);

                case MemberBindingType.ListBinding:
                    return this.VisitMemberListBinding((MemberListBinding)binding);
            }
            throw new Exception(string.Format("类型：(MemberBindingType){0}，不存在。", binding.BindingType));
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

        protected virtual ElementInit VisitElementInitializer(ElementInit initializer)
        {
            var arguments = this.VisitExpressionList(initializer.Arguments);
            return arguments != initializer.Arguments ? Expression.ElementInit(initializer.AddMethod, arguments) : initializer;
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
                case ExpressionType.ArrayIndex:
                case ExpressionType.ListInit:
                case ExpressionType.Constant: { return true; }
            }
            return false;
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

            this.CreateOperate(bexp.NodeType, left, right);

            return bexp;
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
                case ExpressionType.Equal: oper = "="; break;
                case ExpressionType.NotEqual: oper = "<>"; break;
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
            this.SqlList.Push(String.Format("({0} {1} {2})", left, oper, right));
        }
    }
}