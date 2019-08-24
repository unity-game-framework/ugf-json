using System;
using System.Collections.Generic;
using System.IO;
using UGF.Json.Runtime.Values;

namespace UGF.Json.Runtime
{
    public class JsonWriter
    {
        public TextWriter Writer { get; }

        private readonly HashSet<object> m_references = new HashSet<object>();

        public JsonWriter() : this(new StringWriter())
        {
        }

        public JsonWriter(TextWriter writer)
        {
            Writer = writer;
        }

        public void Write(IJsonValue value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            switch (value.Type)
            {
                case JsonValueType.Null:
                {
                    if (value is JsonValue cast)
                    {
                        WriteNull(cast);
                    }
                    else
                    {
                        throw new JsonUnexpectedValueTypeException(typeof(JsonValue), value.Type, value.GetType());
                    }
                    break;
                }
                case JsonValueType.Boolean:
                {
                    if (value is JsonValue cast)
                    {
                        WriteBoolean(cast);
                    }
                    else
                    {
                        throw new JsonUnexpectedValueTypeException(typeof(JsonValue), value.Type, value.GetType());
                    }
                    break;
                }
                case JsonValueType.Number:
                {
                    if (value is JsonValue cast)
                    {
                        WriteNumber(cast);
                    }
                    else
                    {
                        throw new JsonUnexpectedValueTypeException(typeof(JsonValue), value.Type, value.GetType());
                    }
                    break;
                }
                case JsonValueType.String:
                {
                    if (value is JsonValue cast)
                    {
                        WriteString(cast);
                    }
                    else
                    {
                        throw new JsonUnexpectedValueTypeException(typeof(JsonValue), value.Type, value.GetType());
                    }
                    break;
                }
                case JsonValueType.Object:
                {
                    if (value is JsonObject cast)
                    {
                        WriteObject(cast);
                    }
                    else
                    {
                        throw new JsonUnexpectedValueTypeException(typeof(JsonObject), value.Type, value.GetType());
                    }
                    break;
                }
                case JsonValueType.Array:
                {
                    if (value is JsonArray cast)
                    {
                        WriteArray(cast);
                    }
                    else
                    {
                        throw new JsonUnexpectedValueTypeException(typeof(JsonArray), value.Type, value.GetType());
                    }
                    break;
                }
                default: throw new ArgumentOutOfRangeException(nameof(value), $"Unknown JsonValueType: '{value.Type}'.");
            }
        }

        private void WriteObject(JsonObject value)
        {
            m_references.Add(value);

            Writer.Write('{');

            int index = 0;

            foreach (KeyValuePair<string, IJsonValue> pair in value)
            {
                string key = pair.Key;
                IJsonValue element = pair.Value;

                Writer.Write('\"');
                Writer.Write(key);
                Writer.Write('\"');
                Writer.Write(':');

                if (m_references.Add(element))
                {
                    Write(element);

                    m_references.Remove(element);
                }
                else
                {
                    throw new ArgumentException($"Cannot write value, because of circular reference in object at '{pair.Key}' key.", nameof(value));
                }

                if (index++ < value.Count - 1)
                {
                    Writer.Write(',');
                }
            }

            Writer.Write('}');

            m_references.Remove(value);
        }

        private void WriteArray(JsonArray value)
        {
            m_references.Add(value);

            Writer.Write('[');

            for (int i = 0; i < value.Count; i++)
            {
                IJsonValue element = value[i];

                if (m_references.Add(element))
                {
                    Write(element);

                    m_references.Remove(element);
                }
                else
                {
                    throw new ArgumentException($"Cannot write value, because of circular reference in array at '{i}' index.", nameof(value));
                }

                if (i < value.Count - 1)
                {
                    Writer.Write(',');
                }
            }

            Writer.Write(']');

            m_references.Remove(value);
        }

        private void WriteNull(JsonValue value)
        {
            if (value.Raw != "null")
            {
                throw new JsonUnexpectedRawValueException("null", value.Raw);
            }

            Writer.Write(value.Raw);
        }

        private void WriteBoolean(JsonValue value)
        {
            if (value.Raw != "true" && value.Raw != "false")
            {
                throw new JsonUnexpectedRawValueException("'true' or 'false'", value.Raw);
            }

            Writer.Write(value.Raw);
        }

        private void WriteNumber(JsonValue value)
        {
            if (value.Raw == null)
            {
                throw new JsonUnexpectedRawValueException("not null number", value.Raw);
            }

            Writer.Write(value.Raw);
        }

        private void WriteString(JsonValue value)
        {
            if (value.Raw == null)
            {
                throw new JsonUnexpectedRawValueException("not null string", value.Raw);
            }

            string escaped = JsonFormatUtility.Escape(value.Raw);

            Writer.Write('\"');
            Writer.Write(escaped);
            Writer.Write('\"');
        }

        public override string ToString()
        {
            return Writer.ToString();
        }
    }
}
