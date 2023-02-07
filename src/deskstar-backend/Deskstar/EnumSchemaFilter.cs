using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Deskstar;

public class EnumSchemaFilter : ISchemaFilter
{
  public void Apply(OpenApiSchema model, SchemaFilterContext context)
  {
    if (context.Type.IsEnum)
    {
      model.Enum.Clear();
      foreach (var enumName in Enum.GetNames(context.Type))
      {
        var memberInfo = context.Type.GetMember(enumName)
          .FirstOrDefault(m => m.DeclaringType == context.Type);
        var enumMemberAttribute = memberInfo == null
          ? null
          : memberInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false).OfType<EnumMemberAttribute>()
            .FirstOrDefault();
        var label = enumMemberAttribute == null || string.IsNullOrWhiteSpace(enumMemberAttribute.Value)
          ? enumName
          : enumMemberAttribute.Value;
        model.Enum.Add(new OpenApiString(label));
      }
    }
  }
}
