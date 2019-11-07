using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Smartiks.Framework.Data.Web
{
    public class ObjectTypeModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(object))
            {
                var loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();

                return new ObjectTypeModelBinder(loggerFactory);
            }

            return null;
        }
    }
}