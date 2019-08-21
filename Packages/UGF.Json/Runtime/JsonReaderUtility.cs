using System;
using UGF.Json.Runtime.Values;

namespace UGF.Json.Runtime
{
    public static class JsonReaderUtility
    {
        public static JsonValue ReadString(ref JsonTextReader reader)
        {
            int start = reader.Position;
            int end = 0;

            if (reader.Peek() != '\"')
            {
                throw new Exception("");
            }

            reader.Read();

            while (reader.CanRead())
            {
                char ch = reader.Read();

                switch (ch)
                {
                    case '\\':
                    {
                        reader.Read();
                        break;
                    }
                    case '\n':
                    case '\r':
                    {
                        throw new Exception("");
                    }
                    case '"':
                    {
                        break;
                    }
                    default:
                    {
                        continue;
                    }
                }

                if (ch == '"')
                {
                    break;
                }
            }

            string raw = reader.Text.Substring(start, end - start);

            return new JsonValue(JsonValueType.String, raw);
        }
    }
}
