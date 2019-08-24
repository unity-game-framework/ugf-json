using System;

namespace UGF.Json.Runtime
{
    public sealed class JsonUnexpectedSymbolException : Exception
    {
        public JsonUnexpectedSymbolException(string expected, char current) : base($"Expected {expected}, but was '{current}' ({((int)current):x4}).")
        {
        }

        public JsonUnexpectedSymbolException(char expected, char current) : base($"Expected '{expected}', but was '{current}' ({((int)current):x4}).")
        {
        }
    }
}
