using System.Text.Json;

namespace Red.ToolKit.Extensions.Object;

public static class JsonExtensions
{
    public static string ToJson(this object obj)
    {
        string jsonString;
        using MemoryStream stream = new();

        JsonSerializer.Serialize(stream, obj);
        stream.Seek(0, SeekOrigin.Begin);

        using (StreamReader sr = new(stream))
        {
            jsonString = sr.ReadToEnd();
        }

        return jsonString;
    }
}
