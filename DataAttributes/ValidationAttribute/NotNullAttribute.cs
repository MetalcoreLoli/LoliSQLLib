using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LoliSQLLib.DataAttributes.ValidationAttribute
{
    public class NotNullAttribute : System.Attribute, IValidationAttribute
    {
        public bool IsValid(object value) 
            => value != null;
    }
}