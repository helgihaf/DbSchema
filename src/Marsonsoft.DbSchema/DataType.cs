namespace Marsonsoft.DbSchema
{
    public record DataType(string Name, string Native, bool Nullable, string? Description = null)
    {
    }
}