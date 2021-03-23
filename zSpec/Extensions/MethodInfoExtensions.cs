using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace zSpec.Extensions
{
    public static class MethodInfoExtensions
    {
        public static Delegate CreateMethod(this MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (!method.IsStatic)
            {
                throw new ArgumentException("The provided method must be static.", nameof(method));
            }

            if (method.IsGenericMethod)
            {
                throw new ArgumentException("The provided method must not be generic.", nameof(method));
            }

            var parameters = method.GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                .ToArray();

            // ReSharper disable once CoVariantArrayConversion
            var call = Expression.Call(null, method, parameters);
            return Expression.Lambda(call, parameters).Compile();
        }
    }
}
