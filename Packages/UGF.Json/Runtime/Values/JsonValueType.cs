namespace UGF.Json.Runtime.Values
{
    /// <summary>
    /// Represents type of the Json value.
    /// </summary>
    public enum JsonValueType
    {
        /// <summary>
        /// Type is null value, that is 'null'.
        /// </summary>
        Null = 0,

        /// <summary>
        /// Type is boolean value, that can be 'true' or 'false'.
        /// </summary>
        Boolean = 1,

        /// <summary>
        /// Type is number value, that can be integer or floating-point number.
        /// </summary>
        Number = 2,

        /// <summary>
        /// Type is string value, that can be any string.
        /// </summary>
        String = 3,

        /// <summary>
        /// Type is object value, that contains other value as dictionary.
        /// </summary>
        Object = 4,

        /// <summary>
        /// Type is array value, that contains other values as array.
        /// </summary>
        Array = 5
    }
}
