using System;
using UGF.Jsonc.Runtime.Values;

namespace UGF.Jsonc.Runtime
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
