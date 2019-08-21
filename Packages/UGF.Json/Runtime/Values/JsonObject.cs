using System.Collections.Generic;

namespace UGF.Json.Runtime.Values
{
    public class JsonObject : Dictionary<string, IJsonValue>, IJsonValue
    {
        public JsonValueType Type { get; } = JsonValueType.Object;
    }
}
