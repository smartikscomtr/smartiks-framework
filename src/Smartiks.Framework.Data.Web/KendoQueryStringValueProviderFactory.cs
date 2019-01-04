using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Smartiks.Framework.Data.Web
{
    /// <summary>
    /// An <see cref="IValueProviderFactory"/> for <see cref="KendoQueryStringValueProvider"/>.
    /// </summary>
    public class KendoQueryStringValueProviderFactory : IValueProviderFactory
    {
        /// <inheritdoc />
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var query = context.ActionContext.HttpContext.Request.Query;

            if (query != null && query.Count > 0)
            {
                var valueProvider =
                    new KendoQueryStringValueProvider
                    (
                        BindingSource.Query,
                        KendoKeyValuePairNormalizer.Normalize(query),
                        CultureInfo.InvariantCulture
                    );

                context.ValueProviders.Add(valueProvider);
            }

            return Task.CompletedTask;
        }
    }
}