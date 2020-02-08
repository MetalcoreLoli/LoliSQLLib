using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LoliSQLLib.DataAttributes.ValidationAttribute
{
    public interface IValidationAttribute
    {
        bool IsValid(object value);
    }
}