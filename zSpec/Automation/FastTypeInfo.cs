using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using zSpec.Extensions;

// ReSharper disable StaticMemberInGenericType

namespace zSpec.Automation
{
    /// <summary>
    /// Provides information about generic type
    /// </summary>
    /// <typeparam name="TSubject">Type</typeparam>
    public static class FastTypeInfo<TSubject>
    {
        private static readonly FastTypeInfo TypeInfo;

        static FastTypeInfo()
        {
            var type = typeof(TSubject);

            TypeInfo = FastTypeInfo.GetInstance(type);
        }

        public static HashSet<PropertyInfo> PublicProperties => TypeInfo.PublicProperties;

        public static Dictionary<string, PropertyInfo> PublicPropertiesMap => TypeInfo.PublicPropertiesMap;

        public static HashSet<MethodInfo> PublicMethods => TypeInfo.PublicMethods;

        public static HashSet<Attribute> Attributes => TypeInfo.Attributes;

        public static TSubject Create(params object[] args) => TypeInfo.Create<TSubject>(args);

        public static TAttribute GetCustomAttribute<TAttribute>() where TAttribute : Attribute =>
            (TAttribute)Attributes.FirstOrDefault(x => x.GetType() == typeof(TAttribute));

        public static bool HasAttribute<TAttr>()
            where TAttr : Attribute =>
            Attributes.Any(x => x.GetType() == typeof(TAttr));

        public static Func<TObject, TProperty> PropertyGetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject), "Value");

            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            var result = Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression, paramExpression)
                .Compile();

            return result;
        }

        public static Action<TObject, TProperty> PropertySetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject));
            var paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);
            var result = Expression.Lambda<Action<TObject, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();

            return result;
        }
    }

    internal delegate T ObjectActivator<out T>(params object[] args);

    internal delegate object ObjectActivator(params object[] args);

    /// <summary>
    /// Provides information about type
    /// </summary>
    public class FastTypeInfo
    {
        private static readonly ConcurrentDictionary<Type, FastTypeInfo> Cache
            = new();

        private readonly ConcurrentDictionary<string, ObjectActivator> activators;
        private readonly ConstructorInfo[] constructors;

        private readonly Type type;

        public FastTypeInfo(Type type)
        {
            this.type = type;

            this.Attributes = type.GetCustomAttributes().ToHashSet();

            this.PublicProperties = type
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToHashSet();

            this.PublicPropertiesMap = this.PublicProperties.ToDictionary(p => p.Name);

            this.PublicMethods = type.GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToHashSet();

            this.constructors = type.GetConstructors();
            this.activators = new ConcurrentDictionary<string, ObjectActivator>();
        }

        public HashSet<PropertyInfo> PublicProperties { get; }

        public Dictionary<string, PropertyInfo> PublicPropertiesMap { get; }

        public HashSet<MethodInfo> PublicMethods { get; }

        public HashSet<Attribute> Attributes { get; }

        public TSubject Create<TSubject>(params object[] args)
        {
            if (typeof(TSubject) != this.type)
            {
                throw new InvalidOperationException($"It's wrong TSubject parameter expected: [{this.type}]");
            }

            return (TSubject)this.activators.GetOrAdd(
                    GetSignature(args),
                    GetActivator(this.GetConstructorInfo(args)))
                .Invoke(args);
        }

        public TAttr GetCustomAttribute<TAttr>()
            where TAttr : Attribute =>
            (TAttr)this.Attributes.FirstOrDefault(x => x.GetType() == typeof(TAttr));

        public static FastTypeInfo GetInstance(Type type) => Cache.GetOrAdd(type, new FastTypeInfo(type));

        public bool HasAttribute<TAttr>()
            where TAttr : Attribute =>
            this.Attributes.Any(x => x.GetType() == typeof(TAttr));

        public Func<TObject, TProperty> PropertyGetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject), "Value");

            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            var result = Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression,
                paramExpression).Compile();

            return result;
        }

        public Action<TObject, TProperty> PropertySetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject));
            var paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);
            var result = Expression.Lambda<Action<TObject, TProperty>>(Expression
                .Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2).Compile();

            return result;
        }

        #region Create private

        private static string GetSignature(object[] args) => args.Select(x => x.GetType().ToString()).Join(",");

        private ConstructorInfo GetConstructorInfo(object[] args)
        {
            for (var i = 0; i < this.constructors.Length; i++)
            {
                var consturctor = this.constructors[i];
                var ctrParams = consturctor.GetParameters();
                if (ctrParams.Length != args.Length)
                {
                    continue;
                }

                var isWrongParametrType = true;
                for (var j = 0; j < args.Length; i++)
                {
                    if (ctrParams[j].ParameterType != args[j].GetType())
                    {
                        isWrongParametrType = false;
                        break;
                    }
                }

                if (!isWrongParametrType)
                {
                    continue;
                }

                return consturctor;
            }

            var signature = GetSignature(args);

            throw new InvalidOperationException(
                $"Constructor ({signature}) is not found for {this.type}");
        }

        private static ObjectActivator GetActivator(ConstructorInfo ctor)
        {
            var paramsInfo = ctor.GetParameters();

            // create a single param of type object[]
            var param = Expression.Parameter(typeof(object[]), "args");

            var argsExp = new Expression[paramsInfo.Length];

            // pick each arg from the params array 
            // and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            // make a NewExpression that calls the
            // ctor with the args we just created
            var newExp = Expression.New(ctor, argsExp);

            // create a lambda with the New
            // Expression as body and our param object[] as arg
            var lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            // compile it
            var compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }

        #endregion
    }
}
