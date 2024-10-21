namespace Marsonsoft.DbSchema
{
    public record DataType(string Name, bool Nullable, bool PrimaryKey, IReadOnlyDictionary<string, string> Platform, string? Description = null)
    {
    }
}