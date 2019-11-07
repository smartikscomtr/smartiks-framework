using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Smartiks.Framework.Data.Web
{
    public class ObjectTypeModelBinder : IModelBinder
    {
        private readonly ILogger _logger;

        public ObjectTypeModelBinder(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<ObjectTypeModelBinder>();
        }

        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                var value = valueProviderResult.FirstValue;

                object model = null;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    var formats = new[]
                    {
                        "yyyy-MM-ddTHH:mm:ss.fffff",
                        "yyyy-MM-ddTHH:mm:ss.ffff",
                        "yyyy-MM-ddTHH:mm:ss.fff",
                        "yyyy-MM-ddTHH:mm:ss.ff",
                        "yyyy-MM-ddTHH:mm:ss.f",
                        "yyyy-MM-ddTHH:mm:ss",
                        "yyyy-MM-ddTHH:mm:ss.fffffK",
                        "yyyy-MM-ddTHH:mm:ss.ffffK",
                        "yyyy-MM-ddTHH:mm:ss.fffK",
                        "yyyy-MM-ddTHH:mm:ss.ffK",
                        "yyyy-MM-ddTHH:mm:ss.fK",
                        "yyyy-MM-ddTHH:mm:ssK"
                    };

                    if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.NoCurrentDateDefault, out var dateTimeValue))
                    {
                        model = dateTimeValue.ToUniversalTime();
                    }
                    else
                    {
                        model = value;
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(model);

                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                var isFormatException = exception is FormatException;

                if (!isFormatException && exception.InnerException != null)
                {
                    exception = ExceptionDispatchInfo.Capture(exception.InnerException).SourceException;
                }

                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    exception,
                    bindingContext.ModelMetadata);

                return Task.CompletedTask;
            }
        }
    }
}