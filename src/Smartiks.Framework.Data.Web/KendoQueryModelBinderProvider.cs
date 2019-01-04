using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Smartiks.Framework.Data.Abstractions;

namespace Smartiks.Framework.Data.Web
{
    public class KendoQueryModelBinderProvider : IModelBinderProvider
    {
        public static Type KendoQueryModelBinderType = typeof(KendoQueryModelBinder<>);

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType.IsGenericType)
            {
                var modelGenericTypeDefinition = context.Metadata.ModelType.GetGenericTypeDefinition();

                if (modelGenericTypeDefinition == typeof(Query<>) && context.Metadata.ModelType.GenericTypeArguments.Length == 1)
                {
                    var queryableType = context.Metadata.ModelType.GenericTypeArguments[0];

                    var binderType = KendoQueryModelBinderType.MakeGenericType(queryableType);

                    return new BinderTypeModelBinder(binderType);
                }
            }

            return null;
        }
    }
}
