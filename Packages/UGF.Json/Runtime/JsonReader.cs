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
            if (text == null) throw new ArgumentNullException(nameof(text));

            Reader = new StringReader(text);
        }

        public JsonReader(TextReader reader)
        {
            Reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        public IJsonValue Read()
        {
            SkipWhiteSpaceOrComment();

            char ch = Peek();

            if (ch == '/')
            {
                SkipComment();
            }

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

            SkipWhiteSpaceOrComment();

            if (Peek() == '}')
            {
                ReadNext();
            }
            else
            {
                while (true)
                {
                    SkipWhiteSpaceOrComment();

                    string key = ReadString().Raw;

                    SkipWhiteSpaceOrComment();

                    ReadNext();

                    SkipWhiteSpaceOrComment();

                    value.Add(key, Read());

                    SkipWhiteSpaceOrComment();

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
                    SkipWhiteSpaceOrComment();

                    value.Add(Read());

                    SkipWhiteSpaceOrComment();

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

            return JsonValue.Null;
        }

        private JsonValue ReadTrue()
        {
            ReadAndValidate('t');
            ReadAndValidate('r');
            ReadAndValidate('u');
            ReadAndValidate('e');

            return JsonValue.True;
        }

        private JsonValue ReadFalse()
        {
            ReadAndValidate('f');
            ReadAndValidate('a');
            ReadAndValidate('l');
            ReadAndValidate('s');
            ReadAndValidate('e');

            return JsonValue.False;
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
                        string raw = builder.ToString();
                        string unescaped = JsonFormatUtility.Unescape(raw);

                        return new JsonValue(JsonValueType.String, unescaped);
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

        private void SkipWhiteSpaceOrComment()
        {
            char ch = Peek();

            while (CanRead() && (char.IsWhiteSpace(ch) || ch == '/'))
            {
                if (char.IsWhiteSpace(ch))
                {
                    JsonFormatUtility.SkipWhiteSpaces(Reader);
                }

                ch = Peek();

                if (ch == '/')
                {
                    SkipComment();
                }

                ch = Peek();
            }
        }

        private void SkipComment()
        {
            ReadAndValidate('/');

            char ch = ReadNext();

            if (ch == '/')
            {
                while (ch != '\r' && ch != '\n')
                {
                    ReadNext();

                    ch = Peek();
                }
                return;
            }

            if (ch == '*')
            {
                while (CanRead())
                {
                    ch = ReadNext();

                    if (ch == '*')
                    {
                        ch = ReadNext();

                        if (ch == '/')
                        {
                            return;
                        }
                    }
                }

                if (!CanRead())
                {
                    throw new Exception("Reader reach end of the stream during comments read.");
                }
            }

            throw new JsonUnexpectedSymbolException("'/' or '*' after starting comment", ch);
        }

        private bool CanRead()
        {
            CheckStreamEnd();

            return Reader.Peek() != -1;
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
