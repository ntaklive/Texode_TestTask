using System.Reflection;
using AutoMapper.Internal;
using TestTask.Shared.Entities;
using TestTask.Shared.Services.Abstractions;

namespace TestTask.WebApi.Repositories
{
    public sealed class RepositoryContext
    {
        private readonly string _repositoryDirectoryPath;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ILogger<RepositoryContext> _logger;

        public RepositoryContext(
            string repositoryDirectoryPath,
            IJsonSerializer jsonSerializer,
            ILogger<RepositoryContext> logger)
        {
            _repositoryDirectoryPath = repositoryDirectoryPath;
            _jsonSerializer = jsonSerializer;
            _logger = logger;

            EnsureCreated();
            LoadData();
        }

        public ISet<Card> Cards { get; private set; } = null!;

        public ISet<T> Set<T>()
        {
            return (GetDataSetPropertyInfos().First(info =>
                {
                    Type type = info.PropertyType;
                    if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(ISet<>))
                    {
                        return false;
                    }

                    Type itemType = type.GetGenericArguments()[0];
                    return itemType == typeof(T);

                })
                .GetMemberValue(this) as ISet<T>)!;
        }

        public bool SaveChanges()
        {
            try
            {
                foreach (string jsonFilepath in GetJsonsPaths())
                {
                    string dataSetName = Path.GetFileNameWithoutExtension(jsonFilepath);
                    PropertyInfo? propertyInfo = GetDataSetPropertyInfos().FirstOrDefault(x => x.Name == dataSetName);

                    if (propertyInfo == null)
                    {
                        throw new InvalidOperationException(
                            $"It is unable to save the data set named '{dataSetName}' into the '{jsonFilepath}' file");
                    }

                    string json = _jsonSerializer.Serialize(propertyInfo.GetMemberValue(this));
                    File.WriteAllText(jsonFilepath, json);
                }

                return true;
            }
            catch (Exception exception)
            {
                _logger.LogError("Cannot update the configuration");
                _logger.LogCritical(exception, "A critical error was occured");

                return false;
            }
        }

        private void LoadData()
        {
            foreach (string jsonFilepath in GetJsonsPaths())
            {
                string dataSetName = Path.GetFileNameWithoutExtension(jsonFilepath);
                PropertyInfo? propertyInfo = GetDataSetPropertyInfos().FirstOrDefault(x => x.Name == dataSetName);

                if (propertyInfo == null)
                {
                    throw new InvalidOperationException(
                        $"It is unable to load the data set named '{dataSetName}' from the '{jsonFilepath}' file");
                }

                string json = File.ReadAllText(jsonFilepath);
                object? value = _jsonSerializer.Deserialize(json, propertyInfo.PropertyType);

                propertyInfo.SetMemberValue(this, value);
            }
        }

        private void EnsureCreated()
        {
            foreach (string jsonFilepath in GetJsonsPaths())
            {
                if (File.Exists(jsonFilepath))
                {
                    continue;
                }

                File.WriteAllText(jsonFilepath, "[]");
            }
        }

        private IEnumerable<string> GetJsonsPaths()
        {
            return GetDataSetPropertyInfos()
                .Select(info => info.Name)
                .Select(name => Path.Combine(_repositoryDirectoryPath, $"{name}.json"))
                .ToArray();
        }

        private IEnumerable<PropertyInfo> GetDataSetPropertyInfos()
        {
            return typeof(RepositoryContext)
                .GetProperties()
                .Where(info => info.PropertyType.Name == typeof(ISet<>).Name)
                .ToArray();
        }
    }
}