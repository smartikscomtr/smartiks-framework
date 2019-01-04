using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace Smartiks.Framework.Data.Web
{
    /// <summary>
    /// An <see cref="IValueProvider"/> for Kendo formatted data.
    /// </summary>
    public class KendoQueryStringValueProvider : BindingSourceValueProvider, IEnumerableValueProvider, IKeyRewriterValueProvider
    {
        private readonly IDictionary<string, StringValues> _values;

        private PrefixContainer _prefixContainer;

        /// <summary>
        /// Gets the <see cref="CultureInfo"/> associated with the values.
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KendoQueryStringValueProvider"/> class.
        /// </summary>
        /// <param name="bindingSource">The <see cref="BindingSource"/> of the data.</param>
        /// <param name="values">The values.</param>
        /// <param name="culture">The culture to return with ValueProviderResult instances.</param>
        public KendoQueryStringValueProvider(BindingSource bindingSource, IDictionary<string, StringValues> values, CultureInfo culture) : base(bindingSource)
        {
            if (bindingSource == null)
            {
                throw new ArgumentNullException(nameof(bindingSource));
            }

            _values = values ?? throw new ArgumentNullException(nameof(values));

            Culture = culture;
        }

        protected PrefixContainer PrefixContainer
        {
            get
            {
                if (_prefixContainer == null)
                {
                    _prefixContainer = new PrefixContainer(_values.Keys);
                }

                return _prefixContainer;
            }
        }

        /// <inheritdoc />
        public override bool ContainsPrefix(string prefix)
        {
            return PrefixContainer.ContainsPrefix(prefix);
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetKeysFromPrefix(string prefix)
        {
            return PrefixContainer.GetKeysFromPrefix(prefix);
        }

        /// <inheritdoc />
        public override ValueProviderResult GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (_values.TryGetValue(key, out var values) && values.Count > 0)
            {
                return new ValueProviderResult(values, Culture);
            }

            return ValueProviderResult.None;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Always returns <see langword="null"/> because <see cref="JQueryFormValueProviderFactory"/> creates this
        /// <see cref="IValueProvider"/> with rewritten keys (if original contains brackets) or duplicate keys
        /// (that <see cref="FormValueProvider"/> will match).
        /// </remarks>
        public IValueProvider Filter()
        {
            return null;
        }
    }
}