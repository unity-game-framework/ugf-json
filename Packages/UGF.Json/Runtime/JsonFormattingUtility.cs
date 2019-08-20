using System;
using System.Text;

namespace UGF.Json.Runtime
{
    public static class JsonFormattingUtility
    {
        public static string ToCompact(string text)
        {
            var result = new StringBuilder(text.Length);
            var reader = new JsonTextReader(text);

            ToCompact(result, ref reader);

            return result.ToString();
        }

        public static void ToCompact(StringBuilder result, ref JsonTextReader reader)
        {
            reader.SkipWhiteSpaces();

            while (reader.CanRead())
            {
                char ch = reader.Read();

                switch (ch)
                {
                    case '\"':
                    {
                        result.Append('"');
                        reader.ReadUntil(result, '"');
                        result.Append('"');
                        reader.Read();
                        break;
                    }
                    case '\b':
                    case '\f':
                    case '\n':
                    case '\r':
                    case '\t':
                    {
                        break;
                    }
                    default:
                    {
                        result.Append(ch);
                        break;
                    }
                }

                reader.SkipWhiteSpaces();
            }
        }

        public static string ToReadable(string text, int indent = 4)
        {
            var result = new StringBuilder(text.Length);
            var reader = new JsonTextReader(text);

            ToReadable(result, ref reader, indent);

            return result.ToString();
        }

        public static void ToReadable(StringBuilder result, ref JsonTextReader reader, int indent = 4)
        {
            int depth = 0;

            while (reader.CanRead())
            {
                char ch = reader.Read();

                switch (ch)
                {
                    case '\"':
                    {
                        result.Append('"');
                        reader.ReadUntil(result, '"');
                        result.Append('"');
                        reader.Read();
                        break;
                    }
                    case '{':
                    case '[':
                    {
                        result.Append(ch);
                        result.AppendLine();
                        result.Append(' ', indent * ++depth);
                        break;
                    }
                    case '}':
                    case ']':
                    {
                        result.AppendLine();
                        result.Append(' ', indent * --depth);
                        result.Append(ch);
                        break;
                    }
                    case ',':
                    {
                        result.Append(ch);
                        result.AppendLine();
                        result.Append(' ', indent * depth);
                        break;
                    }
                    case ':':
                    {
                        result.Append(':');
                        result.Append(' ');
                        break;
                    }
                    default:
                    {
                        result.Append(ch);
                        break;
                    }
                }
            }
        }

        public static string Escape(string text)
        {
            var result = new StringBuilder(text.Length);
            var reader = new JsonTextReader(text);

            Escape(result, ref reader);

            return result.ToString();
        }

        public static void Escape(StringBuilder result, ref JsonTextReader reader)
        {
            while (reader.CanRead())
            {
                char ch = reader.Read();

                switch (ch)
                {
                    case '"':
                    {
                        result.Append('\\');
                        result.Append('"');
                        break;
                    }
                    case '\\':
                    {
                        result.Append('\\', 2);
                        break;
                    }
                    case '\b':
                    {
                        result.Append('\\');
                        result.Append('b');
                        break;
                    }
                    case '\f':
                    {
                        result.Append('\\');
                        result.Append('f');
                        break;
                    }
                    case '\n':
                    {
                        result.Append('\\');
                        result.Append('n');
                        break;
                    }
                    case '\r':
                    {
                        result.Append('\\');
                        result.Append('r');
                        break;
                    }
                    case '\t':
                    {
                        result.Append('\\');
                        result.Append('t');
                        break;
                    }
                    default:
                    {
                        result.Append(ch);
                        break;
                    }
                }
            }
        }

        public static string Unescape(string text)
        {
            var result = new StringBuilder(text.Length);
            var reader = new JsonTextReader(text);

            Unescape(result, ref reader);

            return result.ToString();
        }

        public static void Unescape(StringBuilder result, ref JsonTextReader reader)
        {
            while (reader.CanRead())
            {
                char ch = reader.Read();

                if (ch == '\\')
                {
                    ch = reader.Read();

                    switch (ch)
                    {
                        case '"':
                        case '\\':
                        case '/':
                        {
                            result.Append(ch);
                            break;
                        }
                        case 'b':
                        {
                            result.Append('\b');
                            break;
                        }
                        case 'f':
                        {
                            result.Append('\f');
                            break;
                        }
                        case 'n':
                        {
                            result.Append('\n');
                            break;
                        }
                        case 'r':
                        {
                            result.Append('\r');
                            break;
                        }
                        case 't':
                        {
                            result.Append('\t');
                            break;
                        }
                        case 'u':
                        {
                            char ch0 = reader.Read();
                            char ch1 = reader.Read();
                            char ch2 = reader.Read();
                            char ch3 = reader.Read();
                            int code = InternalParseUnicode(ch0, ch1, ch2, ch3);

                            result.Append((char)code);
                            break;
                        }
                        default:
                        {
                            throw new Exception($"Unexpected symbol to escape: '{ch}' at '{reader.Position}'.");
                        }
                    }
                }
                else
                {
                    result.Append(ch);
                }
            }
        }

        internal static int InternalParseUnicode(char ch0, char ch1, char ch2, char ch3)
        {
            return ((CharToNumber(ch0) * 16 + CharToNumber(ch1)) * 16 + CharToNumber(ch2)) * 16 + CharToNumber(ch3);
        }

        private static int CharToNumber(char ch)
        {
            if ('0' <= ch && ch <= '9')
            {
                return ch - '0';
            }

            if ('a' <= ch && ch <= 'f')
            {
                return ch - 'a' + 10;
            }

            if ('A' <= ch && ch <= 'F')
            {
                return ch - 'A' + 10;
            }

            throw new ArgumentException($"Can't convert specified char to number: '{ch}'.");
        }
    }
}
