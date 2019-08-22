using System;
using UGF.Json.Runtime.Values;

namespace UGF.Json.Runtime
{
    public struct JsonReader
    {
        private JsonTextReader m_reader;

        public JsonReader(string text)
        {
            m_reader = new JsonTextReader(text);
        }

        public JsonReader(string text, int position, int length)
        {
            m_reader = new JsonTextReader(text, position, length);
        }

        public IJsonValue Read()
        {
            char ch = m_reader.Peek();

            switch (ch)
            {
                case '{': return ReadObject();
                case '[': return ReadArray();
                case '"': return ReadString();
                case 'n': return ReadNull();
                case 't': return ReadTrue();
                case 'f': return ReadFalse();
                case '-':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9': return ReadNumber();
            }

            throw new JsonUnexpectedSymbolException("'{', '[', '\"', 'n', 't', 'f', '-' or '0-9' digit", ch, m_reader.Position);
        }

        private JsonObject ReadObject()
        {
            var value = new JsonObject();

            ReadAndValidate('{');

            m_reader.SkipWhiteSpaces();

            if (m_reader.Peek() == '}')
            {
                m_reader.Read();
            }
            else
            {
                while (true)
                {
                    m_reader.SkipWhiteSpaces();

                    string key = ReadString().Raw;

                    m_reader.SkipWhiteSpaces();
                    m_reader.Read();
                    m_reader.SkipWhiteSpaces();

                    value.Add(key, Read());

                    m_reader.SkipWhiteSpaces();

                    if (m_reader.Read() == '}')
                    {
                        break;
                    }
                }
            }

            return value;
        }

        private JsonArray ReadArray()
        {
            var value = new JsonArray();

            ReadAndValidate('[');

            if (m_reader.Peek() == ']')
            {
                m_reader.Read();
            }
            else
            {
                while (true)
                {
                    m_reader.SkipWhiteSpaces();

                    value.Add(Read());

                    m_reader.SkipWhiteSpaces();

                    if (m_reader.Read() == ']')
                    {
                        break;
                    }
                }
            }

            return value;
        }

        private JsonValue ReadNull()
        {
            ReadAndValidate('n');
            ReadAndValidate('u');
            ReadAndValidate('l');
            ReadAndValidate('l');

            return new JsonValue(JsonValueType.Null, "null");
        }

        private JsonValue ReadTrue()
        {
            ReadAndValidate('t');
            ReadAndValidate('r');
            ReadAndValidate('u');
            ReadAndValidate('e');

            return new JsonValue(JsonValueType.Boolean, bool.TrueString);
        }

        private JsonValue ReadFalse()
        {
            ReadAndValidate('f');
            ReadAndValidate('a');
            ReadAndValidate('l');
            ReadAndValidate('s');
            ReadAndValidate('e');

            return new JsonValue(JsonValueType.Boolean, bool.FalseString);
        }

        private JsonValue ReadNumber()
        {
            int start = m_reader.Position;
            char ch = m_reader.Read();

            if (ch == '-')
            {
                ch = m_reader.Read();

                if (!IsDigit(ch))
                {
                    throw new JsonUnexpectedSymbolException("'0-9 digit after '-' sign at the beginning of the number", ch, m_reader.Position);
                }
            }

            ch = m_reader.ReadUntil(c => IsDigit(c));

            if (ch == '.')
            {
                ch = m_reader.Read();

                if (!IsDigit(ch))
                {
                    throw new JsonUnexpectedSymbolException("'0-9' digit after '.' decimal separator", ch, m_reader.Position);
                }

                ch = m_reader.ReadUntil(c => IsDigit(c));
            }

            if (ch == 'e' || ch == 'E')
            {
                ch = m_reader.Read();

                if (ch != '-' || ch != '+')
                {
                    throw new JsonUnexpectedSymbolException("'-' or '+' after exponent symbol", ch, m_reader.Position);
                }

                ch = m_reader.Read();

                if (!IsDigit(ch))
                {
                    throw new JsonUnexpectedSymbolException("'0-9' digit after exponent sign", ch, m_reader.Position);
                }

                m_reader.ReadUntil(c => IsDigit(c));
            }

            int end = m_reader.Position - 1;
            string raw = m_reader.Read(start, end);

            return new JsonValue(JsonValueType.Number, raw);
        }

        private JsonValue ReadString()
        {
            ReadAndValidate('"');

            int start = m_reader.Position;

            while (m_reader.CanRead())
            {
                char ch = m_reader.Read();

                switch (ch)
                {
                    case '\\':
                    {
                        m_reader.Read();
                        break;
                    }
                    case '"':
                    {
                        int end = m_reader.Position - 1;
                        string raw = m_reader.Read(start, end);

                        return new JsonValue(JsonValueType.String, raw);
                    }
                }
            }

            throw new JsonUnexpectedSymbolException("'\"' at the end of the string", m_reader.Peek(), m_reader.Position);
        }

        private void ReadAndValidate(char expected)
        {
            char ch = m_reader.Read();

            if (ch != expected)
            {
                throw new JsonUnexpectedSymbolException(expected, m_reader.Peek(), m_reader.Position);
            }
        }

        private static bool IsDigit(char ch)
        {
            switch (ch)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9': return true;
            }

            return false;
        }
    }
}
