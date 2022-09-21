using Newtonsoft.Json;

namespace ConveniosApi.Models
{
    public sealed class DateOnlyNullJsonConverter : JsonConverter<DateOnly?>
    {
        public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
        {
            writer.WriteValue(value.HasValue ? value.Value.ToString("O") : null);
        }

        public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                var fecha = (DateTime)reader.Value;
                return DateOnly.FromDateTime(fecha);
            }

            var value = reader.Value;
            return DateOnly.Parse(value.ToString());
        }
    }
}
