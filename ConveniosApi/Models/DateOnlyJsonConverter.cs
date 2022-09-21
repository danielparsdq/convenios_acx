﻿using Newtonsoft.Json;

namespace ConveniosApi.Models
{
    public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.ReadAsString();
            return DateOnly.Parse(value);
        }

        public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString("O"));
        }
    }
}
