using NUnit.Framework;
using UGF.Json.Runtime.Values;
using UnityEngine;

namespace UGF.Json.Runtime.Tests
{
    public class TestJsonReader
    {
        [Test]
        public void Test()
        {
            string text = Resources.Load<TextAsset>("readable").text;
            var reader = new JsonReader(text);

            IJsonValue value = reader.Read();

            Assert.NotNull(value);
        }
    }
}
