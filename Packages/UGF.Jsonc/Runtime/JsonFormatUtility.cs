using System;
using System.IO;

namespace UGF.Jsonc.Runtime
{
    /// <summary>
    /// Provides utilities to format Json text.
    /// </summary>
    public static class JsonFormatUtility
    {
        /// <summary>
        /// Converts the specified Json text into compact layout.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        public static string ToCompact(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var reader = new StringReader(text);
            var writer = new StringWriter();

            ToCompact(reader, writer);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Converts the specified Json text into compact layout.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        /// <param name="writer">The text writer.</param>
        public static void ToCompact(TextReader reader, TextWriter writer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            SkipWhiteSpaces(reader);

            while (reader.Peek() != -1)
            {
                char ch = (char)reader.Read();

                switch (ch)
                {
                    case '"':
                    {
                        writer.Write(ch);

                        do
                        {
                            ch = (char)reader.Read();

                            writer.Write(ch);
                        }
                        while (ch != '"');
                        break;
                    }
                    case '/':
                    {
                        writer.Write(ch);

                        ch = (char)reader.Read();

                        if (ch == '/')
                        {
                            writer.Write('*');

                            while (ch != '\r' && ch != '\n')
                            {
                                writer.Write((char)reader.Read());

                                ch = (char)reader.Peek();
                            }

                            writer.Write('*');
                            writer.Write('/');
                        }
                        else if (ch == '*')
                        {
                            writer.Write(ch);

                            while (reader.Peek() != -1)
                            {
                                ch = (char)reader.Read();

                                writer.Write(ch);

                                if (ch == '*')
                                {
                                    ch = (char)reader.Read();

                                    writer.Write(ch);

                                    if (ch == '/')
                                    {
                                        break;
                                    }
                                }
                            }

                            if (reader.Peek() == -1)
                            {
                                throw new Exception("Reader reach end of the stream during comments read.");
                            }
                        }
                        else
                        {
                            throw new JsonUnexpectedSymbolException("'/' or '*' after starting comment", ch);
                        }
                        break;
                    }
                    case ' ':
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
                        writer.Write(ch);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Converts the specified text to readable layout.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <param name="indent">The indent size.</param>
        public static string ToReadable(string text, int indent = 4)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (indent < 0) throw new ArgumentException("The indent count must be greater or equal to zero.", nameof(indent));

            var reader = new StringReader(text);
            var writer = new StringWriter();

            ToReadable(reader, writer, indent);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Converts the specified text to readable layout.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        /// <param name="writer">The text writer.</param>
        /// <param name="indent">The indent size.</param>
        public static void ToReadable(TextReader reader, TextWriter writer, int indent = 4)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            if (indent < 0) throw new ArgumentException("The indent count must be greater or equal to zero.", nameof(indent));

            int depth = 0;

            while (reader.Peek() != -1)
            {
                char ch = (char)reader.Read();

                switch (ch)
                {
                    case '"':
                    {
                        writer.Write(ch);

                        do
                        {
                            ch = (char)reader.Read();

                            writer.Write(ch);
                        }
                        while (ch != '"');
                        break;
                    }
                    case '{':
                    case '[':
                    {
                        writer.Write(ch);
                        writer.WriteLine();

                        for (int i = 0, count = indent * ++depth; i < count; i++)
                        {
                            writer.Write(' ');
                        }
                        break;
                    }
                    case '}':
                    case ']':
                    {
                        writer.WriteLine();

                        for (int i = 0, count = indent * --depth; i < count; i++)
                        {
                            writer.Write(' ');
                        }

                        writer.Write(ch);
                        break;
                    }
                    case ',':
                    {
                        writer.Write(ch);
                        writer.WriteLine();

                        for (int i = 0, count = indent * depth; i < count; i++)
                        {
                            writer.Write(' ');
                        }
                        break;
                    }
                    case ':':
                    {
                        writer.Write(ch);
                        writer.Write(' ');
                        break;
                    }
                    case '/':
                    {
                        writer.Write(ch);

                        ch = (char)reader.Read();

                        if (ch == '/')
                        {
                            writer.Write(ch);

                            while (ch != '\r' && ch != '\n')
                            {
                                writer.Write((char)reader.Read());

                                ch = (char)reader.Peek();
                            }

                            writer.WriteLine();

                            for (int i = 0, count = indent * depth; i < count; i++)
                            {
                                writer.Write(' ');
                            }
                        }
                        else if (ch == '*')
                        {
                            writer.Write(ch);

                            while (reader.Peek() != -1)
                            {
                                ch = (char)reader.Read();

                                writer.Write(ch);

                                if (ch == '*')
                                {
                                    ch = (char)reader.Read();

                                    writer.Write(ch);

                                    if (ch == '/')
                                    {
                                        writer.WriteLine();
                                        break;
                                    }
                                }
                            }

                            for (int i = 0, count = indent * depth; i < count; i++)
                            {
                                writer.Write(' ');
                            }

                            if (reader.Peek() == -1)
                            {
                                throw new Exception("Reader reach end of the stream during comments read.");
                            }
                        }
                        else
                        {
                            throw new JsonUnexpectedSymbolException("'/' or '*' after starting comment", ch);
                        }
                        break;
                    }
                    case ' ':
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
                        writer.Write(ch);
                        break;
                    }
                }
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Escapes the specified text.
        /// </summary>
        /// <param name="text">The text to escape.</param>
        public static string Escape(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var reader = new StringReader(text);
            var writer = new StringWriter();

            Escape(reader, writer);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Escapes the specified text.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        /// <param name="writer">The text writer.</param>
        public static void Escape(TextReader reader, TextWriter writer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            while (reader.Peek() != -1)
            {
                char ch = (char)reader.Read();

                switch (ch)
                {
                    case '"':
                    {
                        writer.Write('\\');
                        writer.Write('"');
                        break;
                    }
                    case '\\':
                    {
                        writer.Write('\\');
                        writer.Write('\\');
                        break;
                    }
                    case '\b':
                    {
                        writer.Write('\\');
                        writer.Write('b');
                        break;
                    }
                    case '\f':
                    {
                        writer.Write('\\');
                        writer.Write('f');
                        break;
                    }
                    case '\n':
                    {
                        writer.Write('\\');
                        writer.Write('n');
                        break;
                    }
                    case '\r':
                    {
                        writer.Write('\\');
                        writer.Write('r');
                        break;
                    }
                    case '\t':
                    {
                        writer.Write('\\');
                        writer.Write('t');
                        break;
                    }
                    default:
                    {
                        writer.Write(ch);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Unescapes the specified text.
        /// </summary>
        /// <param name="text">The text to unescape.</param>
        public static string Unescape(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var reader = new StringReader(text);
            var writer = new StringWriter();

            Unescape(reader, writer);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Unescapes the specified text.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        /// <param name="writer">The text writer.</param>
        public static void Unescape(TextReader reader, TextWriter writer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            while (reader.Peek() != -1)
            {
                char ch = (char)reader.Read();

                if (ch == '\\')
                {
                    ch = (char)reader.Read();

                    switch (ch)
                    {
                        case '"':
                        case '\\':
                        case '/':
                        {
                            writer.Write(ch);
                            break;
                        }
                        case 'b':
                        {
                            writer.Write('\b');
                            break;
                        }
                        case 'f':
                        {
                            writer.Write('\f');
                            break;
                        }
                        case 'n':
                        {
                            writer.Write('\n');
                            break;
                        }
                        case 'r':
                        {
                            writer.Write('\r');
                            break;
                        }
                        case 't':
                        {
                            writer.Write('\t');
                            break;
                        }
                        case 'u':
                        {
                            char ch0 = (char)reader.Read();
                            char ch1 = (char)reader.Read();
                            char ch2 = (char)reader.Read();
                            char ch3 = (char)reader.Read();

                            if (!IsDigit(ch0)) throw new JsonUnexpectedSymbolException("'0-9' after 'u' symbol", ch0);
                            if (!IsDigit(ch1)) throw new JsonUnexpectedSymbolException("'0-9' after 'u' symbol", ch1);
                            if (!IsDigit(ch2)) throw new JsonUnexpectedSymbolException("'0-9' after 'u' symbol", ch2);
                            if (!IsDigit(ch3)) throw new JsonUnexpectedSymbolException("'0-9' after 'u' symbol", ch3);

                            int code = ParseUnicode(ch0, ch1, ch2, ch3);

                            writer.Write((char)code);
                            break;
                        }
                        default:
                        {
                            throw new JsonUnexpectedSymbolException("'\"', '\\', '/', 'b', 'f', 'n', 'r', 't' or 'u'", ch);
                        }
                    }
                }
                else
                {
                    writer.Write(ch);
                }
            }
        }

        /// <summary>
        /// Clears comments from the specified text.
        /// </summary>
        /// <param name="text">The text to clear.</param>
        public static string ClearComments(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var reader = new StringReader(text);
            var writer = new StringWriter();

            ClearComments(reader, writer);

            return writer.GetStringBuilder().ToString();
        }

        /// <summary>
        /// Clears comments from the specified text.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        /// <param name="writer">The text writer.</param>
        public static void ClearComments(TextReader reader, TextWriter writer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            while (reader.Peek() != -1)
            {
                char ch = (char)reader.Read();

                switch (ch)
                {
                    case '"':
                    {
                        writer.Write(ch);

                        do
                        {
                            ch = (char)reader.Read();

                            writer.Write(ch);
                        }
                        while (ch != '"');
                        break;
                    }
                    case '/':
                    {
                        ch = (char)reader.Read();

                        if (ch == '/')
                        {
                            while (ch != '\r' && ch != '\n')
                            {
                                reader.Read();

                                ch = (char)reader.Peek();
                            }

                            SkipWhiteSpaces(reader);
                        }
                        else if (ch == '*')
                        {
                            while (reader.Peek() != -1)
                            {
                                ch = (char)reader.Read();

                                if (ch == '*')
                                {
                                    ch = (char)reader.Read();

                                    if (ch == '/')
                                    {
                                        break;
                                    }
                                }
                            }

                            SkipWhiteSpaces(reader);

                            if (reader.Peek() == -1)
                            {
                                throw new Exception("Reader reach end of the stream during comments read.");
                            }
                        }
                        else
                        {
                            throw new JsonUnexpectedSymbolException("'/' or '*' after starting comment", ch);
                        }
                        break;
                    }
                    default:
                    {
                        writer.Write(ch);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Skips all white spaces from the current reader position and until non-whitespace character in the stream.
        /// </summary>
        /// <param name="reader">The text reader.</param>
        public static void SkipWhiteSpaces(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            while (reader.Peek() != -1 && char.IsWhiteSpace((char)reader.Peek()))
            {
                reader.Read();
            }
        }

        /// <summary>
        /// Determines whether the specified character in range of '0-9' digits.
        /// </summary>
        /// <param name="ch">The character to check.</param>
        public static bool IsDigit(char ch)
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

        /// <summary>
        /// Parses the specified four characters as unicode.
        /// </summary>
        /// <param name="ch0">The character.</param>
        /// <param name="ch1">The character.</param>
        /// <param name="ch2">The character.</param>
        /// <param name="ch3">The character.</param>
        public static int ParseUnicode(char ch0, char ch1, char ch2, char ch3)
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
