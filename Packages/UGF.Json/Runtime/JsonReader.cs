using System;
using System.IO;
using System.Text;
using UGF.Json.Runtime.Values;

namespace UGF.Json.Runtime
{
    public class JsonReader
    {
        public TextReader Reader { get; }

        public JsonReader(string text)
        {
            Reader = new StringReader(text);
        }

        public JsonReader(TextReader reader)
        {
            Reader = reader;
        }

        public IJsonValue Read()
        {
            char ch = Peek();

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

            throw new JsonUnexpectedSymbolException("'{', '[', '\"', 'n', 't', 'f', '-' or '0-9' digit", ch);
        }

        private JsonObject ReadObject()
        {
            var value = new JsonObject();

            ReadAndValidate('{');

            JsonFormatUtility.SkipWhiteSpaces(Reader);

            if (Peek() == '}')
            {
                ReadNext();
            }
            else
            {
                while (true)
                {
                    JsonFormatUtility.SkipWhiteSpaces(Reader);

                    string key = ReadString().Raw;

                    JsonFormatUtility.SkipWhiteSpaces(Reader);

                    ReadNext();

                    JsonFormatUtility.SkipWhiteSpaces(Reader);

                    value.Add(key, Read());

                    JsonFormatUtility.SkipWhiteSpaces(Reader);

                    if (ReadNext() == '}')
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

            if (Peek() == ']')
            {
                ReadNext();
            }
            else
            {
                while (true)
                {
                    JsonFormatUtility.SkipWhiteSpaces(Reader);

                    value.Add(Read());

                    JsonFormatUtility.SkipWhiteSpaces(Reader);

                    if (ReadNext() == ']')
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
            var builder = new StringBuilder();

            char ch = Peek();

            if (!JsonFormatUtility.IsDigit(ch) && ch != '-')
            {
                throw new JsonUnexpectedSymbolException("'0-9' digit or '-' sign at the beginning of the number", ch);
            }

            if (ch == '-')
            {
                builder.Append(ReadNext());
                ch = Peek();

                if (!JsonFormatUtility.IsDigit(ch))
                {
                    throw new JsonUnexpectedSymbolException("'0-9' digit after '-' sign at the beginning of the number", ch);
                }

                builder.Append(ReadNext());
                ch = Peek();
            }

            while (JsonFormatUtility.IsDigit(ch))
            {
                builder.Append(ReadNext());
                ch = Peek();
            }

            if (ch == '.')
            {
                builder.Append(ReadNext());
                ch = Peek();

                if (!JsonFormatUtility.IsDigit(ch))
                {
                    throw new JsonUnexpectedSymbolException("'0-9' digit after '.' decimal separator", ch);
                }

                builder.Append(ReadNext());
                ch = Peek();

                while (JsonFormatUtility.IsDigit(ch))
                {
                    builder.Append(ReadNext());
                    ch = Peek();
                }
            }

            if (ch == 'e' || ch == 'E')
            {
                builder.Append(ReadNext());
                ch = Peek();

                if (ch != '-' && ch != '+')
                {
                    throw new JsonUnexpectedSymbolException("'-' or '+' after exponent symbol", ch);
                }

                builder.Append(ReadNext());
                ch = Peek();

                if (!JsonFormatUtility.IsDigit(ch))
                {
                    throw new JsonUnexpectedSymbolException("'0-9' digit after exponent sign", ch);
                }

                builder.Append(ReadNext());
                ch = Peek();

                while (JsonFormatUtility.IsDigit(ch))
                {
                    builder.Append(ReadNext());
                    ch = Peek();
                }
            }

            return new JsonValue(JsonValueType.Number, builder.ToString());
        }

        private JsonValue ReadString()
        {
            var builder = new StringBuilder();

            ReadAndValidate('"');

            while (CanRead())
            {
                char ch = ReadNext();

                switch (ch)
                {
                    case '\\':
                    {
                        builder.Append(ch);
                        builder.Append(ReadNext());
                        break;
                    }
                    case '"':
                    {
                        return new JsonValue(JsonValueType.String, builder.ToString());
                    }
                    default:
                    {
                        builder.Append(ch);
                        break;
                    }
                }
            }

            throw new JsonUnexpectedSymbolException("'\"' at the end of the string", (char)Reader.Peek());
        }

        private bool CanRead()
        {
            CheckStreamEnd();

            return true;
        }

        private char Peek()
        {
            CheckStreamEnd();

            return (char)Reader.Peek();
        }

        private char ReadNext()
        {
            CheckStreamEnd();

            return (char)Reader.Read();
        }

        private void ReadAndValidate(char expected)
        {
            char ch = ReadNext();

            if (ch != expected)
            {
                throw new JsonUnexpectedSymbolException(expected, (char)Reader.Peek());
            }
        }

        private void CheckStreamEnd()
        {
            if (Reader.Peek() == -1)
            {
                throw new InvalidOperationException("Reader reach end of the stream.");
            }
        }
    }
}
