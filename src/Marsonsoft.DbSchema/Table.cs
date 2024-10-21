namespace Marsonsoft.DbSchema
{
    public record Table(string Name, IReadOnlyDictionary<string, Column> Columns, bool IsAbstract = false, Table? BaseTable = null);
}
