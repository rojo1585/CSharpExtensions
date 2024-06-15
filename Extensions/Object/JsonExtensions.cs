using Red.ToolKit.Helpers;
using System.Reflection;

namespace Red.ToolKit.Extensions.Object;

public static class JsonExtensions
{
    private static readonly Dictionary<Type, object> defaultValues = [];
    public static string ToJson<T>(this T obj) where T : class
        => JsonHelper.CastToJson(obj);


    public static T InitializingNullProperties<T>(this T? obj) where T : class
    {
        ArgumentNullException.ThrowIfNull(obj);

        PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (property.GetIndexParameters().Length > 0 || !property.CanWrite)
                continue;

            object? value = property.GetValue(obj);
            Type propertyType = property.PropertyType;

            if (value == null || IsNullableAndDefault(value, propertyType))
            {
                object? defaultValue = GetDefaultValue(propertyType);
                property.SetValue(obj, defaultValue);
            }
        }
        return obj;
    }

    private static object? GetDefaultValue(Type type)
    {
        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;

        if (underlyingType == typeof(string))
            return string.Empty;
        else if (underlyingType.IsValueType)
        {
            if (defaultValues.TryGetValue(underlyingType, out var value))
                return value;

            value = Activator.CreateInstance(underlyingType);
            if (value is not null)
                defaultValues.Add(underlyingType, value);

            return value;
        }
        else
        {
            try
            {
                var instance = Activator.CreateInstance(underlyingType);
                InitializingNullProperties(instance);
                return instance;
            }
            catch
            {
                return null;
            }
        }
    }
    private static bool IsNullableAndDefault(object value, Type type) =>
        value == null && Nullable.GetUnderlyingType(type) != null;



}
