using System;
using System.Text;

namespace UGF.Json.Runtime
{
    public struct JsonTextReader
    {
        public string Text { get { return m_text; } }
        public int Length { get { return m_length; } }
        public int Position { get { return m_position; } }

        private readonly string m_text;
        private readonly int m_length;
        private int m_position;

        public JsonTextReader(string text)
        {
            m_text = text ?? throw new ArgumentNullException(nameof(text));
            m_position = 0;
            m_length = text.Length;
        }

        public JsonTextReader(string text, int position, int length)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (position < 0) throw new ArgumentOutOfRangeException(nameof(position), "The position must be greater than zero.");
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "The length must be greater than zero.");
            if (text.Length - position < length) throw new ArgumentException("The position and length do not specify a valid range in text.");

            m_text = text;
            m_position = position;
            m_length = length;
        }

        public bool CanRead()
        {
            return m_position < m_length;
        }

        public void Move(int length)
        {
            m_position += length;
        }

        public char Peek()
        {
            return m_text[m_position];
        }

        public char Read()
        {
            return m_text[m_position++];
        }

        public void ReadUntil(StringBuilder result, char ch)
        {
            while (m_position < m_length && m_text[m_position] != ch)
            {
                result.Append(m_text[m_position++]);
            }
        }

        public void ReadUntil(StringBuilder result, Predicate<char> predicate)
        {
            while (m_position < m_length && predicate(m_text[m_position]))
            {
                result.Append(m_text[m_position++]);
            }
        }

        public void SkipWhiteSpaces()
        {
            while (m_position < m_length && char.IsWhiteSpace(m_text[m_position]))
            {
                m_position++;
            }
        }
    }
}
