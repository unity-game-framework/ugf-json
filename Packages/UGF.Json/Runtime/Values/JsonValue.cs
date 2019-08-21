using System;

namespace UGF.Json.Runtime.Values
{
    public class JsonValue : IJsonValue
    {
        public JsonValueType Type { get; }
        public string Raw { get; }

        public JsonValue(JsonValueType type, string raw)
        {
            Type = type;
            Raw = raw ?? throw new ArgumentNullException(nameof(raw));
        }
    }
}
