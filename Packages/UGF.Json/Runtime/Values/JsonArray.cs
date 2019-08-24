using System.Collections.Generic;

namespace UGF.Json.Runtime.Values
{
    public class JsonArray : List<IJsonValue>, IJsonValue
    {
        public JsonValueType Type { get; } = JsonValueType.Array;

        public override string ToString()
        {
            return $"Count: {Count.ToString()}";
        }
    }
}
