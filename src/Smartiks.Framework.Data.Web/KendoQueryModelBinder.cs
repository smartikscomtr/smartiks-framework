using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Smartiks.Framework.Data.Abstractions;

namespace Smartiks.Framework.Data.Web
{
    public class KendoQueryModelBinder<TQueryable> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }


            bindingContext.Result =
                TryGetQuery(bindingContext, out var query) ?
                    ModelBindingResult.Success(query) :
                    ModelBindingResult.Failed();

            return Task.CompletedTask;
        }


        private bool TryGetQuery(ModelBindingContext bindingContext, out Query<TQueryable> query)
        {
            QueryCriteria<TQueryable> criteria = null;

            if (TryGetCriteria(bindingContext, out var queryCriteria))
            {
                criteria = queryCriteria;
            }


            List<QueryOrder> orders = null;

            if (TryGetOrders(bindingContext, out var queryOrders))
            {
                orders = queryOrders;
            }
            

            QuerySegment segment = null;

            if (TryGetSegment(bindingContext, out var querySegment))
            {
                segment = querySegment;
            }


            if (criteria != null || orders != null || segment != null)
            {
                query = new Query<TQueryable> {
                    Criteria = criteria,
                    Orders = orders,
                    Segment = segment
                };

                return true;
            }

            query = null;

            return false;
        }

        private bool TryGetCriteria(ModelBindingContext bindingContext, out QueryCriteria<TQueryable> queryCriteria)
        {
            if (TryGetFilter(bindingContext, "filter", out var filter))
            {
                queryCriteria = new QueryCriteria<TQueryable> {
                    Expression = filter.Expression
                };

                return true;
            }

            queryCriteria = null;

            return false;
        }

        private bool TryGetFilter(ModelBindingContext bindingContext, string prefix, out KendoFilter filter)
        {
            if (TryGetFilterLogic(bindingContext, prefix, out var logic))
            {
                var filters = new List<KendoFilter>();

                var filterIndex = 0;

                while (true)
                {
                    if (!TryGetFilter(bindingContext, $"{prefix}[filters][{filterIndex}]", out var subFilter))
                        break;

                    filters.Add(subFilter);

                    filterIndex++;
                }

                filter = new KendoFilter {
                    Logic = logic,
                    Filters = filters
                };

                return true;
            }

            if (TryGetFilterField(bindingContext, prefix, out var field) && TryGetFilterValue(bindingContext, prefix, out var value))
            {
                filter = new KendoFilter {
                    Field = field,
                    Operation = TryGetFilterOperation(bindingContext, prefix, out var operation) ? operation : null,
                    Value = value
                };

                return true;
            }

            filter = null;

            return false;
        }

        private bool TryGetFilterLogic(ModelBindingContext bindingContext, string prefix, out string logic)
        {
            var logicValue = bindingContext.ValueProvider.GetValue($"{prefix}[logic]");

            if (logicValue.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(logicValue));

            if (logicValue.Length == 1)
            {
                logic = logicValue.FirstValue;

                return true;
            }

            logic = null;

            return false;
        }

        private bool TryGetFilterField(ModelBindingContext bindingContext, string prefix, out string field)
        {
            var fieldValue = bindingContext.ValueProvider.GetValue($"{prefix}[field]");

            if (fieldValue.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(fieldValue));

            if (fieldValue.Length == 1)
            {
                field = fieldValue.FirstValue;

                return true;
            }

            field = null;

            return false;
        }

        private bool TryGetFilterOperation(ModelBindingContext bindingContext, string prefix, out string operation)
        {
            var operationValue = bindingContext.ValueProvider.GetValue($"{prefix}[operator]");

            if (operationValue.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(operationValue));

            if (operationValue.Length == 1)
            {
                operation = operationValue.FirstValue;

                return true;
            }

            operation = null;

            return false;
        }

        private bool TryGetFilterValue(ModelBindingContext bindingContext, string prefix, out string value)
        {
            var valueValue = bindingContext.ValueProvider.GetValue($"{prefix}[value]");

            if (valueValue.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(valueValue));

            if (valueValue.Length == 1)
            {
                value = valueValue.FirstValue;

                return true;
            }

            value = null;

            return false;
        }

        private bool TryGetOrders(ModelBindingContext bindingContext, out List<QueryOrder> queryOrders)
        {
            queryOrders = new List<QueryOrder>();

            var sortIndex = 0;

            while (true)
            {
                var sortFieldValue = bindingContext.ValueProvider.GetValue($"sort[{sortIndex}][field]");

                if (sortFieldValue.Length == 0)
                    break;

                if (sortFieldValue.Length != 1)
                    throw new ArgumentOutOfRangeException(nameof(sortFieldValue), sortFieldValue.Length, null);


                var sortDirValue = bindingContext.ValueProvider.GetValue($"sort[{sortIndex}][dir]");

                if (sortDirValue.Length != 0 && sortDirValue.Length != 1)
                    throw new ArgumentOutOfRangeException(nameof(sortDirValue), sortDirValue.Length, null);


                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(sortFieldValue.FirstValue, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(sortFieldValue), sortFieldValue.FirstValue, null);


                var delegateType = typeof(Func<,>).MakeGenericType(documentType, propertyInfo.PropertyType);

                var parameter = Expression.Parameter(documentType, "queryable");

                var body = Expression.MakeMemberAccess(parameter, propertyInfo);

                var queryOrder = new QueryOrder {
                    Expression = Expression.Lambda(delegateType, body, parameter),
                    IsDescending = sortDirValue.Length == 1 && sortDirValue.FirstValue == "desc"
                };

                queryOrders.Add(queryOrder);


                sortIndex++;
            }

            return queryOrders.Count > 0;
        }

        private bool TryGetSegment(ModelBindingContext bindingContext, out QuerySegment querySegment)
        {
            var skipValue = bindingContext.ValueProvider.GetValue("skip");

            if (skipValue.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(skipValue));

            var takeValue = bindingContext.ValueProvider.GetValue("take");

            if (takeValue.Length > 1)
                throw new ArgumentOutOfRangeException(nameof(takeValue));

            if (skipValue.Length > 0 || takeValue.Length > 0)
            {
                querySegment = new QuerySegment
                {
                    StartIndex = skipValue.Length > 0 ? (int) Convert.ChangeType(skipValue.FirstValue, TypeCode.Int32) : 0,
                    Count = takeValue.Length > 0 ? (int?) Convert.ChangeType(takeValue.FirstValue, TypeCode.Int32) : null
                };

                return true;
            }

            querySegment = null;

            return false;
        }


        private class KendoFilter
        {
            public string Logic { get; set; }

            public IEnumerable<KendoFilter> Filters { get; set; }


            public string Field { get; set; }

            public string Operation { get; set; }

            public object Value { get; set; }


            public Expression<Func<TQueryable, bool>> Expression
            {
                get
                {
                    var documentType = typeof(TQueryable);

                    var parameter = System.Linq.Expressions.Expression.Parameter(documentType, "queryable");

                    var body = ToExpression(parameter, this);

                    return System.Linq.Expressions.Expression.Lambda<Func<TQueryable, bool>>(body, parameter);
                }
            }


            public static Expression ToExpression(ParameterExpression parameter, KendoFilter filter)
            {
                if (filter == null)
                    throw new ArgumentNullException(nameof(filter));

                if (filter.Filters != null)
                {
                    if (filter.Filters.Count() < 2)
                        throw new ArgumentOutOfRangeException(nameof(filter.Filters), filter.Filters, "Number of Filters must be at least 2.");

                    switch (filter.Logic)
                    {
                        case "or":
                            return OrElseExpression(parameter, filter.Filters);

                        case "and":
                        case null:
                            return AndAlsoExpression(parameter, filter.Filters);

                        default:
                            throw new ArgumentOutOfRangeException(nameof(filter.Logic), filter.Logic, "Logic not supported.");
                    }
                }

                switch (filter.Operation)
                {
                    case "eq":
                    case null:
                        return EqualExpression(parameter, filter.Field, filter.Value);

                    case "neq":
                        return NotEqualExpression(parameter, filter.Field, filter.Value);

                    case "lt":
                        return LessThanExpression(parameter, filter.Field, filter.Value);

                    case "lte":
                        return LessThanOrEqualExpression(parameter, filter.Field, filter.Value);

                    case "gt":
                        return GreaterThanExpression(parameter, filter.Field, filter.Value);

                    case "gte":
                        return GreaterThanOrEqualExpression(parameter, filter.Field, filter.Value);

                    case "startswith":
                        return StartsWithExpression(parameter, filter.Field, filter.Value);

                    case "endswith":
                        return EndsWithExpression(parameter, filter.Field, filter.Value);

                    case "contains":
                        return ContainsExpression(parameter, filter.Field, filter.Value);

                    case "doesnotcontain":
                        return NotContainsExpression(parameter, filter.Field, filter.Value);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(filter.Operation), filter.Operation, "Operator not supported.");
                }
            }

            public static Expression AndAlsoExpression(ParameterExpression parameter, IEnumerable<KendoFilter> filters)
            {
                Expression body = null;

                foreach (var filter in filters)
                {
                    if (body == null)
                    {
                        body = ToExpression(parameter, filter);

                        continue;
                    }

                    body = System.Linq.Expressions.Expression.AndAlso(body, ToExpression(parameter, filter));
                }

                return body;
            }

            public static Expression OrElseExpression(ParameterExpression parameter, IEnumerable<KendoFilter> filters)
            {
                Expression body = null;

                foreach (var filter in filters)
                {
                    if (body == null)
                    {
                        body = ToExpression(parameter, filter);

                        continue;
                    }

                    body = System.Linq.Expressions.Expression.OrElse(body, ToExpression(parameter, filter));
                }

                return body;
            }

            public static BinaryExpression EqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);


                var bodyLeft = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var bodyRight = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.Equal(bodyLeft, bodyRight);
            }

            public static BinaryExpression NotEqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);


                var bodyLeft = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var bodyRight = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.NotEqual(bodyLeft, bodyRight);
            }

            public static BinaryExpression LessThanExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);


                var bodyLeft = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var bodyRight = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.LessThan(bodyLeft, bodyRight);
            }

            public static BinaryExpression LessThanOrEqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);


                var bodyLeft = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var bodyRight = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.LessThanOrEqual(bodyLeft, bodyRight);
            }

            public static BinaryExpression GreaterThanExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);


                var bodyLeft = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var bodyRight = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.GreaterThan(bodyLeft, bodyRight);
            }

            public static BinaryExpression GreaterThanOrEqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);


                var bodyLeft = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var bodyRight = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.GreaterThanOrEqual(bodyLeft, bodyRight);
            }

            public static MethodCallExpression StartsWithExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);

                var methodInfo = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

                if (methodInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(methodInfo), propertyName);


                var instance = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var argument = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.Call(instance, methodInfo, argument);
            }

            public static MethodCallExpression EndsWithExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);

                var methodInfo = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

                if (methodInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(methodInfo), propertyName);


                var instance = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var argument = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.Call(instance, methodInfo, argument);
            }

            public static MethodCallExpression ContainsExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (propertyInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);

                var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                if (methodInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(methodInfo), propertyName);


                var instance = System.Linq.Expressions.Expression.MakeMemberAccess(parameter, propertyInfo);

                var constantValue = Convert.ChangeType(value, propertyInfo.PropertyType); //TODO

                var argument = System.Linq.Expressions.Expression.Constant(constantValue);

                return System.Linq.Expressions.Expression.Call(instance, methodInfo, argument);
            }

            public static UnaryExpression NotContainsExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var expression = ContainsExpression(parameter, propertyName, value);

                return System.Linq.Expressions.Expression.Not(expression);
            }
        }
    }
}
