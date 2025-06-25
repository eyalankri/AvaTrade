namespace Common.Settings
{
    public class SqliteSettings
    {
        public string? FilePath { get; init; }

        public string ConnectionString => $"Data Source={FilePath}";
    }
}
