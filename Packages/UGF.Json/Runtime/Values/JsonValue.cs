using System;
using System.Globalization;

namespace UGF.Json.Runtime.Values
{
    public class JsonValue : IJsonValue
    {
        public JsonValueType Type { get; }
        public string Raw { get; }

        public static JsonValue Null { get; } = new JsonValue(JsonValueType.Null, "null");
        public static JsonValue True { get; } = new JsonValue(JsonValueType.Boolean, "true");
        public static JsonValue False { get; } = new JsonValue(JsonValueType.Boolean, "false");

        public JsonValue(JsonValueType type, string raw)
        {
            if (type == JsonValueType.Object || type == JsonValueType.Array) throw new ArgumentException("The type of the value cannot be Object or Array.", nameof(type));

            Type = type;
            Raw = raw ?? throw new ArgumentNullException(nameof(raw));
        }

        public bool GetBoolean()
        {
            if (Type != JsonValueType.Boolean) throw new InvalidOperationException($"The type of this value not a boolean: '{Type}'.");

            return bool.Parse(Raw);
        }

        public int GetInt32()
        {
            if (Type != JsonValueType.Number) throw new InvalidOperationException($"The type of this value not a number: '{Type}'.");

            return int.Parse(Raw);
        }

        public float GetSingle()
        {
            if (Type != JsonValueType.Number) throw new InvalidOperationException($"The type of this value not a number: '{Type}'.");

            return float.Parse(Raw);
        }

        public string GetString()
        {
            if (Type != JsonValueType.String) throw new InvalidOperationException($"The type of this value not a string: '{Type}'");

            return JsonFormatUtility.Unescape(Raw);
        }

        public static JsonValue CreateBoolean(bool value)
        {
            return value ? True : False;
        }

        public static JsonValue CreateNumber(int value, IFormatProvider formatProvider = null)
        {
            string raw = formatProvider != null ? value.ToString(formatProvider) : value.ToString();

            return new JsonValue(JsonValueType.Number, raw);
        }

        public static JsonValue CreateNumber(float value, IFormatProvider formatProvider = null)
        {
            string raw = formatProvider != null ? value.ToString(formatProvider) : value.ToString(CultureInfo.InvariantCulture);

            return new JsonValue(JsonValueType.Number, raw);
        }

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
