using System.Collections.Generic;

namespace UGF.Jsonc.Runtime.Values
{
    /// <summary>
    /// Represents Json array value.
    /// </summary>
    public class JsonArray : List<IJsonValue>, IJsonValue
    {
        public JsonValueType Type { get; } = JsonValueType.Array;

        public override string ToString()
        {
            return $"Count: {Count.ToString()}";
        }
    }
}
