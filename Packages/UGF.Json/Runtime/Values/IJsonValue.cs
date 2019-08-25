namespace UGF.Json.Runtime.Values
{
    /// <summary>
    /// Represents a Json value that can be Null, Boolean, Number, Object or Array.
    /// </summary>
    public interface IJsonValue
    {
        /// <summary>
        /// Gets the type of the Json value.
        /// </summary>
        JsonValueType Type { get; }
    }
}
