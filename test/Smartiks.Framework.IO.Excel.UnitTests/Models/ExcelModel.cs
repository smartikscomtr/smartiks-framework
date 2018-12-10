using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Smartiks.Framework.IO.Excel.UnitTests.Models
{
    public class ExcelModel
    {
        [Display(Name = "IntValueDisplayName")]
        public int IntValue { get; set; }

        public int? NullableIntValue { get; set; }

        public decimal DecimalValue { get; set; }

        [DisplayFormat(DataFormatString = "#,##0.00")]
        public decimal? NullableDecimalValue { get; set; }

        [DisplayFormat(DataFormatString = "m/d/yyyy")]
        public DateTime DateTimeValue { get; set; }

        [DisplayFormat(DataFormatString = "d/m/yyyy")]
        public DateTime? NullableDateTimeValue { get; set; }

        [DisplayFormat(DataFormatString = "yyyy-m-d")]
        public DateTime DateValue { get; set; }

        [DisplayFormat(DataFormatString = "[$-en-US]mmmm d, yyyy;@")]
        public DateTime? NullableDateValue { get; set; }

        public string StringValue { get; set; }


        public override bool Equals(object obj)
        {
            var model = obj as ExcelModel;
            return model != null &&
                   IntValue == model.IntValue &&
                   EqualityComparer<int?>.Default.Equals(NullableIntValue, model.NullableIntValue) &&
                   DecimalValue == model.DecimalValue &&
                   EqualityComparer<decimal?>.Default.Equals(NullableDecimalValue, model.NullableDecimalValue) &&
                   DateTimeValue == model.DateTimeValue &&
                   EqualityComparer<DateTime?>.Default.Equals(NullableDateTimeValue, model.NullableDateTimeValue) &&
                   DateValue == model.DateValue &&
                   EqualityComparer<DateTime?>.Default.Equals(NullableDateValue, model.NullableDateValue) &&
                   StringValue == model.StringValue;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(IntValue);
            hash.Add(NullableIntValue);
            hash.Add(DecimalValue);
            hash.Add(NullableDecimalValue);
            hash.Add(DateTimeValue);
            hash.Add(NullableDateTimeValue);
            hash.Add(DateValue);
            hash.Add(NullableDateValue);
            hash.Add(StringValue);
            return hash.ToHashCode();
        }
    }
}
