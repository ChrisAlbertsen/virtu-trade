using System.Collections.Generic;
using System.Reflection;

namespace Data.DTOs.BaseModels;

public abstract class BaseQueryParamModel
{
    public Dictionary<string, string> ToDictionary()
    {
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var queryParameters = new Dictionary<string, string>();
        foreach (var property in properties)
        {
            var value = property.GetValue(this);
            if (value is not null)
            {
                var propertyName = char.ToLower(property.Name[0]) + property.Name.Substring(1);
                queryParameters.Add(propertyName, value!.ToString());
            }
        }

        return queryParameters;
    }
}