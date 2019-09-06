using System.Collections.Generic;

namespace UGF.Jsonc.Runtime.Values
{
    /// <summary>
    /// Represents Json object value.
    /// </summary>
    public class JsonObject : Dictionary<string, IJsonValue>, IJsonValue
    {
        public JsonValueType Type { get; } = JsonValueType.Object;

        public override string ToString()
        {
            return $"Count: {Count.ToString()}";
        }
    }
}
