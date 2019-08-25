using System;
using System.Globalization;

namespace UGF.Json.Runtime.Values
{
    /// <summary>
    /// Represents a generic value, that can be Null, Boolean or Number.
    /// </summary>
    public class JsonValue : IJsonValue
    {
        public JsonValueType Type { get; }

        /// <summary>
        /// Gets the raw string representation of this value.
        /// </summary>
        public string Raw { get; }

        /// <summary>
        /// Gets value that represents 'null' value.
        /// </summary>
        public static JsonValue Null { get; } = new JsonValue(JsonValueType.Null, "null");

        /// <summary>
        /// Gets value that represents 'true' value.
        /// </summary>
        public static JsonValue True { get; } = new JsonValue(JsonValueType.Boolean, "true");

        /// <summary>
        /// Gets value that represents 'false' value.
        /// </summary>
        public static JsonValue False { get; } = new JsonValue(JsonValueType.Boolean, "false");

        /// <summary>
        /// Creates value with the specified type and raw string representation.
        /// </summary>
        /// <param name="type">The type of the value, that can be Null, Boolean or Number only.</param>
        /// <param name="raw">The value raw string representation.</param>
        public JsonValue(JsonValueType type, string raw)
        {
            if (type == JsonValueType.Object || type == JsonValueType.Array) throw new ArgumentException("The type of the value cannot be Object or Array.", nameof(type));

            Type = type;
            Raw = raw ?? throw new ArgumentNullException(nameof(raw));
        }

        /// <summary>
        /// Gets value as boolean.
        /// </summary>
        public bool GetBoolean()
        {
            if (Type != JsonValueType.Boolean) throw new InvalidOperationException($"The type of this value not a boolean: '{Type}'.");

            return bool.Parse(Raw);
        }

        /// <summary>
        /// Gets value as Int32.
        /// </summary>
        public int GetInt32()
        {
            if (Type != JsonValueType.Number) throw new InvalidOperationException($"The type of this value not a number: '{Type}'.");

            return int.Parse(Raw);
        }

        /// <summary>
        /// Gets value as Single.
        /// </summary>
        public float GetSingle()
        {
            if (Type != JsonValueType.Number) throw new InvalidOperationException($"The type of this value not a number: '{Type}'.");

            return float.Parse(Raw);
        }

        /// <summary>
        /// Gets value as unescaped string.
        /// </summary>
        public string GetString()
        {
            if (Type != JsonValueType.String) throw new InvalidOperationException($"The type of this value not a string: '{Type}'");

            return JsonFormatUtility.Unescape(Raw);
        }

        /// <summary>
        /// Creates boolean from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static JsonValue CreateBoolean(bool value)
        {
            return value ? True : False;
        }

        /// <summary>
        /// Creates number from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        public static JsonValue CreateNumber(int value, IFormatProvider formatProvider = null)
        {
            string raw = formatProvider != null ? value.ToString(formatProvider) : value.ToString();

            return new JsonValue(JsonValueType.Number, raw);
        }

        /// <summary>
        /// Creates number from the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="formatProvider">The format provider.</param>
        public static JsonValue CreateNumber(float value, IFormatProvider formatProvider = null)
        {
            string raw = formatProvider != null ? value.ToString(formatProvider) : value.ToString(CultureInfo.InvariantCulture);

            return new JsonValue(JsonValueType.Number, raw);
        }

        /// <summary>
        /// Creates string from the specified value.
        /// <para>
        /// The specified value will be escaped.
        /// </para>
        /// </summary>
        /// <param name="value">The value.</param>
        public static JsonValue CreateString(string value)
        {
            value = JsonFormatUtility.Escape(value);

            return new JsonValue(JsonValueType.String, value);
        }

        protected bool Equals(JsonValue other)
        {
            return Type == other.Type && Raw == other.Raw;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JsonValue)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Type * 397) ^ (Raw != null ? Raw.GetHashCode() : 0);
            }
        }

        public static bool operator ==(JsonValue left, JsonValue right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(JsonValue left, JsonValue right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Raw;
        }
    }
}
