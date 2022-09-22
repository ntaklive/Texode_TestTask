using System.Text.Json;
using TestTask.Shared.Services.Abstractions;

namespace TestTask.Shared.Services;

public class JsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonSerializer(System.Text.Json.JsonSerializerOptions serializerOptions)
    {
        _serializerOptions = serializerOptions;
    }

    public string Serialize<T>(T obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj, _serializerOptions);
    }

    public T Deserialize<T>(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(json, _serializerOptions)!;
    }

    public object? Deserialize(string json, Type returnType)
    {
        return System.Text.Json.JsonSerializer.Deserialize(json, returnType, _serializerOptions);
    }
}