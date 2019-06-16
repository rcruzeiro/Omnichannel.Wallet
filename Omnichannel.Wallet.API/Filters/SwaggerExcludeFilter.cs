using System.Linq;
using System.Reflection;
using Omnichannel.Wallet.API.Attributes;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Omnichannel.Wallet.API.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null) return;

            var excludedProperties =
                context.SystemType.GetProperties().Where(
                    t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

            foreach (PropertyInfo excludedProperty in excludedProperties)
            {
                if (schema.Properties.ContainsKey(excludedProperty.Name.ToLower()))
                    schema.Properties.Remove(excludedProperty.Name.ToLower());
            }
        }
    }
}
