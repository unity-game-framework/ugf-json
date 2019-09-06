using System;
using NUnit.Framework;
using UGF.Jsonc.Runtime.Values;
using UnityEngine;

namespace UGF.Jsonc.Runtime.Tests
{
    public class TestJsonWriter
    {
        private string m_compact;
        private IJsonValue m_value;

        [SetUp]
        public void Setup()
        {
            m_compact = Resources.Load<TextAsset>("compact").text;
            m_value = new JsonReader(m_compact).Read();
        }

        [Test]
        public void Write()
        {
            var writer = new JsonWriter();

            writer.Write(m_value);

            string result = writer.ToString();

            Assert.AreEqual(m_compact, result);
        }

        [Test]
        public void WriteCircular()
        {
            var value0 = new JsonObject();

            value0["value0"] = value0;

            var writer = new JsonWriter();

            Assert.Throws<ArgumentException>(() => writer.Write(value0), "Cannot write value, because of circular reference in object at 'value0' key.");
        }
    }
}
