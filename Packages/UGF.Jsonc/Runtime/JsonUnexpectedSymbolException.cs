using System;

namespace UGF.Jsonc.Runtime
{
    /// <summary>
    /// Represents error that occur when unexpected symbol specified.
    /// </summary>
    public sealed class JsonUnexpectedSymbolException : Exception
    {
        public JsonUnexpectedSymbolException(string expected, char actual) : base($"Expected {expected}, but was '{actual}' ({((int)actual):x4}).")
        {
        }

        public JsonUnexpectedSymbolException(char expected, char actual) : base($"Expected '{expected}', but was '{actual}' ({((int)actual):x4}).")
        {
        }
    }
}
