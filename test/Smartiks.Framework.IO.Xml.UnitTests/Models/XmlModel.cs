using System;
using System.Collections.Generic;

namespace Smartiks.Framework.IO.Xml.UnitTests.Models
{
    public class XmlModel
    {
        public int IntValue { get; set; }

        public int? NullableIntValue { get; set; }

        public decimal DecimalValue { get; set; }

        public decimal? NullableDecimalValue { get; set; }

        public string StringValue { get; set; }


        public override bool Equals(object obj)
        {
            var model = obj as XmlModel;

            return model != null &&
                   IntValue == model.IntValue &&
                   EqualityComparer<int?>.Default.Equals(NullableIntValue, model.NullableIntValue) &&
                   DecimalValue == model.DecimalValue &&
                   EqualityComparer<decimal?>.Default.Equals(NullableDecimalValue, model.NullableDecimalValue) &&
                   StringValue == model.StringValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IntValue, NullableIntValue, DecimalValue, NullableDecimalValue, StringValue);
        }
    }
}
