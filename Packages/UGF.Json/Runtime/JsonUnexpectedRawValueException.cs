using System;

namespace UGF.Json.Runtime
{
    /// <summary>
    /// Represents error that occur when unexpected raw value specified.
    /// </summary>
    public sealed class JsonUnexpectedRawValueException : Exception
    {
        public JsonUnexpectedRawValueException(string expected, string actual) : base($"Expected '{expected}', but was '{actual}'.")
        {
        }
    }
}
