using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeExample.Services
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var parameterExpression = Expression.Parameter(typeof(TSource), "x");

            var propertyBindings = GetPropertyBindings<TSource, TDestination>(parameterExpression);
            var fieldBindings = GetFieldBindings<TSource, TDestination>(parameterExpression);

            var newExpression = Expression.New(typeof(TDestination));
            var body = Expression.MemberInit(newExpression, propertyBindings.Union(fieldBindings));
            var lambda = Expression.Lambda<Func<TSource, TDestination>>(body, parameterExpression);
            
            return new Mapper<TSource, TDestination>(lambda.Compile());
        }

        private IEnumerable<MemberBinding> GetPropertyBindings<TSource, TDestination>(ParameterExpression parameterExpression)
        {
            var sourcePropertyInfos = typeof(TSource)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            var destinationPropertyInfos = typeof(TDestination)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);

            foreach (var destinationPropertyInfo in destinationPropertyInfos)
            {
                var sourcePropertyInfo = sourcePropertyInfos
                    .FirstOrDefault(x => x.Name == destinationPropertyInfo.Name &&
                                         x.PropertyType == destinationPropertyInfo.PropertyType);

                if (sourcePropertyInfo == null)
                    continue;

                var sourcePropertyExpression = Expression.Property(parameterExpression, sourcePropertyInfo);

                yield return Expression.Bind(destinationPropertyInfo, sourcePropertyExpression);
            }
        }

        private IEnumerable<MemberBinding> GetFieldBindings<TSource, TDestination>(ParameterExpression parameterExpression)
        {
            var sourceFieldInfos = typeof(TSource)
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);

            var destinationFieldInfos = typeof(TDestination)
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetField);

            foreach (var destinationFieldInfo in destinationFieldInfos)
            {
                var sourceFieldInfo = sourceFieldInfos
                    .FirstOrDefault(x => x.Name == destinationFieldInfo.Name &&
                                         x.FieldType == destinationFieldInfo.FieldType);

                if (sourceFieldInfo == null)
                    continue;

                var sourceFieldExpression = Expression.Field(parameterExpression, sourceFieldInfo);

                yield return Expression.Bind(destinationFieldInfo, sourceFieldExpression);
            }
        }
    }
}
