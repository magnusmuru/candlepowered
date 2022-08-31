using System.Globalization;
using System.Text.Json;

namespace CandlePowered.Helpers;

public class StringToDecimalConverter : System.Text.Json.Serialization.JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return decimal.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}