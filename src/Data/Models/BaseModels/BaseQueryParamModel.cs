namespace Data.Models.BaseModels;

public abstract class BaseQueryParamModel
{
    public Dictionary<string, string> ToDictionary()
    {
        var properties = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        
        var queryParameters = new Dictionary<string, string>();
        foreach (var property in properties)
        {
            var value = property.GetValue(this);
            if (value is not null)
            {
                queryParameters.Add(property.Name, value!.ToString());
            }

        }

        return queryParameters;
    }
}