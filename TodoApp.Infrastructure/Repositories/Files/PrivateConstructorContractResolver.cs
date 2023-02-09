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
                    var hasParameterlessPrivateConstructor = jsonTypeInfo.Type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                        .Any(c => c.GetParameters().Length == 0);

                    if (!hasParameterlessPrivateConstructor)
                    {
                        throw new InvalidOperationException($"{type.FullName} has no private constructor");
                    }

                    jsonTypeInfo.CreateObject = () =>
                        Activator.CreateInstance(jsonTypeInfo.Type, true);
                }
            }

            return jsonTypeInfo;
        }
    }
}
