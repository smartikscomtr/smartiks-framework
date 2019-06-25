using Microsoft.AspNetCore.Mvc.ModelBinding;
using Smartiks.Framework.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SystemExpression = System.Linq.Expressions.Expression;

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
                query = new Query<TQueryable>
                {
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
                queryCriteria = new QueryCriteria<TQueryable>
                {
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

                filter = new KendoFilter
                {
                    Logic = logic,
                    Filters = filters
                };

                return true;
            }

            if (TryGetFilterField(bindingContext, prefix, out var field) && TryGetFilterValue(bindingContext, prefix, out var value))
            {
                filter = new KendoFilter
                {
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

                var parameter = SystemExpression.Parameter(documentType, "queryable");

                var body = SystemExpression.MakeMemberAccess(parameter, propertyInfo);

                var queryOrder = new QueryOrder
                {
                    Expression = SystemExpression.Lambda(delegateType, body, parameter),
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
                    StartIndex = skipValue.Length > 0 ? (int)Convert.ChangeType(skipValue.FirstValue, TypeCode.Int32) : 0,
                    Count = takeValue.Length > 0 ? (int?)Convert.ChangeType(takeValue.FirstValue, TypeCode.Int32) : null
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

                    var parameter = SystemExpression.Parameter(documentType, "queryable");

                    var body = ToExpression(parameter, this);

                    return SystemExpression.Lambda<Func<TQueryable, bool>>(body, parameter);
                }
            }

            private static SystemExpression ToExpression(ParameterExpression parameter, KendoFilter filter)
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

            private static SystemExpression AndAlsoExpression(ParameterExpression parameter, IEnumerable<KendoFilter> filters)
            {
                SystemExpression body = null;

                foreach (var filter in filters)
                {
                    if (body == null)
                    {
                        body = ToExpression(parameter, filter);

                        continue;
                    }

                    body = SystemExpression.AndAlso(body, ToExpression(parameter, filter));
                }

                return body;
            }

            private static SystemExpression OrElseExpression(ParameterExpression parameter, IEnumerable<KendoFilter> filters)
            {
                SystemExpression body = null;

                foreach (var filter in filters)
                {
                    if (body == null)
                    {
                        body = ToExpression(parameter, filter);

                        continue;
                    }

                    body = SystemExpression.OrElse(body, ToExpression(parameter, filter));
                }

                return body;
            }

            private static BinaryExpression EqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.Equal(bodyLeft, bodyRight);
            }

            private static BinaryExpression NotEqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.NotEqual(bodyLeft, bodyRight);
            }

            private static BinaryExpression LessThanExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.LessThan(bodyLeft, bodyRight);
            }

            private static BinaryExpression LessThanOrEqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.LessThanOrEqual(bodyLeft, bodyRight);
            }

            private static BinaryExpression GreaterThanExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.GreaterThan(bodyLeft, bodyRight);
            }

            private static BinaryExpression GreaterThanOrEqualExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.GreaterThanOrEqual(bodyLeft, bodyRight);
            }

            private static MethodCallExpression StartsWithExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var methodInfo = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

                if (methodInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(methodInfo), propertyName);

                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.Call(bodyLeft, methodInfo, bodyRight);
            }

            private static MethodCallExpression EndsWithExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var methodInfo = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

                if (methodInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(methodInfo), propertyName);

                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.Call(bodyLeft, methodInfo, bodyRight);
            }

            private static MethodCallExpression ContainsExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                if (methodInfo == null)
                    throw new ArgumentOutOfRangeException(nameof(methodInfo), propertyName);

                var (bodyLeft, bodyRight) = GetBodyExpressions(parameter, propertyName, value);

                return SystemExpression.Call(bodyLeft, methodInfo, bodyRight);
            }

            private static UnaryExpression NotContainsExpression(ParameterExpression parameter, string propertyName, object value)
            {
                var expression = ContainsExpression(parameter, propertyName, value);

                return SystemExpression.Not(expression);
            }

            private static (SystemExpression, UnaryExpression) GetBodyExpressions(ParameterExpression parameter, string propertyName, object value)
            {
                var documentType = typeof(TQueryable);

                if (propertyName.Contains("."))
                {
                    var propertyType = documentType;

                    var propertyNames = propertyName.Split('.');

                    SystemExpression bodyLeft = parameter;

                    foreach (var property in propertyNames)
                    {
                        var propertyInfo = propertyType.GetProperty(property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                        if (propertyInfo == null)
                            throw new ArgumentOutOfRangeException(nameof(propertyInfo), propertyName);

                        propertyType = propertyInfo.PropertyType;

                        bodyLeft = SystemExpression.Property(bodyLeft, property);
                    }

                    var bodyRight =
                        SystemExpression.Convert(
                            SystemExpression.Constant(GetConstantValue(value, bodyLeft.Type)),
                            bodyLeft.Type
                        );

                    return (bodyLeft, bodyRight);
                }
                else
                {
                    var propertyInfo = documentType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                    if (propertyInfo == null)
                        throw new ArgumentOutOfRangeException(nameof(propertyName), propertyName);

                    var bodyLeft = SystemExpression.MakeMemberAccess(parameter, propertyInfo);

                    var bodyRight =
                        SystemExpression.Convert(
                            SystemExpression.Constant(GetConstantValue(value, propertyInfo.PropertyType)),
                            propertyInfo.PropertyType
                        );

                    return (bodyLeft, bodyRight);
                }
            }

            private static object GetConstantValue(object value, Type propertyType)
            {
                if (value == null)
                    return null;

                var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                return Convert.ChangeType(value, type);
            }
        }
    }
}