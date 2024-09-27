using System.Linq.Expressions;
using System.Reflection;

namespace Common.Extension
{
    // 定義一個委派型別，用於動態生成表達式
    public delegate void PropertyMappingDelegate(object source, object destination);

    /// <summary> <see href="https://stackoverflow.com/questions/5537995/entity-framework-left-join"/> </summary>
    public static class EFCoreExtension
    {
        ///// <summary> Returns distinct elements from a sequence according to a specified key selector function. </summary>
        ///// <typeparam name="TSource">The type of the elements of source.</typeparam>
        ///// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
        ///// <param name="source">The sequence to remove duplicate elements from.</param>
        ///// <param name="keySelector">A function to extract the key for each element.</param>
        ///// <returns>An System.Collections.Generic.IEnumerable`1 that contains distinct elements from the source sequence.</returns>
        ///// <exception cref="ArgumentNullException"><paramref name="source"/> is a null reference.</exception>
        //public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //    HashSet<TKey> seenKeys = new HashSet<TKey>();
        //    foreach (TSource element in source)
        //    {
        //        if (seenKeys.Add(keySelector(element)))
        //        {
        //            yield return element;
        //        }
        //    }
        //}

        public static PropertyMappingDelegate CreateMap<TSource, TDestination>()
        {
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            var sourceParam = Expression.Parameter(typeof(object), "source");
            var sourceCast = Expression.Convert(sourceParam, sourceType);

            var destinationVar = Expression.Variable(destinationType, "destination");
            var destinationAssign = Expression.Assign(destinationVar, Expression.New(destinationType));

            var body = new Expression[] { destinationVar, destinationAssign };

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null && destinationProperty.PropertyType == sourceProperty.PropertyType)
                {
                    var sourcePropertyAccess = Expression.Property(sourceCast, sourceProperty);
                    var destinationPropertyAccess = Expression.Property(destinationVar, destinationProperty);
                    var assign = Expression.Assign(destinationPropertyAccess, sourcePropertyAccess);
                    body = body.Concat([assign]).ToArray();
                }
            }

            var block = Expression.Block([destinationVar], body);
            var lambda = Expression.Lambda<PropertyMappingDelegate>(block, sourceParam);

            return lambda.Compile();
        }

        public static IQueryable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(
            this IQueryable<TOuter> outer,
            IQueryable<TInner> inner,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            MethodInfo groupJoin = typeof(Queryable).GetMethods()
                .Single(m => m.ToString() == "System.Linq.IQueryable`1[TResult] GroupJoin[TOuter,TInner,TKey,TResult](System.Linq.IQueryable`1[TOuter], System.Collections.Generic.IEnumerable`1[TInner], System.Linq.Expressions.Expression`1[System.Func`2[TOuter,TKey]], System.Linq.Expressions.Expression`1[System.Func`2[TInner,TKey]], System.Linq.Expressions.Expression`1[System.Func`3[TOuter,System.Collections.Generic.IEnumerable`1[TInner],TResult]])")
                .MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(LeftJoinIntermediate<TOuter, TInner>));
            MethodInfo selectMany = typeof(Queryable).GetMethods()
                .Single(m => m.ToString() == "System.Linq.IQueryable`1[TResult] SelectMany[TSource,TCollection,TResult](System.Linq.IQueryable`1[TSource], System.Linq.Expressions.Expression`1[System.Func`2[TSource,System.Collections.Generic.IEnumerable`1[TCollection]]], System.Linq.Expressions.Expression`1[System.Func`3[TSource,TCollection,TResult]])")
                .MakeGenericMethod(typeof(LeftJoinIntermediate<TOuter, TInner>), typeof(TInner), typeof(TResult));

            var groupJoinResultSelector = (Expression<Func<TOuter, IEnumerable<TInner>, LeftJoinIntermediate<TOuter, TInner>>>)
                ((oneOuter, manyInners) => new LeftJoinIntermediate<TOuter, TInner> { OneOuter = oneOuter, ManyInners = manyInners });

            MethodCallExpression exprGroupJoin = Expression.Call(groupJoin, outer.Expression, inner.Expression, outerKeySelector, innerKeySelector, groupJoinResultSelector);

            var selectManyCollectionSelector = (Expression<Func<LeftJoinIntermediate<TOuter, TInner>, IEnumerable<TInner>>>)
                (t => t.ManyInners.DefaultIfEmpty());

            ParameterExpression paramUser = resultSelector.Parameters.First();

            ParameterExpression paramNew = Expression.Parameter(typeof(LeftJoinIntermediate<TOuter, TInner>), "t");
            MemberExpression propExpr = Expression.Property(paramNew, "OneOuter");

            LambdaExpression selectManyResultSelector = Expression.Lambda(new Replacer(paramUser, propExpr).Visit(resultSelector.Body), paramNew, resultSelector.Parameters.Skip(1).First());

            MethodCallExpression exprSelectMany = Expression.Call(selectMany, exprGroupJoin, selectManyCollectionSelector, selectManyResultSelector);

            return outer.Provider.CreateQuery<TResult>(exprSelectMany);
        }

        private sealed class LeftJoinIntermediate<TOuter, TInner>
        {
            public TOuter OneOuter { get; set; } = default!;
            public IEnumerable<TInner> ManyInners { get; set; } = default!;
        }

        private sealed class Replacer(ParameterExpression oldParam, Expression replacement) : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam = oldParam;
            private readonly Expression _replacement = replacement;

            public override Expression Visit(Expression? exp) => exp == _oldParam ? _replacement : base.Visit(exp) ?? _replacement;
        }
    }
}
