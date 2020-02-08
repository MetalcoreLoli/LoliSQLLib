using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LoliSQLLib.DataAttributes.ValidationAttribute
{
    public class StringLength : System.Attribute, IValidationAttribute
    {
        public Int32 ExpectedLength { get; private set; }
        public StringLength(Int32 expected)
        {
            ExpectedLength = expected;
        }

        public bool IsValid(object value) 
            => ExpectedLength <= value.ToString().Length;
    }
}