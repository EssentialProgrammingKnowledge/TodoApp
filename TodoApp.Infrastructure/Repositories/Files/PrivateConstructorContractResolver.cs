using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;

namespace TodoApp.Infrastructure.Repositories.Files
{
    internal sealed class PrivateConstructorContractResolver : DefaultJsonTypeInfoResolver
    {
        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

            if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object && jsonTypeInfo.CreateObject is null)
            {
                var hasParameterlessPublicConstructor = jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .Any(c => c.GetParameters().Length == 0);
                if (!hasParameterlessPublicConstructor)
                {
                    // The type doesn't have public constructors
                    jsonTypeInfo.CreateObject = () =>
                        Activator.CreateInstance(jsonTypeInfo.Type, true);
                }
            }

            return jsonTypeInfo;
        }
    }
}
