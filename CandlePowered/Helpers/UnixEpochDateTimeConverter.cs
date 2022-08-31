using System.Text.Json;

namespace CandlePowered.Helpers;

internal sealed class UnixEpochDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return UnixEpoch.AddMilliseconds(reader.GetInt64());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var unixTime = Convert.ToInt64((value - UnixEpoch).TotalMilliseconds);

        var formatted = FormattableString.Invariant($"/Date({unixTime})/");
        writer.WriteStringValue(formatted);
    }
}