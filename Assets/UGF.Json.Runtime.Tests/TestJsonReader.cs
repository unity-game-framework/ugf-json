using NUnit.Framework;
using UGF.Json.Runtime.Values;
using UnityEngine;

namespace UGF.Json.Runtime.Tests
{
    public class TestJsonReader
    {
        private string m_compact;
        private string m_compact_c1;
        private string m_readable;
        private string m_readable_c1;
        private string m_readable_c3;

        [SetUp]
        public void Setup()
        {
            m_compact = Resources.Load<TextAsset>("compact").text;
            m_compact_c1 = Resources.Load<TextAsset>("compact_c1").text;
            m_readable = Resources.Load<TextAsset>("readable").text;
            m_readable_c1 = Resources.Load<TextAsset>("readable_c1").text;
            m_readable_c3 = Resources.Load<TextAsset>("readable_c3").text;
        }

        [Test]
        public void Read()
        {
            var reader0 = new JsonReader(m_compact);
            var reader1 = new JsonReader(m_readable);
            var reader2 = new JsonReader(m_compact_c1);
            var reader3 = new JsonReader(m_readable_c1);
            var reader4 = new JsonReader(m_readable_c3);

            IJsonValue value0 = reader0.Read();
            IJsonValue value1 = reader1.Read();
            IJsonValue value2 = reader2.Read();
            IJsonValue value3 = reader3.Read();
            IJsonValue value4 = reader4.Read();

            Assert.NotNull(value0);
            Assert.NotNull(value1);
            Assert.NotNull(value2);
            Assert.NotNull(value3);
            Assert.NotNull(value4);
        }
    }
}
