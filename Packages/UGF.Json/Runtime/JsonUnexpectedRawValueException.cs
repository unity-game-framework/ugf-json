using System;

namespace UGF.Json.Runtime
{
    public sealed class JsonUnexpectedRawValueException : Exception
    {
        public JsonUnexpectedRawValueException(string expected, string actual) : base($"Expected '{expected}', but was '{actual}'.")
        {
        }
    }
}
