using Red.ToolKit.Extensions.Object;
using Red.ToolKit.Helpers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Red.ToolKit.Utilities;
public static class Json
{
    public static string AddJsonElement(ref string str, string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        JsonObject? json = JsonSerializer.Deserialize<JsonObject>(str);
        json?.Add(key, value);

        return JsonHelper.CastToJson(json);
    }
    public static string RemoveJsonElement(ref string str, string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        JsonObject? json = JsonSerializer.Deserialize<JsonObject>(str);
        json?.Remove(key);
        return JsonHelper.CastToJson(json);
    }
    public static void UpdateJsonElement(ref string str, string key, dynamic value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        _ = value ?? throw new ArgumentException("Value can not be null");

        var json = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(str);

        _ = json ?? throw new ArgumentException("Could not replace value");
        if (json.ContainsKey(key))
            json[key] = value;

        str = json.ToJson();
    }
}

