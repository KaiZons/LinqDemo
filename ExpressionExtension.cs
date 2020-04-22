using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LinqDemo
{
    /// <summary>
    /// 表达式树运算 扩展方法
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// 并 expr1 AND expr2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Visit(expr1.Body);
            var right = visitor.Visit(expr2.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);

        }

        /// <summary>
        /// 或 expr1 or expr2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Visit(expr1.Body);
            var right = visitor.Visit(expr2.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        /// <summary>
        /// 非 !expr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
            var temp = visitor.Visit(expr.Body);
            var body = Expression.Not(temp);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        private class NewExpressionVisitor : ExpressionVisitor
        {
            private ParameterExpression m_parameter { get; set; }

            public NewExpressionVisitor(ParameterExpression param)
            {
                m_parameter = param;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return m_parameter;
            }
        }
    }
}
