using System;

namespace UGF.Json.Runtime
{
    public sealed class JsonUnexpectedSymbolException : Exception
    {
        public JsonUnexpectedSymbolException(string expected, char current, int position) : base($"Expected {expected}, but was '{current}' at '{position}' position.")
        {
        }

        public JsonUnexpectedSymbolException(char expected, char current, int position) : base($"Expected '{expected}', but was '{current}' at '{position}' position.")
        {
        }
    }
}
