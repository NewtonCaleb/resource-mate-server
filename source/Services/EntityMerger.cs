using System.Reflection;
using Namotion.Reflection;

namespace SocialWorkApi.Services;

public class EntityMerger()
{
    public static T Merge<T>(object source, T destination, IEnumerable<string>? excludedProperties = null)
    {
        Type type = typeof(T);
        foreach (PropertyInfo i in type.GetProperties())
        {
            if (excludedProperties != null && excludedProperties.Contains(i.Name)) continue;
            if (!i.CanWrite) continue;

            object? value = source.TryGetPropertyValue<object>(i.Name, null);
            if (value == null) continue;
            
            i.SetValue(destination, value);
        }

        return destination;
    }
}