using System;
using UGF.Json.Runtime.Values;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Represents error that occur when unexpected Json value specified.
    /// </summary>
    public sealed class JsonUnexpectedValueTypeException : Exception
    {
        public JsonUnexpectedValueTypeException(Type expected, JsonValueType valueType, Type actual) : base($"Expected '{expected}' type for JsonValueType '{valueType}', but was '{actual}'.")
        {
        }
    }
}
