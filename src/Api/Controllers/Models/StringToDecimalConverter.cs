using System.Text.Json;
using System.Text.Json.Serialization;

public class StringToDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String &&
            decimal.TryParse(reader.GetString(), out var result))
        {
            return result;
        }

        throw new JsonException("Invalid decimal format in JSON.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}