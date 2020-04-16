using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.Web
{
    /// <summary>
    /// An <see cref="Microsoft.AspNetCore.Mvc.ModelBinding.IValueProviderFactory"/> for <see cref="SquareBracketedQueryStringValueProvider"/>.
    /// </summary>
    public class SquareBracketedQueryStringValueProviderFactory : IValueProviderFactory
    {
        /// <inheritdoc />
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var query = context.ActionContext.HttpContext.Request.Query;

            if (query?.Count > 0)
            {
                var valueProvider =
                    new SquareBracketedQueryStringValueProvider
                    (
                        BindingSource.Query,
                        SquareBracketedKeyValuePairNormalizer.Normalize(query),
                        CultureInfo.InvariantCulture
                    );

                context.ValueProviders.Add(valueProvider);
            }

            return Task.CompletedTask;
        }
    }
}