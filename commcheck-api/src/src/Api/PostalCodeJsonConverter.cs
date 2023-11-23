namespace CommsCheck;

using System.Text.Json;
using System.Text.Json.Serialization;

public class PostalCodeJsonConverter : JsonConverter<PostalCode>
    {
        public override PostalCode Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                PostalCode.Parse(reader.GetString()!);

        public override void Write(
            Utf8JsonWriter writer,
            PostalCode postCodeValue,
            JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("postcode", postCodeValue.PostCode);
                writer.WriteBoolean("isZZ99", postCodeValue.IsZZ99);
                writer.WriteEndObject();
               // writer.WriteStringValue(postCodeValue.PostCode);
            }
    }
