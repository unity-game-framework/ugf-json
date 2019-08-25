using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace UGF.Json.Runtime.Tests
{
    public class TestJsonFormatUtility
    {
        private readonly string m_unescaped = "\" / \\ \b \f \n \r \t a";
        private readonly string m_escaped = "\\\" / \\\\ \\b \\f \\n \\r \\t a";
        private readonly string m_escaped2 = "\\\" / \\\\ \\b \\f \\n \\r \\t \\u0061";
        private string m_compact;
        private string m_compact_c1;
        private string m_readable;
        private string m_readable_c1;
        private string m_readable_c2;
        private string m_readable_c3;

        [SetUp]
        public void Setup()
        {
            m_compact = Resources.Load<TextAsset>("compact").text;
            m_compact_c1 = Resources.Load<TextAsset>("compact_c1").text;
            m_readable = Resources.Load<TextAsset>("readable").text;
            m_readable_c1 = Resources.Load<TextAsset>("readable_c1").text;
            m_readable_c2 = Resources.Load<TextAsset>("readable_c2").text;
            m_readable_c3 = Resources.Load<TextAsset>("readable_c3").text;
        }

        [Test]
        public void ToCompact()
        {
            string result0 = JsonFormatUtility.ToCompact(m_readable);
            string result1 = JsonFormatUtility.ToCompact(m_compact);
            string result2 = JsonFormatUtility.ToCompact(m_readable_c1);

            Assert.AreEqual(m_compact, result0);
            Assert.AreEqual(m_compact, result1);
            Assert.AreEqual(m_compact_c1, result2);
        }

        [Test]
        public void ToReadable()
        {
            string result0 = JsonFormatUtility.ToReadable(m_compact);
            string result1 = JsonFormatUtility.ToReadable(m_readable);
            string result2 = JsonFormatUtility.ToReadable(m_compact_c1);
            string result3 = JsonFormatUtility.ToReadable(m_readable_c1);

            Assert.AreEqual(m_readable, result0);
            Assert.AreEqual(m_readable, result1);
            Assert.AreEqual(m_readable_c2, result2);
            Assert.AreEqual(m_readable_c1, result3);
        }

        [Test]
        public void Escape()
        {
            string result0 = JsonFormatUtility.Escape(m_unescaped);
            string result1 = JsonFormatUtility.Escape("http://placehold.it/32x32");

            Assert.AreEqual(m_escaped, result0);
            Assert.AreEqual("http://placehold.it/32x32", result1);
        }

        [Test]
        public void Unescape()
        {
            string result0 = JsonFormatUtility.Unescape(m_escaped);
            string result1 = JsonFormatUtility.Unescape("http:\\/\\/placehold.it\\/32x32");
            string result2 = JsonFormatUtility.Unescape(m_escaped2);

            Assert.AreEqual(m_unescaped, result0);
            Assert.AreEqual("http://placehold.it/32x32", result1);
            Assert.AreEqual(m_unescaped, result2);
        }

        [Test]
        public void ClearComments()
        {
            string result0 = JsonFormatUtility.ClearComments(m_readable_c1);
            string result1 = JsonFormatUtility.ClearComments(m_readable_c2);
            string result2 = JsonFormatUtility.ClearComments(m_readable_c3);
            string result3 = JsonFormatUtility.ClearComments(m_compact_c1);

            Assert.AreEqual(m_readable, result0);
            Assert.AreEqual(m_readable, result1);
            Assert.AreEqual(m_readable, result2);
            Assert.AreEqual(m_compact, result3);
        }

        [Test]
        public void SkipWhiteSpaces()
        {
            string text = "0    12    3";
            var reader = new StringReader(text);

            Assert.AreEqual('0', (char)reader.Peek());

            JsonFormatUtility.SkipWhiteSpaces(reader);

            Assert.AreEqual('0', (char)reader.Peek());

            reader.Read();
            JsonFormatUtility.SkipWhiteSpaces(reader);

            Assert.AreEqual('1', (char)reader.Peek());

            reader.Read();

            Assert.AreEqual('2', (char)reader.Peek());

            reader.Read();
            JsonFormatUtility.SkipWhiteSpaces(reader);

            Assert.AreEqual('3', (char)reader.Peek());

            reader.Read();

            Assert.AreEqual(-1, reader.Peek());
        }

        [Test]
        public void IsDigit()
        {
            Assert.True(JsonFormatUtility.IsDigit('0'));
            Assert.True(JsonFormatUtility.IsDigit('1'));
            Assert.True(JsonFormatUtility.IsDigit('2'));
            Assert.True(JsonFormatUtility.IsDigit('3'));
            Assert.True(JsonFormatUtility.IsDigit('4'));
            Assert.True(JsonFormatUtility.IsDigit('5'));
            Assert.True(JsonFormatUtility.IsDigit('6'));
            Assert.True(JsonFormatUtility.IsDigit('7'));
            Assert.True(JsonFormatUtility.IsDigit('8'));
            Assert.True(JsonFormatUtility.IsDigit('9'));
            Assert.False(JsonFormatUtility.IsDigit('a'));
        }

        [Test]
        public void ParseUnicode()
        {
            char ch = 'a';
            string text = ((int)ch).ToString("x4");
            int code = JsonFormatUtility.ParseUnicode(text[0], text[1], text[2], text[3]);

            Assert.AreEqual(ch, (char)code);
        }
    }
}
