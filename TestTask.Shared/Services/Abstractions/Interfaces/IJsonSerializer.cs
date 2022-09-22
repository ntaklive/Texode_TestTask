namespace TestTask.Shared.Services.Abstractions;

public interface IJsonSerializer
{
    public string Serialize<T>(T obj);
    public T Deserialize<T>(string json);
    public object? Deserialize(string json, Type returnType);
}