using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Utils
{
	public class DateOnlyJsonConverter : JsonConverter<DateOnly>
	{
		public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var stringValue = reader.GetString();
			if (string.IsNullOrEmpty(stringValue))
			{
				throw new JsonException("The date string is null or empty.");
			}

			if (!DateOnly.TryParseExact(stringValue, "yyyy-MM-dd", out var date))
			{
				throw new JsonException($"Unable to parse '{stringValue}' as a DateOnly.");
			}

			return date;
		}

		public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
		}
	}
}
