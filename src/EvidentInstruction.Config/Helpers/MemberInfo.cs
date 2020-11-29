using System;
using System.Linq.Expressions;

namespace EvidentInstruction.Config.Helpers
{
    public static class MemberInfo
    {
        public static string GetName<T>(Expression<Func<T>> memberExpression)
        {
            var expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }
    }
}