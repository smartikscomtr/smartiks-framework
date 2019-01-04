using System;
using System.Linq;
using System.Linq.Expressions;
using Smartiks.Framework.Data.Abstractions;

namespace Smartiks.Framework.Data.Extensions
{
    public static class QueryOrderExtensions
    {
        public static IOrderedQueryable<TQueryable> ApplyTo<TQueryable>(this QueryOrder queryOrder, IQueryable<TQueryable> queryable)
        {
            if (queryable.Expression.Type == typeof(IOrderedQueryable<TQueryable>) && queryable is IOrderedQueryable<TQueryable> orderedQueryable)
            {
                switch (queryOrder.IsDescending)
                {
                    case false:

                        switch (queryOrder.Expression)
                        {
                            case Expression<Func<TQueryable, string>> stringExpression:

                                queryable = orderedQueryable.ThenBy(stringExpression);

                                break;

                            case Expression<Func<TQueryable, int>> intExpression:

                                queryable = orderedQueryable.ThenBy(intExpression);

                                break;

                            case Expression<Func<TQueryable, int?>> nullableIntExpression:

                                queryable = orderedQueryable.ThenBy(nullableIntExpression);

                                break;

                            case Expression<Func<TQueryable, decimal>> decimalExpression:

                                queryable = orderedQueryable.ThenBy(decimalExpression);

                                break;

                            case Expression<Func<TQueryable, decimal?>> nullableDecimalExpression:

                                queryable = orderedQueryable.ThenBy(nullableDecimalExpression);

                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(queryOrder.Expression), queryOrder.Expression, null);
                        }

                        break;

                    default:

                        switch (queryOrder.Expression)
                        {
                            case Expression<Func<TQueryable, string>> stringExpression:

                                queryable = orderedQueryable.ThenByDescending(stringExpression);

                                break;

                            case Expression<Func<TQueryable, int>> intExpression:

                                queryable = orderedQueryable.ThenByDescending(intExpression);

                                break;

                            case Expression<Func<TQueryable, int?>> nullableIntExpression:

                                queryable = orderedQueryable.ThenByDescending(nullableIntExpression);

                                break;

                            case Expression<Func<TQueryable, decimal>> decimalExpression:

                                queryable = orderedQueryable.ThenByDescending(decimalExpression);

                                break;

                            case Expression<Func<TQueryable, decimal?>> nullableDecimalExpression:

                                queryable = orderedQueryable.ThenByDescending(nullableDecimalExpression);

                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(queryOrder.Expression), queryOrder.Expression, null);
                        }

                        break;
                }
            }
            else
            {
                switch (queryOrder.IsDescending)
                {
                    case false:

                        switch (queryOrder.Expression)
                        {
                            case Expression<Func<TQueryable, string>> stringExpression:

                                queryable = queryable.OrderBy(stringExpression);

                                break;

                            case Expression<Func<TQueryable, int>> intExpression:

                                queryable = queryable.OrderBy(intExpression);

                                break;

                            case Expression<Func<TQueryable, int?>> nullableIntExpression:

                                queryable = queryable.OrderBy(nullableIntExpression);

                                break;

                            case Expression<Func<TQueryable, decimal>> decimalExpression:

                                queryable = queryable.OrderBy(decimalExpression);

                                break;

                            case Expression<Func<TQueryable, decimal?>> nullableDecimalExpression:

                                queryable = queryable.OrderBy(nullableDecimalExpression);

                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(queryOrder.Expression), queryOrder.Expression, null);
                        }

                        break;

                    default:

                        switch (queryOrder.Expression)
                        {
                            case Expression<Func<TQueryable, string>> stringExpression:

                                queryable = queryable.OrderByDescending(stringExpression);

                                break;

                            case Expression<Func<TQueryable, int>> intExpression:

                                queryable = queryable.OrderByDescending(intExpression);

                                break;

                            case Expression<Func<TQueryable, int?>> nullableIntExpression:

                                queryable = queryable.OrderByDescending(nullableIntExpression);

                                break;

                            case Expression<Func<TQueryable, decimal>> decimalExpression:

                                queryable = queryable.OrderByDescending(decimalExpression);

                                break;

                            case Expression<Func<TQueryable, decimal?>> nullableDecimalExpression:

                                queryable = queryable.OrderByDescending(nullableDecimalExpression);

                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(queryOrder.Expression), queryOrder.Expression, null);
                        }

                        break;
                }
            }

            return (IOrderedQueryable<TQueryable>) queryable;
        }
    }
}
