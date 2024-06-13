using System.Text;
using System.Text.Json;

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
}
