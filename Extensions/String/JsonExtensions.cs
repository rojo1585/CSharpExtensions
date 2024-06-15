using Red.ToolKit.Helpers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace Red.ToolKit.Extensions.String;

public static class JsonExtensions
{
    public static T? DeserializeJson<T>(this string str)
    {
        try
        {
            T? obj;
            using (MemoryStream stream = new(Encoding.UTF8.GetBytes(str)))
            {
                obj = JsonSerializer.Deserialize<T>(stream);
            }
            return obj;
        }
        catch (JsonException)
        {
            throw new JsonException("No fue posible convertir la cadena");
        }
    }
    public static bool IsValidJson(this string str)
    {
        try
        {
            JsonDocument.Parse(str);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
    public static string AddJsonElement(this string str, string key, string value)
    {
        JsonObject? json = JsonSerializer.Deserialize<JsonObject>(str);
        json?.Add(key, value);

        return JsonHelper.CastToJson(json);
    }
    public static string RemoveJsonElement(this string str, string key)
    {
        JsonObject? json = JsonSerializer.Deserialize<JsonObject>(str);
        json?.Remove(key);
        return JsonHelper.CastToJson(json);
    }
}
