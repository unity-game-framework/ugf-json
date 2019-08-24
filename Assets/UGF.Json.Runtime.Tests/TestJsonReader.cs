using NUnit.Framework;
using UGF.Json.Runtime.Values;
using UnityEngine;

namespace UGF.Json.Runtime.Tests
{
    public class TestJsonReader
    {
        private string m_compact;
        private string m_readable;

        [SetUp]
        public void Setup()
        {
            m_compact = Resources.Load<TextAsset>("compact").text;
            m_readable = Resources.Load<TextAsset>("readable").text;
        }

        [Test]
        public void Read()
        {
            var reader0 = new JsonReader(m_compact);
            var reader1 = new JsonReader(m_readable);

            IJsonValue value0 = reader0.Read();
            IJsonValue value1 = reader1.Read();

            Assert.NotNull(value0);
            Assert.NotNull(value1);
        }
    }
}
