using NUnit.Framework;
using UGF.Jsonc.Runtime.Values;

namespace UGF.Jsonc.Runtime.Tests.Values
{
    public class TestJsonValue
    {
        [Test]
        public void Null()
        {
            Assert.AreEqual(JsonValueType.Null, JsonValue.Null.Type);
            Assert.AreEqual("null", JsonValue.Null.Raw);
        }

        [Test]
        public void True()
        {
            Assert.AreEqual(JsonValueType.Boolean, JsonValue.True.Type);
            Assert.AreEqual("true", JsonValue.True.Raw);
        }

        [Test]
        public void False()
        {
            Assert.AreEqual(JsonValueType.Boolean, JsonValue.False.Type);
            Assert.AreEqual("false", JsonValue.False.Raw);
        }

        [Test]
        public void GetBoolean()
        {
            var value0 = new JsonValue(JsonValueType.Boolean, "true");
            var value1 = new JsonValue(JsonValueType.Boolean, "false");

            bool result0 = value0.GetBoolean();
            bool result1 = value1.GetBoolean();

            Assert.True(result0);
            Assert.False(result1);
        }

        [Test]
        public void GetInt32()
        {
            var value0 = new JsonValue(JsonValueType.Number, "10");
            var value1 = new JsonValue(JsonValueType.Number, "15");

            int result0 = value0.GetInt32();
            int result1 = value1.GetInt32();

            Assert.AreEqual(10, result0);
            Assert.AreEqual(15, result1);
        }

        [Test]
        public void GetSingle()
        {
            var value0 = new JsonValue(JsonValueType.Number, "10.5");
            var value1 = new JsonValue(JsonValueType.Number, "15.5e+10");

            float result0 = value0.GetSingle();
            float result1 = value1.GetSingle();

            Assert.AreEqual(10.5F, result0);
            Assert.AreEqual(15.5e+10F, result1);
        }

        [Test]
        public void GetString()
        {
            var value0 = new JsonValue(JsonValueType.String, "text");
            var value1 = new JsonValue(JsonValueType.String, "text\\ntext");

            string result0 = value0.GetString();
            string result1 = value1.GetString();

            Assert.AreEqual("text", result0);
            Assert.AreEqual("text\ntext", result1);
        }

        [Test]
        public void CreateBoolean()
        {
            JsonValue value0 = JsonValue.CreateBoolean(true);
            JsonValue value1 = JsonValue.CreateBoolean(false);

            Assert.AreEqual(JsonValueType.Boolean, value0.Type);
            Assert.AreEqual(JsonValueType.Boolean, value1.Type);
            Assert.AreEqual("true", value0.Raw);
            Assert.AreEqual("false", value1.Raw);
        }

        [Test]
        public void CreateNumberInt32()
        {
            JsonValue value0 = JsonValue.CreateNumber(10);
            JsonValue value1 = JsonValue.CreateNumber(15);

            Assert.AreEqual(JsonValueType.Number, value0.Type);
            Assert.AreEqual(JsonValueType.Number, value1.Type);
            Assert.AreEqual("10", value0.Raw);
            Assert.AreEqual("15", value1.Raw);
        }

        [Test]
        public void CreateNumberSingle()
        {
            JsonValue value0 = JsonValue.CreateNumber(10.5F);
            JsonValue value1 = JsonValue.CreateNumber(1.5e+10F);

            Assert.AreEqual(JsonValueType.Number, value0.Type);
            Assert.AreEqual(JsonValueType.Number, value1.Type);
            Assert.AreEqual("10.5", value0.Raw);
            Assert.AreEqual("1.5E+10", value1.Raw);
        }

        [Test]
        public void CreateString()
        {
            JsonValue value0 = JsonValue.CreateString("text");
            JsonValue value1 = JsonValue.CreateString("text\ntext");

            Assert.AreEqual(JsonValueType.String, value0.Type);
            Assert.AreEqual(JsonValueType.String, value1.Type);
            Assert.AreEqual("text", value0.Raw);
            Assert.AreEqual("text\\ntext", value1.Raw);
        }

        [Test]
        public void Compare()
        {
            var value0 = new JsonValue(JsonValueType.Boolean, "true");
            var value1 = new JsonValue(JsonValueType.Boolean, "true");

            Assert.True(value0 == value1);
            Assert.AreEqual(value0, value1);
        }
    }
}
